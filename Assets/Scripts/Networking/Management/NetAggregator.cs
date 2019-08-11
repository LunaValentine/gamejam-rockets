using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetAggregator : MonoBehaviour
{
    //This is a constant (at least once a scene is started)
    public List<NetBehaviour> NetObjs = new List<NetBehaviour>();

    //This is because PlayerObjs will be modified regularly as players come and go...
    public List<NetBehaviour> PlayerObjs = new List<NetBehaviour>();
}
