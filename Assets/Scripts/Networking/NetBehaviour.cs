using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NetBehaviour : MonoBehaviour
{
    public static int PackedSize = sizeof(float) * (3 + 4 + 3);

    //Did I just get set this frame
    private bool _fresh = true;

    private Vector3 _velocity = Vector3.zero;
    //private Quaternion RotationalVelocity;

    // Use this for initialization
    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if(GnetServer.Server)
        {
            var pastLocation = transform.position;
            ServerFixedUpdate();
            //Update Velocity after we ServerUpdate
            _velocity = transform.position - pastLocation;
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
            transform.position += _velocity;
        }
    }

    public void FreshPush(MyBitStream stream)
    {
        transform.position = stream.ReadVector3();
        transform.rotation = stream.ReadQuaternion();

        _velocity = stream.ReadVector3();
        _fresh = true;
    }

    public byte[] Pack()
    {
        MyBitStream stream = new MyBitStream(PackedSize); ;
        stream.PackVector3(transform.position);
        stream.PackQuaternion(transform.rotation);
        stream.PackVector3(_velocity);

        return stream.GetUnderlyingArray();
    }

}
