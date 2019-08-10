using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct NetBState
{
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Velocity;

    public NetBState(Vector3 pos, Quaternion rot)
    {
        Position = pos;
        Rotation = rot;
        Velocity = Vector3.Zero;
    }

    public NetBState(Vector3 pos, Quaternion rot, Vector3 vel)
    {
        Position = pos;
        Rotation = rot;
        Velocity = vel;
    }
}