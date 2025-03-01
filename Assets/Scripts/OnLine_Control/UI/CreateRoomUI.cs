//Author : _SourceCode
//CreateTime : 2025-02-17-21:42:39
//Version : 1.0
//UnityVersion : 2021.3.45f1c1


using Photon.Pun.Demo.PunBasics;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomUI : UIObject
{
    public InputField inputField;
    public override void OnLoad()
    {
        transform.Find("bg/title/closeBtn").GetComponent<Button>().onClick.AddListener(OnCloseBtnClicked);
        transform.Find("bg/okBtn").GetComponent<Button>().onClick.AddListener(OnCreateBtnClicked);
        inputField = transform.Find("bg/InputField").GetComponent<InputField>();
        inputField.text = OnLine_Manager.Instance.PlayerName + "的房间";
        OnLine_Manager.Instance.OnJoinedRoomEvent += OnJoinedRoom;
        OnLine_Manager.Instance.OnCreateRoomFailedEvent += OnCreateRoomFailed;

    }

    public void OnCloseBtnClicked()
    {
        UI_Manager.Instance.CloseUI(this.name);
    }
    public void OnCreateBtnClicked()
    {
        if (inputField.text.Length < 2) { return; }
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 5;
        roomOptions.PublishUserId = true;


        OnLine_Manager.Instance.CreateRoom(inputField.text, roomOptions);
        UI_Manager.Instance.ShowUI<MaskUI>("MaskUI").ShowMessage("正在创建房间...");
    }
    public void OnJoinedRoom(string name)
    {
        OnLine_Manager.Instance.OnCreatedRoomEvent -= OnJoinedRoom;
        OnLine_Manager.Instance.OnCreateRoomFailedEvent -= OnCreateRoomFailed;
        UI_Manager.Instance.ShowUI<RoomUI>("RoomUI");
        UI_Manager.Instance.CloseUI("LobbyUI");
        UI_Manager.Instance.CloseUI("MaskUI");
        UI_Manager.Instance.CloseUI(this.name);
    }
    public void OnCreateRoomFailed()
    {
        UI_Manager.Instance.LogWarnning("创建房间失败");
        UI_Manager.Instance.CloseUI(name);
    }
}
