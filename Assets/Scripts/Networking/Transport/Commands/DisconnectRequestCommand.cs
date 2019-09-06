using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectRequestCommand : INetworkCommand
{
    public void ExecuteCommand()
    {
        GnetBase.Gnet.ExecuteDisconnectRequest();
    }
}
