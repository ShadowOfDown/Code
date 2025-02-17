//Author : _SourceCode
//CreateTime : 2025-02-17-21:48:58
//Version : 1.0
//UnityVersion : 2021.3.45f1c1


using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomUI : MonoBehaviour
{
    private Text text;
    private GameObject PlayerItem;
    private Transform ContentTrf;
    private Button StartGame;
    private List<string> PlayerIDs = new List<string>();
    public void Init(string name)
    {
        text = transform.Find("bg/title/Text").GetComponent<Text>();
        text.text = name;
        transform.Find("bg/title/closeBtn").GetComponent<Button>().onClick.AddListener(OnCloseBtnClicked);
        PlayerItem = Resources.Load<GameObject>("UI/PlayerItem");
        ContentTrf = transform.Find("bg/Content");
        StartGame = transform.Find("bg/startBtn").GetComponent<Button>();
    }
    public void OnCloseBtnClicked()
    {
        PhotonNetwork.LeaveRoom(false);
    }
}
