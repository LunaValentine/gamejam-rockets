using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMover : MonoBehaviour
{
    bool moveUp = true;

    // Update is called once per frame
    void Update()
    {
        if (moveUp)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + (1 * Time.deltaTime), transform.position.z);
            if (transform.position.y > 4)
                moveUp = false;
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + (-1 * Time.deltaTime), transform.position.z);
            if (transform.position.y < -2)
                moveUp = true;
        }
    }
}
