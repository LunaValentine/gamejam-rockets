using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketCar : MonoBehaviour
{
    public float turnSpeed, forwardSpeed;
    public float accelerationPower = 10;
    public float brakingPower = 5;
    //Vector3 velocity = new Vector3();
    float currentSpeed = 0;
    float maxWheelTurn = 45;
    float maximumGroundSpeed = 20;


    private void FixedUpdate()
    {
        //Get input this frame
        float moveForward = Input.GetAxis("Vertical");
        float moveRight = Input.GetAxis("Horizontal");
        float throttle = Input.GetAxis("Throttle");
        
        if(throttle!=0)
        {
            currentSpeed = currentSpeed + (throttle*accelerationPower*Time.deltaTime);
            currentSpeed = Mathf.Clamp(currentSpeed, -maximumGroundSpeed, maximumGroundSpeed);
        }
        else
        {

            currentSpeed = currentSpeed - (brakingPower * Time.deltaTime);
            currentSpeed = Mathf.Clamp(currentSpeed, 0, maximumGroundSpeed);
        }
     

        Vector3 rotatedVector = Quaternion.AngleAxis((maxWheelTurn*moveRight), Vector3.up) * transform.forward;

        Vector3 targetPosition = (rotatedVector * currentSpeed * Time.deltaTime) + transform.position;
        transform.LookAt(targetPosition,Vector3.up);
        transform.position = targetPosition;
        
        
    }


}
