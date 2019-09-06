using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class GnetServer : MonoBehaviour
{
    public static bool Server = false;
    public Dictionary<IPEndPoint, uint> EndpointLastReceived = new Dictionary<IPEndPoint, uint>();
    public Dictionary<IPEndPoint, InputHandler> PlayerInput = new Dictionary<IPEndPoint, InputHandler>();

    public GameObject PlayerPrefab;

    private UdpClient ListenerClient;

    public int ListenPort = 26234;
    public uint PacketNumber = 0;

    private byte[] message = new byte[1000];
    private int messageIndex = 0;

    private static GnetServer _instance;
    public static GnetServer Instance
    {
        get
        {
            if (_instance == null)
                _instance = UnityEngine.Object.FindObjectOfType<GnetServer>();
            return _instance;
        }
    }

    void OnEnable()
    {
        ListenerClient = new UdpClient(ListenPort);

        //Without a NetAggregator we don't actually send anything back to the clients
        if (NetAggregator.Instance == null)
            gameObject.AddComponent<NetAggregator>();
        if (!NetAggregator.Instance.enabled)
            NetAggregator.Instance.enabled = true;
        Server = true;
    }

    void FixedUpdate()
    {
        ReceiveAll();
    }

    public void ReceiveAll()
    {
        while (ListenerClient.Available > 0)
        {
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
            Byte[] message = ListenerClient.Receive(ref endpoint);

            //This is one of ours
            if (BitConverter.ToUInt32(message, 0) == GnetBase.PROTOCOL_ID)
            {
                uint packetNumber = BitConverter.ToUInt32(message, 4);

                //Is this a new connection?
                if (!EndpointLastReceived.ContainsKey(endpoint))
                {
                    EndpointLastReceived.Add(endpoint, packetNumber - 1);
                    NewConnectionReceived(endpoint);
                }
                
                //If the message is more recent than the last we received then we should use it.... Duplicate last input from client
                if (SequenceMoreRecent(BitConverter.ToUInt32(message, 4), EndpointLastReceived[endpoint]))
                    Receive(endpoint, message);
            }
        }
    }

    public void AddToPacket(byte[] bytes)
    {
        Buffer.BlockCopy(bytes, 0, message, messageIndex, bytes.Length);
        messageIndex += bytes.Length;
    }

    public void StartPacket(MyBitStream stream)
    {
        byte[] message = new byte[stream.BufferLength() + GnetBase.HeaderSize];
        Buffer.BlockCopy(stream.GetUnderlyingArray(), 0, message, GnetBase.HeaderSize, stream.BufferLength());
        Buffer.BlockCopy(BitConverter.GetBytes(GnetBase.PROTOCOL_ID), 0, message, 0, 4);
        Buffer.BlockCopy(BitConverter.GetBytes(PacketNumber), 0, message, 4, 4);
    }

    public void SendPackets()
    {
        foreach (KeyValuePair<IPEndPoint, uint> KV in EndpointLastReceived)
        {
            //Add the last received to the packet before sending
            Buffer.BlockCopy(BitConverter.GetBytes(KV.Value), 0, message, 8, 4);

            ListenerClient.Send(message, messageIndex, KV.Key);
        }
        //Wipe the message buffer, start the index over, and increase the packetNumber
        message.Initialize();
        messageIndex = 0;
        PacketNumber++;
    }

    public void NewConnectionReceived(IPEndPoint endpoint)
    {
        GameObject playerobj = Instantiate(PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        InputHandler io = playerobj.GetComponent<InputHandler>();
        NetBehaviour playerNet = playerobj.GetComponent<NetBehaviour>();
        NetAggregator.Instance.PlayerObjs.Add(playerNet);
        PlayerInput.Add(endpoint, io);
    }


    public void Receive(IPEndPoint endpoint, Byte[] message)
    {
        EndpointLastReceived[endpoint] = BitConverter.ToUInt32(message, 4);

        //Last thing acked? = BitConverter.ToUInt32(message, 8);

        byte[] map = new byte[IoMap.Size];
        Buffer.BlockCopy(message, 12, map, 0, IoMap.Size);

        //For debugging player input
        /*Debug.Log(map[0] + " " + map[1] + " " + map[2] + " " + map[3] + " " + map[4] + " " + map[5] + " " + map[6] + " " + map[7] + " " +
            map[8] + " " + map[9] + " " + map[10] + " " + map[11] + " " + map[12] + " " + map[13] + " " + map[14] + " " + map[15] + " " +
            map[16] + " " + map[17] + " " + map[18] + " " + map[19] + " " + map[20] + " " + map[21] + " " + map[22] + " " + map[23] + " " +
            map[24] + " " + map[25]);
            */

        //Push to Associated InputHandler
        PlayerInput[endpoint].Push(new IoMap(map));
    }

    bool SequenceMoreRecent(uint s1, uint s2)
    {
        return (s1 > s2) && (s1 - s2 <= uint.MaxValue / 2) || (s2 > s1) && (s2 - s1 > uint.MaxValue / 2);
    }

    public void OnDisable()
    {
        //Probably should do some graceful disconnect... but not right now?
        //TODO

        //Send TearDown to All EndPoints
        ListenerClient.Close();
        ListenerClient = null;
    }
}
