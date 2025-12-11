using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public  AudioSource Audio;  
    public float motorTorque = 15000f;      
    public float brakeForce = 10000f;        
    public float maxSteerAngle = 90f;
    public float maxSteerspeed = 10;

    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    public Transform frontLeftTransform;
    public Transform frontRightTransform;
    public Transform rearLeftTransform;
    public Transform rearRightTransform;

    private float currentBrakeForce;
    private float currentSteerAngle;
    private float currentSteerSpeed;

    private CarState currentCarState;

    public enum CarState
    {
        idle,
        inMovement,
        breaked,
        reverse
    }

    

    private void Awake()
    {
        currentCarState = CarState.idle;
    }

    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");   
        float horizontalInput = Input.GetAxisRaw("Horizontal"); 

        if(verticalInput != 0)
        {
            currentCarState = CarState.inMovement;
        }
        else if (verticalInput == 0)
        {
            currentCarState = CarState.idle;
            currentBrakeForce = brakeForce;
            ApplyBrake(currentBrakeForce);
        }

        Debug.Log("Horizontal Input..." + horizontalInput);

        if (currentCarState == CarState.inMovement)
        {   
            HandleMotor(verticalInput);
            HandleSteering(horizontalInput);
            UpdateWheels();
        }
    }

    private void HandleMotor(float verticalInput)
    {
       
        rearLeftWheel.motorTorque = verticalInput * motorTorque;
        rearRightWheel.motorTorque = verticalInput * motorTorque;

        
        if (Input.GetKey(KeyCode.Space))
        {
            currentBrakeForce = brakeForce;
        }
        else
        {
            currentBrakeForce = 0f;
        }

        ApplyBrake(currentBrakeForce);
    }

    private void HandleSteering(float horizontalInput)
    {
        horizontalInput = Mathf.Clamp(horizontalInput, -1, 1);
        currentSteerAngle = maxSteerAngle * horizontalInput;
        currentSteerAngle=maxSteerspeed * horizontalInput;
      
        frontLeftWheel.steerAngle = currentSteerAngle;
        frontRightWheel.steerAngle = currentSteerAngle;
    }

    private void ApplyBrake(float brakeForce)
    {
        frontLeftWheel.brakeTorque = brakeForce;
        frontRightWheel.brakeTorque = brakeForce;
        rearLeftWheel.brakeTorque = brakeForce;
        rearRightWheel.brakeTorque = brakeForce;
    }

    private void UpdateWheels()
    {
       if(currentCarState == CarState.inMovement)
        {
            UpdateWheelPose(frontLeftWheel, frontLeftTransform);
            UpdateWheelPose(frontRightWheel, frontRightTransform);
            UpdateWheelPose(rearLeftWheel, rearLeftTransform);
            UpdateWheelPose(rearRightWheel, rearRightTransform);
        }
    }

    private void UpdateWheelPose(WheelCollider col, Transform trans)
    {
        Vector3 pos;
        Quaternion rot;
        col.GetWorldPose(out pos, out rot);
        trans.position = pos;
        trans.rotation = rot;
    }
}
