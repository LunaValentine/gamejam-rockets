using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NetBehaviour : MonoBehaviour
{
    public bool Server;

    private bool _fresh;
    private Vector3 Velocity;
    //private Quaternion RotationalVelocity;

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
        if(Server)
        {
            ServerFixedUpdate();
        }
        else
        {
            ClientFixedUpdate();
        }
    }

    //Is this game logic?
    protected abstract void ServerFixedUpdate();

    //Is this client interpolation
    protected void ClientFixedUpdate()
    {
        if(_fresh)
        {
            _fresh = false;
        }
        else
        {
            transform.position += Velocity;
        }
    }

    protected void FreshPush(MyBitStream stream)
    {
        transform.position = stream.ReadVector3();
        transform.rotation = stream.ReadQuaternion();

        Velocity = stream.ReadVector3();
        _fresh = true;
    }
}
