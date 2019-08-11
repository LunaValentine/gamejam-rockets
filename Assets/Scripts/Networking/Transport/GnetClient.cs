using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class GnetClient : MonoBehaviour
{
    private static GnetClient _instance;
    public static GnetClient Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = UnityEngine.Object.FindObjectOfType<GnetClient>();
            }
            return _instance;
        }
    }

    public GameObject PlayerPrefab;

    [Header("IP info (only available when not running)")]
    public int ServerPort;
    public string ServerIp;

    private IPEndPoint endpoint;

    private UdpClient ListenerClient;

    public Action<EndPoint> NewConnectionReceived;

    public int ListenPort = 26234;
    public uint PacketNumber = 0;

    private byte[] message = new byte[IoMap.Size + GnetBase.HeaderSize];
    public uint LastReceived = 0;

    void OnEnable()
    {
        ListenerClient = new UdpClient(ListenPort);
        endpoint = new IPEndPoint(IPAddress.Parse(ServerIp), ServerPort);

        //Create a player
        Instantiate(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
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
        Buffer.BlockCopy(BitConverter.GetBytes(GnetBase.PROTOCOL_ID), 0, message, 0, 4);
        //PacketNumber
        Buffer.BlockCopy(BitConverter.GetBytes(PacketNumber), 0, message, 4, 4);
        //LastReceived
        Buffer.BlockCopy(BitConverter.GetBytes(LastReceived), 0, message, 8, 4);

        //Then the messageContents
        Buffer.BlockCopy(messageContents, 0, message, 12, messageContents.Length);

        ListenerClient.Send(message, message.Length, endpoint);
    }

    public void OnDisable()
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
        Debug.Log(map[0] + " " + map[1] + " " + map[2] + " " + map[3]+ " " + map[4] + " " + map[5] + " " + map[6] + " " + map[7] + " " +
            map[8] + " " + map[9] + " " + map[10] + " " + map[11] + " " + map[12] + " " + map[13] + " " + map[14] + " " + map[15] + " " +
            map[16] + " " + map[17] + " " + map[18] + " " + map[19] + " " + map[20] + " " + map[21] + " " + map[22] + " " + map[23] + " " +
            map[24] + " " + map[25]);

        //Send the Packet
        Send(map);
    }
}
