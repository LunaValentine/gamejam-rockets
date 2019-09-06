using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCommandController
{
    enum CommandTypes
    {
        disconnectRequest = 0
    }

    private static List<INetworkCommand> CommandsToSend;
    //////////////////////////////////////////////////////////////////////////////////////////////////

    public static INetworkCommand[] GetCommandsFromStream(MyBitStream stream)
    {
        int count = (int)(stream.ReadByte());
        INetworkCommand[] commands = new INetworkCommand[count];
        for(int i = 0; i < count; i++)
        {
            byte commandType = stream.ReadByte();
            switch((CommandTypes)commandType)
            {
                case CommandTypes.disconnectRequest:
                    commands[i] = new DisconnectRequestCommand();
                    break;
                default:
                    break;
            }
        }

        return commands;
    }
}
