using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerController : MonoBehaviour
{
    private static ServerController _instance;
    public static ServerController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Object.FindObjectOfType<ServerController>();
            }
            return _instance;
        }
    }

    public int Port;
    public 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
