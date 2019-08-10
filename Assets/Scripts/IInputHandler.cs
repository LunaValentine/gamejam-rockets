using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputHandler
{
    void Push(IoMap io);

    IoMap Poll();
}
