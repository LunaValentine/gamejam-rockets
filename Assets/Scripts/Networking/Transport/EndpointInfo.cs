using UnityEngine;
using System.Collections;
using System.Net;
using System.Collections.Generic;

public class EndpointInfo
{
    public int Index;

    public AcknowledgementBundle AcksToSend = new AcknowledgementBundle(0, 0);
    public uint SendingMessageNum = 1;
    //Window

    public IPEndPoint endpoint;
    public Queue<byte[]> MessagesFrom = new Queue<byte[]>();

    public EndpointInfo(int i, IPEndPoint ep)
    {
        Index = i;
        endpoint = ep;
    }
}