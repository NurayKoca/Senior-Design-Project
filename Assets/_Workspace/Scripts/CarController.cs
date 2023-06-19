using System;
using _Workspace.ScriptableObjects;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;

namespace _Workspace.Scripts
{
    public class CarController : NetworkBehaviour
    {
        #region Variables
        
        [SerializeField] MeshRenderer carRenderer;
        
        [Header("Car Movement Variables")]

        private float _horizontalInput;
        private float _verticalInput;
        private bool _isBreaking;
        private float _currentBreakForce;
        private float _currentSteerAngle;
        
        [SerializeField] private  float _maxSteerAngle = 30f;
        [SerializeField] private  float motorForce = 50f;
        [SerializeField] private  float breakForce = 50f;
        
        [SerializeField] WheelCollider frontLeftWheelCollider;
        [SerializeField] WheelCollider frontRightWheelCollider;
        [SerializeField] WheelCollider rearLeftWheelCollider;
        [SerializeField] WheelCollider rearRightWheelCollider;
        
        
        [SerializeField] Transform frontLeftWheelTransform;
        [SerializeField] Transform frontRightWheelTransform;
        [SerializeField] Transform rearLeftWheelTransform;
        [SerializeField] Transform rearRightWheelTransform;
        
        
        private CinemachineVirtualCamera _carCamera;

        private Rigidbody _rigidbody;

        private GameInput _gameInput;

        #endregion


        #region Setting Car

        public void SetCarSo(CarDataHolder carDataSo)
        {
            carRenderer.material = carDataSo.carMaterial;
        }

        #endregion


        #region Unity Funcs

        private void Awake()
        {
            
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (!IsOwner)
            {
                enabled = false;
                return;
            }
            

            transform.name = IsHost ? "Host" : "Client";

            // Setting Rigidbody Center Of Mass
            _rigidbody = GetComponent<Rigidbody>();
            var curCenter =_rigidbody.centerOfMass;
            curCenter.y = -1;
            _rigidbody.centerOfMass = curCenter;

            // Setting Camera
            _carCamera = GameObject.FindGameObjectWithTag("CarCamera").GetComponent<CinemachineVirtualCamera>();
            _carCamera.m_Follow = transform;
            _carCamera.m_LookAt = transform;

            // Setting Input System
            _gameInput = new GameInput();
        }

        private void FixedUpdate()
        {
            if (!IsOwner)
            {
                return;
            }
            
            GetInput();
            HandleMotor();
            HandleSteering();
            UpdateWheels();
        }

        #endregion
        

        #region Car Movement

        private void GetInput()
        {
            
            _horizontalInput = _gameInput.GetMovementDirection().x;
            _verticalInput = _gameInput.GetMovementDirection().y;
            _isBreaking = _gameInput.GetBrakeStatus();
            
            //Debug.Log($"Horizontal : {_horizontalInput} // Vertical : {_verticalInput} // Breaking : {_isBreaking}");
        }
        
        private void HandleMotor()
        {
            frontLeftWheelCollider.motorTorque = _verticalInput * motorForce;
            frontRightWheelCollider.motorTorque = _verticalInput * motorForce;
            _currentBreakForce = _isBreaking ? breakForce : 0f;
            ApplyBreaking();
            
            //Debug.Log("Handling Motor");
        }

        private void ApplyBreaking()
        {
            frontRightWheelCollider.brakeTorque = _currentBreakForce;
            frontLeftWheelCollider.brakeTorque = _currentBreakForce;
            rearLeftWheelCollider.brakeTorque = _currentBreakForce;
            rearRightWheelCollider.brakeTorque = _currentBreakForce;
            
            //Debug.Log("Applying Breaking");
        }

        private void HandleSteering()
        {
            _currentSteerAngle = _maxSteerAngle * _horizontalInput;
            frontLeftWheelCollider.steerAngle = _currentSteerAngle;
            frontRightWheelCollider.steerAngle = _currentSteerAngle;
            
            
            //Debug.Log("Handling Steering");
        }

        private void UpdateWheels()
        {
            UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
            UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
            UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
            UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
            
            
            //Debug.Log("Updating Wheels");
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
        
        
    }
}
