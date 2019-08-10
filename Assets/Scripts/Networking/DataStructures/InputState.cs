﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MoveSet
{
    public bool Up, Right, Down, Left, Jump;
    public short MouseX, MouseY;

    public void Pack(MyBitStream stream)
    {
        stream.PackBool(Up);
        stream.PackBool(Right);
        stream.PackBool(Down);
        stream.PackBool(Left);
        stream.PackBool(Jump);
        stream.PackInt(MouseX);
        stream.PackInt(MouseY);
    }

    public MoveSet(MyBitStream stream)
    {
        //UnpackBools too
        Up = stream.ReadBool();
        Right = stream.ReadBool();
        Down = stream.ReadBool();
        Left = stream.ReadBool();
        Jump = stream.ReadBool();

        MouseX = (short)stream.ReadInt();
        MouseY = (short)stream.ReadInt();
    }

    public MoveSet(bool bUp, bool bRight, bool bDown, bool bLeft, bool bJump, short mouseX, short mouseY)
    {
        Up = bUp;
        Right = bRight;
        Down = bDown;
        Left = bLeft;
        Jump = bJump;
        MouseX = mouseX;
        MouseY = mouseY;
    }
}