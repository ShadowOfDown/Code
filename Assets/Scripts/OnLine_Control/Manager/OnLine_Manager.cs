//Author : _SourceCode
//CreateTime : 2025-02-17-13:16:25
//Version : 1.0
//UnityVersion : 2021.3.45f1c1

using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class OnLine_Manager : MonoBehaviourPunCallbacks
{
    #region 单例
    private static OnLine_Manager instance;
    private static object locker = new object();
    public static OnLine_Manager Instance{
        get{
            if(instance == null){
                lock(locker){
                    if(instance == null){
                        Debug.Log("OnLine_Manager SingleTon Created !");
                        GameObject go = new GameObject("OnLine_Manager");
                        instance = go.AddComponent<OnLine_Manager>();
                        DontDestroyOnLoad(go);
                    }
                }
            }
            return instance;
        }
    }
    #endregion

    private OnLine_Manager(){}

    public event Action OnJoinedLobbyEvent;
    public event Action OnLeftLobbyEvent;
    public event Action OnConnectedServerEvent;
    public event Action OnDisconnectedServerEvent;
    public event Action<string> OnCreatedRoomEvent;
    public event Action OnCreatingRoomEvent;
    public event Action OnCreateRoomFailedEvent;
    public event Action<string> OnJoinedRoomEvent;
    public event Action <string> OnLeftRoomEvent;
    public event Action OnRoomListUpdateEvent;

    private Photon.Realtime.Room currentRoom;
    private Photon.Realtime.TypedLobby currentLobby;
    private Photon.Realtime.TypedLobby lobby = new Photon.Realtime.TypedLobby("lobby",Photon.Realtime.LobbyType.SqlLobby);
    public List<RoomInfo> Rooms= new List<RoomInfo>();
    public string PlayerName
    {
        get; private set;
    }
    public Photon.Realtime.Room CurrentRoom
    {
        get { return currentRoom; }
    }
    public Photon.Realtime.TypedLobby CurrentLobby
    {
        get { return currentLobby; }
    }
    public bool IsClient
    {
        get { return PhotonNetwork.IsMasterClient; }
    }
    public bool InLobby
    {
        get
        {
            return PhotonNetwork.InLobby;
        }
    }


    public void SetName(string name)
    {
        PlayerName = name;
    }

    public void RefreshRoomList()
    {
        PhotonNetwork.GetCustomRoomList(lobby, "1");
        
    }
    public bool EnterTypedLubby()
    {
        return PhotonNetwork.JoinLobby(lobby);
    }

    public bool LeaveLubby()
    {
        return PhotonNetwork.LeaveLobby();
    }

    public bool ConnectMaster()
    {
        return PhotonNetwork.ConnectUsingSettings();
    }

    public bool CreateRoom(string roomName,Photon.Realtime.RoomOptions roomOptions)
    {
        OnCreatingRoomEvent?.Invoke();
        return PhotonNetwork.CreateRoom(roomName, roomOptions,currentLobby);
    }

    public bool LeaveRoom(string roomName)
    {
        return PhotonNetwork.LeaveRoom();
    }

    public bool JoinRoom(string roomName) { 
        return PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        if(PhotonNetwork.CurrentRoom  == null) { Debug.Log("Joined Room Failed"); return; }
        Debug.Log("Joined Room : " + currentRoom.Name);
        currentRoom = PhotonNetwork.CurrentRoom;
        OnJoinedRoomEvent?.Invoke(PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnLeftRoom()
    {
        if(currentRoom == null) { return; }
        Debug.Log("Left Room : " + currentRoom.Name);
        OnLeftRoomEvent?.Invoke(currentRoom.Name);
        currentRoom = null;
    }

    public override void OnCreatedRoom()
    {
        if (currentRoom == null) { Debug.Log("Create Room Failed"); return; }
        currentRoom = PhotonNetwork.CurrentRoom;
        OnCreatedRoomEvent?.Invoke(currentRoom.Name);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Maseter : " + PhotonNetwork.CloudRegion + "  "+ PhotonNetwork.ServerAddress);
        OnConnectedServerEvent?.Invoke();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create Room Failed! ErrorCode: "+returnCode+"  Error Message: "+message);
        OnCreateRoomFailedEvent?.Invoke();
    }

    public override void OnJoinedLobby()
    {
        if(PhotonNetwork.CurrentLobby == null) { Debug.Log("Joined Lobby Failed"); return;}
        Debug.Log("Joined Lobby : " + PhotonNetwork.CurrentLobby.Name);
        OnJoinedLobbyEvent?.Invoke();
    }

    public override void OnLeftLobby()
    {
        if(currentLobby == null) { return;}
        Debug.Log("Left Lobby : " + currentLobby.Name);
        currentLobby = null;
        OnLeftLobbyEvent?.Invoke();
    }

    public override void OnConnected()
    {
        OnConnectedServerEvent?.Invoke();
    }

    public override void OnRoomListUpdate(List<RoomInfo> rooms)
    {
        UpdateRoomList(rooms);
    }

    private void UpdateRoomList(List<RoomInfo> rooms)
    {
        Dictionary<string, RoomInfo> d = new Dictionary<string, RoomInfo>();
        foreach (RoomInfo room in rooms)
        {
            d.Add(room.Name, room);
        }
        RemoveOutRooms(d);
        foreach (RoomInfo room in rooms)
        {
            if (!room.IsVisible || !room.IsOpen || room.RemovedFromList)
            {
                return;
            }
            else
            {
                Rooms.Add(room);
            }
        }
        OnRoomListUpdateEvent?.Invoke();
    }

    private void RemoveOutRooms(Dictionary<string, RoomInfo> d)
    {
        List<string> needtoremove = new List<string>();
        foreach (RoomInfo room in Rooms)
        {
            if (d.ContainsKey(room.Name))
            {
                needtoremove.Add(room.Name);
            }
        }
        foreach (string roomName in needtoremove)
        {
            foreach (RoomInfo room in Rooms) {
                if (room.Name == roomName) {
                    Rooms.Remove(room);
                }
            }
        }
    }
}
