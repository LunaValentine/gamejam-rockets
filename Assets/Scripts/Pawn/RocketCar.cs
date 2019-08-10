using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketCar : MonoBehaviour
{
    public float turnSpeed, forwardSpeed, maxSpeed;
    public GameObject pawn;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float moveForward = Input.GetAxis("Vertical");

        float moveRight = Input.GetAxis("Horizontal");

        Vector3 updatedMove = transform.position;

        float x = updatedMove.x ;
        float y = updatedMove.y;
        float z = updatedMove.z + (moveForward * forwardSpeed * Time.deltaTime);

        updatedMove = new Vector3(x, y, z);

        transform.position = updatedMove;

    }


}
