using System;
using _Workspace.ScriptableObjects;
using UnityEngine;

namespace _Workspace.Scripts
{
    public class CarController : MonoBehaviour
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


        private Rigidbody _rigidbody;

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
            _rigidbody = GetComponent<Rigidbody>();

            var curCenter =_rigidbody.centerOfMass;
            curCenter.y = -1;
            _rigidbody.centerOfMass = curCenter;
        }

        private void FixedUpdate()
        {
            GetInput();
            HandleMotor();
            HandleSteering();
            UpdateWheels();
            //frontLeftWheelCollider.motorTorque = 8.174 * motorForce;
        }

        #endregion
        

        #region Car Movement

        private void GetInput()
        {
            _horizontalInput = Input.GetAxis("Horizontal");
            _verticalInput = Input.GetAxis("Vertical");
            _isBreaking = Input.GetKey(KeyCode.Space);
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
        
        
    }
}
