using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct IoMap
{
    //If this file is ever updated then this value also needs to be updated....
    //14 + 6*4 = 36
    public static int Size = 36;

    public bool A;
    public bool B;
    public bool X;
    public bool Y;
    public bool Select;
    public bool Start;
    public bool LB;
    public bool RB;

    public bool Left;
    public bool Right;
    public bool Up;
    public bool Down;

    public bool LeftStick;
    public bool RightStick;

    public float LeftAxisVertical;
    public float LeftAxisHorizontal;
    public float RightAxisVertical;
    public float RightAxisHorizontal;

    public float LT;
    public float RT;

    public IoMap(byte[] buffer)
    {
        MyBitStream stream = new MyBitStream(buffer);
        A = stream.ReadBool();
        B = stream.ReadBool();
        X = stream.ReadBool();
        Y = stream.ReadBool();
        Select = stream.ReadBool();
        Start = stream.ReadBool();
        LB = stream.ReadBool();
        RB = stream.ReadBool();

        Left = stream.ReadBool();
        Right = stream.ReadBool();
        Up = stream.ReadBool();
        Down = stream.ReadBool();

        LeftStick = stream.ReadBool();
        RightStick = stream.ReadBool();

        stream.ReadBool();
        stream.ReadBool();

        LeftAxisVertical = stream.ReadFloat();
        LeftAxisHorizontal = stream.ReadFloat();
        RightAxisVertical = stream.ReadFloat();
        RightAxisHorizontal = stream.ReadFloat();

        LT = stream.ReadFloat();
        RT = stream.ReadFloat();
    }

    public byte[] Pack()
    {
        MyBitStream stream = new MyBitStream(Size);

        stream.PackBool(A);
        stream.PackBool(B);
        stream.PackBool(X);
        stream.PackBool(Y);
        stream.PackBool(Select);
        stream.PackBool(Start);
        stream.PackBool(LB);
        stream.PackBool(RB);

        stream.PackBool(Left);
        stream.PackBool(Right);
        stream.PackBool(Up);
        stream.PackBool(Down);

        stream.PackBool(LeftStick);
        stream.PackBool(RightStick);

        stream.PackBool(false);
        stream.PackBool(false);

        stream.PackFloat(LeftAxisVertical);
        stream.PackFloat(LeftAxisHorizontal);
        stream.PackFloat(RightAxisVertical);
        stream.PackFloat(RightAxisHorizontal);

        stream.PackFloat(LT);
        stream.PackFloat(RT);

        return stream.GetUnderlyingArray();
    }
}
