using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetBehaviour : MonoBehaviour
{
    // Use this for initialization
    void Awake()
    {
        if (ServerController.Instance == null)
        {
            enabled = false;
        }
        else
        {
            gameObject.AddComponent<NetSync>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
    
    }
}
