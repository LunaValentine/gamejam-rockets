﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

class GnetServer : MonoBehaviour
{
    //0Protocol_Id, 4MessageNum, 8Acks
    int HeaderSize = 12;

    public Dictionary<IPEndPoint, uint> EndpointLastReceived = new Dictionary<IPEndPoint, uint>();

    static UdpClient ListenerClient;

    public Action<EndPoint> NewConnectionReceived;

    public int ListenPort = 26234;
    public uint PacketNumber = 0;

    private byte[] message = new byte[1000];
    private int messageIndex = 0;

    public GnetServer()
    {
        ListenerClient = new UdpClient(ListenPort);
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
                    if (NewConnectionReceived != null)
                        NewConnectionReceived(endpoint);
                }
                
                //If the message is more recent than the last we received then we should use it.... Duplicate last input from client
                if (SequenceMoreRecent(BitConverter.ToUInt32(message, 4), EndpointLastReceived[endpoint]))
                    Receive(endpoint, message);
            }
        }
    }

    public void AddToPacket(MyBitStream stream)
    {
        Buffer.BlockCopy(stream.GetUnderlyingArray(), 0, message, messageIndex, stream.BufferLength());
        messageIndex += stream.BufferLength();
    }

    public void StartPacket(MyBitStream stream)
    {
        byte[] message = new byte[stream.BufferLength() + HeaderSize];
        Buffer.BlockCopy(stream.GetUnderlyingArray(), 0, message, HeaderSize, stream.BufferLength());
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

    public void Close()
    {
        //Send TearDown to All EndPoints
        ListenerClient.Close();
        ListenerClient = null;
    }

    public void Receive(IPEndPoint endpoint, Byte[] message)
    {
        EndpointLastReceived[endpoint] = BitConverter.ToUInt32(message, 4);

        //TODO process the actual Input

        //Push to Associated InputHandler
    }

    bool SequenceMoreRecent(uint s1, uint s2)
    {
        return (s1 > s2) && (s1 - s2 <= uint.MaxValue / 2) || (s2 > s1) && (s2 - s1 > uint.MaxValue / 2);
    }
}