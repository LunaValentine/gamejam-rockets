using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetBehaviour : MonoBehaviour
{
    Vector3 Velocity { get; set; }

    // Use this for initialization
    void Awake()
    {
        /*if (ServerController.Instance == null)
        {
            enabled = false;
        }*/
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
