using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetAggregator : MonoBehaviour
{
    private static NetAggregator _instance;
    public static NetAggregator Instance
    {
        get
        {
            if (_instance == null)
                _instance = UnityEngine.Object.FindObjectOfType<NetAggregator>();
            return _instance;
        }
    }

    //This is a constant (at least once a scene is started)
    public List<NetBehaviour> NetObjs = new List<NetBehaviour>();

    //This is because PlayerObjs will be modified regularly as players come and go...
    public List<NetBehaviour> PlayerObjs = new List<NetBehaviour>();

    //I should be last, trigger the server to send.
    private void FixedUpdate()
    {
        if(GnetServer.Server)
        {
            PackAll();
        }
    }

    private void PackAll()
    {
        byte[] data = new byte[((NetObjs.Count + PlayerObjs.Count) * NetBehaviour.PackedSize) + 1];
        int index = 0;
        //I guess I aggregate everything?
        //Since this never changes in size, I don't have to add indexes...
        for (int i = 0; i < NetObjs.Count; i++)
        {
            byte[] objBytes = NetObjs[i].Pack();
            Buffer.BlockCopy(objBytes, 0, data, index, NetBehaviour.PackedSize);
            index += NetBehaviour.PackedSize;
        }

        //Pack the number of players
        if (PlayerObjs.Count > byte.MaxValue)
            throw new Exception("Wow you managed to load in too many players");
        data[index] = (byte)PlayerObjs.Count;
        index++;

        for (int i = 0; i < PlayerObjs.Count; i++)
        {
            byte[] objBytes = PlayerObjs[i].Pack();
            Buffer.BlockCopy(objBytes, 0, data, index, NetBehaviour.PackedSize);
        }


        //Finally Trigger the Send Packets
        GnetServer.Instance.AddToPacket(data);
        GnetServer.Instance.SendPackets();
    }

    public void UnPackAll(MyBitStream stream)
    {
        //I guess I aggregate everything?
        //Since this never changes in size, I don't have to add indexes...
        for (int i = 0; i < NetObjs.Count; i++)
        {
            NetObjs[i].FreshPush(stream);
        }

        //UnPack the number of players
        //TODO fix number of player discrepancy
        if (PlayerObjs.Count != (int)stream.ReadByte())
            throw new Exception("Wow you managed to have the wrong number of players...");

        for (int i = 0; i < PlayerObjs.Count; i++)
        {
            PlayerObjs[i].FreshPush(stream);
        }
    }
}
