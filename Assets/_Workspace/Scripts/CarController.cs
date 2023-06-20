using _Workspace.ScriptableObjects;
using Cinemachine;
using TMPro.Examples;
using Unity.Netcode;
using UnityEngine;
using Workspace.Scripts;

namespace _Workspace.Scripts
{
    public class CarController : NetworkBehaviour
    {
        #region Variables

        [SerializeField] private MeshRenderer carRenderer;

        [Header("Car Movement Variables")]
        private float _horizontalInput;
        private float _verticalInput;
        private bool _isBreaking;
        private float _currentBreakForce;
        private float _currentSteerAngle;

        [SerializeField] private float _maxSteerAngle = 30f;
        [SerializeField] private float motorForce = 50f;
        [SerializeField] private float breakForce = 50f;
        private bool _canMove;

        [SerializeField] private WheelCollider frontLeftWheelCollider;
        [SerializeField] private WheelCollider frontRightWheelCollider;
        [SerializeField] private WheelCollider rearLeftWheelCollider;
        [SerializeField] private WheelCollider rearRightWheelCollider;

        [SerializeField] private Transform frontLeftWheelTransform;
        [SerializeField] private Transform frontRightWheelTransform;
        [SerializeField] private Transform rearLeftWheelTransform;
        [SerializeField] private Transform rearRightWheelTransform;

        private CinemachineVirtualCamera _carCamera;
        private Rigidbody _rigidbody;
        private GameInput _gameInput;
        public CarDataIndex carDataIndex;

        private int _currentCarDataIndex;

        [Header("MINI MAP")]
        [SerializeField] private SpriteRenderer miniMapSpriteRenderer;
        [SerializeField] private Sprite allieSpriteMiniMap;
        private CameraController _miniMapCameraController;
        
        private MapController _activeMapController;

        #endregion

        #region Setting Car

        [ClientRpc]
        private void SetCarSo_ClientRpc(int carDataSoIndex)
        {
            carRenderer.material = carDataIndex.GetCarDataSoByIndex(carDataSoIndex).carMaterial;
            _currentCarDataIndex = carDataSoIndex;
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetCarSo_ServerRpc(int carDataSoIndex)
        {
            SetCarSo_ClientRpc(carDataSoIndex);
        }

        public void SetCarSo(int index)
        {
            if (!IsOwner) return;

            SetCarSo_ServerRpc(index);

            transform.position = _activeMapController.GetEmptyStartPosition();
        }

        #endregion

        #region Unity Funcs

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (!IsOwner)
            {
                enabled = false;
                return;
            }

            _activeMapController = FindObjectOfType<MapController>();

            transform.name = IsHost ? "Host" : "Client";

            transform.position = _activeMapController.GetEmptyCarSpawnPosition();

            // Setting Rigidbody Center Of Mass
            _rigidbody = GetComponent<Rigidbody>();
            var curCenter = _rigidbody.centerOfMass;
            curCenter.y = -1;
            _rigidbody.centerOfMass = curCenter;
            _rigidbody.isKinematic = true;

            // Setting Camera
            _carCamera = GameObject.FindGameObjectWithTag("CarCamera").GetComponent<CinemachineVirtualCamera>();
            _carCamera.m_Follow = transform;
            _carCamera.m_LookAt = transform;

            // Setting Input System
            _gameInput = new GameInput();

            // Setting Mini Map Icon
            miniMapSpriteRenderer.sprite = allieSpriteMiniMap;
            _miniMapCameraController = FindObjectOfType<CameraController>();
            _miniMapCameraController.CameraTarget = transform;
            
            // Setting Speedometer
            _speedoMeterUIController = FindObjectOfType<SpeedoMeterUIController>();

            CarSelectAreaController.OnCarSelected += SetCarSo;
            NetworkManager.OnClientConnectedCallback += NetworkManagerOnOnClientConnectedCallback;
            GameManager.instance._playersCanMove.OnValueChanged += PlayersCanMoveOnValueChanged;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            CarSelectAreaController.OnCarSelected -= SetCarSo;
            NetworkManager.OnClientConnectedCallback -= NetworkManagerOnOnClientConnectedCallback;
            GameManager.instance._playersCanMove.OnValueChanged -= PlayersCanMoveOnValueChanged;
        }

        private void NetworkManagerOnOnClientConnectedCallback(ulong obj)
        {
            SetCarSo_ServerRpc(_currentCarDataIndex);
        }
        private void PlayersCanMoveOnValueChanged(bool previousvalue, bool newvalue)
        {
            _canMove = newvalue;

            if (newvalue)
                _rigidbody.isKinematic = false;
        }

        private void FixedUpdate()
        {
            if (!IsOwner) return;
            if(!_canMove) return;

            GetInput();
            HandleMotor();
            HandleSteering();
            UpdateWheels();
            SetSpeed();
        }

        #endregion

        #region Car Movement

        private void GetInput()
        {
            _horizontalInput = _gameInput.GetMovementDirection().x;
            _verticalInput = _gameInput.GetMovementDirection().y;
            _isBreaking = _gameInput.GetBrakeStatus();
        }

        private void HandleMotor()
        {
            frontLeftWheelCollider.motorTorque = _verticalInput * motorForce;
            frontRightWheelCollider.motorTorque = _verticalInput * motorForce;
            _currentBreakForce = _isBreaking ? breakForce : 0f;
            ApplyBreaking();
        }

        private void ApplyBreaking()
        {
            frontRightWheelCollider.brakeTorque = _currentBreakForce;
            frontLeftWheelCollider.brakeTorque = _currentBreakForce;
            rearLeftWheelCollider.brakeTorque = _currentBreakForce;
            rearRightWheelCollider.brakeTorque = _currentBreakForce;
        }

        private void HandleSteering()
        {
            _currentSteerAngle = _maxSteerAngle * _horizontalInput;
            frontLeftWheelCollider.steerAngle = _currentSteerAngle;
            frontRightWheelCollider.steerAngle = _currentSteerAngle;
        }

        private void UpdateWheels()
        {
            UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
            UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
            UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
            UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        }

        private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
        {
            Vector3 pos;
            Quaternion rot;
            wheelCollider.GetWorldPose(out pos, out rot);
            wheelTransform.rotation = rot;
            wheelTransform.position = pos;
        }

        #endregion

        #region SpeedoMeter

        private float _prevSpeed = 0;
        private float _curSpeed = 0;
        private SpeedoMeterUIController _speedoMeterUIController;

        private void SetSpeed()
        {
            _curSpeed = _rigidbody.velocity.magnitude*6;
            UpdateSpeedometerUI();
            _prevSpeed = _curSpeed;
        }
        private void UpdateSpeedometerUI()
        {
            if(_speedoMeterUIController == null) return;
            
            _speedoMeterUIController.SetSpeed(Mathf.Abs(_prevSpeed),Mathf.Abs(_curSpeed));
            _speedoMeterUIController.SetSpeedHand(-Mathf.Abs(_curSpeed));
        }

        #endregion
    }
}
