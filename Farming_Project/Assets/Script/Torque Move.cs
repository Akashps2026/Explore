using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public  AudioSource Audio;  
    public float motorTorque = 15000f;       // Power to move
    public float brakeForce = 10000f;        // Braking power
    public float maxSteerAngle = 90f;
    public float maxSteerspeed = 10;// Maximum steer angle

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

    //public bool isEnemy = false;

    void FixedUpdate()
    {
        float verticalInput = Input.GetAxis("Vertical");   // W/S or Up/Down
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right

        Debug.Log("Horizontal Input..." +horizontalInput);

        HandleMotor(verticalInput);
        HandleSteering(horizontalInput);
        UpdateWheels();
    }

    private void HandleMotor(float verticalInput)
    {
        // Apply torque to rear wheels
        rearLeftWheel.motorTorque = verticalInput * motorTorque;
        rearRightWheel.motorTorque = verticalInput * motorTorque;

        // Braking
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
       // Debug.Log("Current Steer Angle....." + currentSteerAngle);
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
        UpdateWheelPose(frontLeftWheel, frontLeftTransform);
        UpdateWheelPose(frontRightWheel, frontRightTransform);
        UpdateWheelPose(rearLeftWheel, rearLeftTransform);
        UpdateWheelPose(rearRightWheel, rearRightTransform);
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
