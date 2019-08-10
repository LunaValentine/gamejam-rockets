using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetMenu : Editor
{
    [MenuItem("Networking/AddNetBehavioursToDictionary")]
    public static void Whatever()
    {
        NetAggregator Agg = Object.FindObjectOfType<NetAggregator>();
        NetSync[] Nets = Object.FindObjectsOfType<NetSync>();

        Agg.NetObjs.Clear();
        for (int i = 0; i < Nets.Length; i++)
        {
            Agg.NetObjs.Add(Nets[i]);
        }

        //NetAggregator.NetObjs
    }
}
