//Author : _SourceCode
//CreateTime : 2025-02-15-01:00:25
//Version : 1.0
//UnityVersion : 2021.3.45f1c1

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class test : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        GameLoop.Instance.gameObject.SetActive(true);
    }
}
