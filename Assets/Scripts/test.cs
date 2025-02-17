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
    [SerializeField]
    float timer = 0;
    int t = 0;
    IEnumerator ttt;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    private void Update()
    {
        timer += Time.deltaTime;
        while(timer < 2)
        {
            return;
        }
        timer = 0;
        IEnumerator tt = te();
        IEnumeratorSystem.Instance.startCoroutine(tt);
        t++;
        if(t == 3)
        {
            ttt = tt;
        }
        if(t == 5)
        {
            IEnumeratorSystem.Instance.stopCoroutine(ttt);
        }
    }

    IEnumerator te()
    {
        float t = Time.time;
        Debug.Log("Start"+"  "+ t);
        yield return new WaitForSeconds(20);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.Server);
        Debug.Log(PhotonNetwork.CloudRegion);
    }
}
