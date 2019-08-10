using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private IoMap _playerBuffer;
    public bool NoNetwork;
    public ClientController client;

    //If Client Controller exists and is enabled then we need to send input to server

    //If Server Controller Exists and is enabled then we need to receive from the server

    //This is where we take the Input from the InputManager and map it to the DataStructure

    void FixedUpdate()
    {
        //If we are server do nothing
        if(client != null || NoNetwork)
        {
            var map = new IoMap();

            //Fill out the IoMap

            if(NoNetwork)
            {
                Push(map);
            }
            else
            {
                //If client then send to server
            }
        }
    }

    public void Push(IoMap map)
    {
        _playerBuffer = map;
    }

    public IoMap Poll()
    {
        return _playerBuffer;
    }
}
