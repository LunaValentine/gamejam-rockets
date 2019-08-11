using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

class GnetClient : MonoBehaviour
{
    [Header("IP info (only available when not running)")]
    public int ServerPort;
    public string ServerIp;

    private IPEndPoint endpoint;

    private UdpClient ListenerClient;

    public Action<EndPoint> NewConnectionReceived;

    public int ListenPort = 26234;
    public uint PacketNumber = 0;

    private byte[] message = new byte[IoMap.Size + GnetBase.HeaderSize];
    private int messageIndex = 0;
    public uint LastReceived = 0;

    public GnetClient()
    {
        ListenerClient = new UdpClient(ListenPort);
        endpoint = new IPEndPoint(IPAddress.Parse(ServerIp), ServerPort);
    }

    public void ReceiveAll()
    {
        while (ListenerClient.Available > 0)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
            Byte[] message = ListenerClient.Receive(ref endpoint);

            //This is one of ours
            if (BitConverter.ToUInt32(message, 0) == GnetBase.PROTOCOL_ID && ep == endpoint)
            {
                uint packetNumber = BitConverter.ToUInt32(message, 4);

                //If the message is more recent than the last we received then we should use it....




                //TODO This is WRONG
                //what the server does... not client
                if (SequenceMoreRecent(BitConverter.ToUInt32(message, 4), LastReceived))
                {
                    byte[] buffer = new byte[message.Length - GnetBase.HeaderSize];
                    Buffer.BlockCopy(message, GnetBase.HeaderSize, buffer, 0, buffer.Length);
                    var map = new IoMap(buffer);
                }
            }
        }
    }

    public void Send(byte[] messageContents)
    {
        //0Protocol_Id, 4MessageNum, 8Acks
        //Protocol_ID
        Buffer.BlockCopy(GnetBase.PROTOCOL_ID, 0, message, 0, 4);
        //PacketNumber
        Buffer.BlockCopy(PacketNumber, 0, message, 4, 4);
        //LastReceived
        Buffer.BlockCopy(LastReceived, 0, message, 8, 4);

        //Then the messageContents
        Buffer.BlockCopy(messageContents, 0, message, 12, messageContents.Length);

        ListenerClient.Send(message, message.Length);
    }

    public void Close()
    {
        //Send TearDown to All EndPoints
        ListenerClient.Close();
        ListenerClient = null;
    }

    public void Receive(IPEndPoint endpoint, Byte[] message)
    {
        //EndpointLastReceived[endpoint] = BitConverter.ToUInt32(message, 4);

        //TODO process the actual Input
    }

    bool SequenceMoreRecent(uint s1, uint s2)
    {
        return (s1 > s2) && (s1 - s2 <= uint.MaxValue / 2) || (s2 > s1) && (s2 - s1 > uint.MaxValue / 2);
    }

    public void Push(IoMap io)
    {
        //Pack the IoMap
        byte[] map = io.Pack();

        //Send the Packet
        Send(map);
    }
}
