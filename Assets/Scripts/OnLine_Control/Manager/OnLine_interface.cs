//Author : _SourceCode
//CreateTime : 2025-02-18-18:33:05
//Version : 1.0
//UnityVersion : 2021.3.45f1c1


using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface OnLine_interface
{

    public virtual void Init()
    {
        OnLine_Manager.Instance.OnJoinedLobbyEvent += OnJoinedLobby;
        OnLine_Manager.Instance.OnJoinedRoomEvent += OnJoinedRoom;
        OnLine_Manager.Instance.OnPlayerEnteredRoomEvent += OnPlayerEnteredRoom;
        OnLine_Manager.Instance.OnLeftLobbyEvent += OnLeftLobby;
        OnLine_Manager.Instance.OnLeftRoomEvent += OnLeftRoom;
        OnLine_Manager.Instance.OnPlayerPropertiesUpdateEvent += OnPlayerPropertiesUpdate;
        OnLine_Manager.Instance.OnPlayerLeftRoomEvent += OnPlayerLeftRoom;
        OnLine_Manager.Instance.OnConnectedServerEvent += OnConnected;
        OnLine_Manager.Instance.OnCreatedRoomEvent += OnCreatedRoom;
        OnLine_Manager.Instance.OnCreateRoomFailedEvent += OnCreateRoomFailed;
        OnLine_Manager.Instance.OnRoomListUpdateEvent += OnRoomListUpdate;
    }
    public virtual void OnJoinedRoom(string s)
    {
    }

    public virtual void OnLeftRoom(string s)
    {
    }

    public virtual void OnCreatedRoom(string s)
    {
    }

    public virtual void OnCreateRoomFailed()
    {
    }

    public virtual void OnJoinedLobby()
    {
    }

    public virtual void OnLeftLobby()
    {
    }

    public virtual void OnConnected()
    {

    }

    public virtual void OnPlayerEnteredRoom(Player newPlayer)
    {

    }

    public virtual void OnPlayerLeftRoom(Player otherPlayer)
    {

    }

    public virtual void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {

    }

    public virtual void OnRoomListUpdate(List<RoomInfo> rooms)
    {

    }
}
