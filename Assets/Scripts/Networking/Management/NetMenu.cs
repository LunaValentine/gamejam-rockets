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
        NetBehaviour[] Nets = Object.FindObjectsOfType<NetBehaviour>();

        Agg.NetObjs.Clear();
        for (int i = 0; i < Nets.Length; i++)
        {
            Agg.NetObjs.Add(Nets[i]);
        }
    }
}
