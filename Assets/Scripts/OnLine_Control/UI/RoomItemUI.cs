//Author : _SourceCode
//CreateTime : 2025-02-17-21:31:12
//Version : 1.0
//UnityVersion : 2021.3.45f1c1


using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItemUI : MonoBehaviour
{
    private Text roomName;
    private Text roomCount;
    private RoomInfo roomInfo;
    private Button joinBtn;
    private void Awake()
    {
        roomName = transform.Find("roomName").GetComponent<Text>();
        roomCount = transform.Find("Count").GetComponent<Text>();
        joinBtn = transform.Find("joinBtn").GetComponent<Button>();
        joinBtn.onClick.AddListener(JoinRoom);
        OnLine_Manager.Instance.OnJoinedRoomEvent += OnJoinedRoom;
    }

    public void setRoomInfo(RoomInfo roomInfo)
    {
        roomName.text = roomInfo.Name;
        roomCount.text = roomInfo.PlayerCount + " / " + roomInfo.MaxPlayers;
        this.roomInfo = roomInfo;
        if (roomInfo.PlayerCount >= roomInfo.MaxPlayers)
        {
            joinBtn.gameObject.SetActive(false);
        }
        else if (!joinBtn.gameObject.activeSelf)
        {
            joinBtn.gameObject.SetActive(true);
        }

    }

    public void JoinRoom()
    {
        if (roomInfo.PlayerCount < roomInfo.MaxPlayers)
        {
            if(OnLine_Manager.Instance.JoinRoom(roomInfo.Name))
                UI_Manager.Instance.ShowUI<MaskUI>("MaskUI").ShowMessage("正在加入房间...");
        }
    }


    public void OnJoinedRoom(string name)
    {
        OnLine_Manager.Instance.OnJoinedRoomEvent -= OnJoinedRoom;
        UI_Manager.Instance.ShowUI<RoomUI>("RoomUI").Init(name);
        UI_Manager.Instance.CloseUI("LobbyUI");
        UI_Manager.Instance.CloseUI("MaskUI");
    }
}
