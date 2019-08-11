using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketCar : MonoBehaviour
{
    public float turnSpeed, forwardSpeed;
    public float accelerationPower = 160;
    public float brakingPower = 60;
    float currentSpeed = 0;
    public float maximumGroundSpeed = 30;
    public float rotationStrength = 10;
    float rotationThisFrame;

    private void FixedUpdate()
    {
        //Get input this frame
        float moveForward = Input.GetAxis("Vertical");
        float moveRight = Input.GetAxis("Horizontal");
        float throttle = Input.GetAxis("Throttle");
        

        //Adjust speed or swap to braking behaviour
        //TODO: Add a hard brake function for when player is hitting reverse throttle while moving forward and vice versa
        if(throttle!=0)
        {
            Accelerate(throttle);
        }
        else
        {
            SoftBrake();  
        }


        //Steering. Flip steering rotation if moving backwards
        if(currentSpeed > 0)
        {
            rotationThisFrame = rotationStrength * Time.deltaTime * moveRight * currentSpeed;
        }
        else
        {
            rotationThisFrame = rotationStrength * Time.deltaTime * moveRight * currentSpeed * -1;
        }
        
        //Apply Rotation
        transform.Rotate(0, rotationThisFrame, 0);

        //Apply speed
        transform.position = transform.position + (transform.forward * currentSpeed * Time.deltaTime);

    }



    public void Accelerate(float iThrottle)
    {
        currentSpeed = currentSpeed + (iThrottle * accelerationPower * Time.deltaTime);
        currentSpeed = Mathf.Clamp(currentSpeed, -maximumGroundSpeed, maximumGroundSpeed);
    }

    public void SoftBrake()
    {
        if(currentSpeed==0)
        {
            return;
        }

        if (currentSpeed > 0)
        {
            currentSpeed = currentSpeed - brakingPower * Time.deltaTime;
            Mathf.Clamp(currentSpeed, 0, maximumGroundSpeed);
        }
        else
        {
            currentSpeed = currentSpeed + brakingPower * Time.deltaTime;
            Mathf.Clamp(currentSpeed, -maximumGroundSpeed, 0);
        }
    }
}
