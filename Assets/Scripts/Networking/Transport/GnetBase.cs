using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GnetBase
{
    //0Protocol_Id, 4MessageNum, 8Acks
    public const int HeaderSize = 12;

    public const int PROTOCOL_ID = 73629579;

    public static IGnet Gnet;
}
