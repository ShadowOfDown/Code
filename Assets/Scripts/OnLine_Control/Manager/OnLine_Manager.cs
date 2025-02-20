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
    public static OnLine_Manager Instance {
        get {
            if (instance == null) {
                lock (locker) {
                    if (instance == null) {
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

    private OnLine_Manager() { }

    public event Action OnJoinedLobbyEvent;
    public event Action OnLeftLobbyEvent;
    public event Action OnConnectedServerEvent;
    public event Action OnDisconnectedServerEvent;
    public event Action<string> OnCreatedRoomEvent;
    public event Action OnCreatingRoomEvent;
    public event Action OnCreateRoomFailedEvent;
    public event Action<string> OnJoinedRoomEvent;
    public event Action<string> OnLeftRoomEvent;
    public event Action<List<RoomInfo>> OnRoomListUpdateEvent;
    public event Action<Player> OnPlayerEnteredRoomEvent;
    public event Action<Player> OnPlayerLeftRoomEvent;
    public event Action<Player, ExitGames.Client.Photon.Hashtable> OnPlayerPropertiesUpdateEvent;

    private Photon.Realtime.Room currentRoom;
    private Photon.Realtime.TypedLobby currentLobby;
    private static Photon.Realtime.TypedLobby lobby = new Photon.Realtime.TypedLobby("MainLobby", Photon.Realtime.LobbyType.SqlLobby);
    public List<RoomInfo> Rooms = new List<RoomInfo>();


    public bool isInMaster
    {
        get { return PhotonNetwork.IsConnectedAndReady; }
    }
    public bool IsConnected
    {
        get
        {
            return PhotonNetwork.IsConnected;
        }
    }
    /// <summary>
    /// 获取当前房间的管理者(不在房间内则返回null)
    /// </summary>
    public Player MasterClient
    {
        get
        {
            if (currentRoom == null) { return null; }
            return PhotonNetwork.MasterClient;
        }
    }
    /// <summary>
    /// 获得当前房间内自己的基础信息(Player类：包含userID（房间内玩家标识信息int），Name（玩家名），哈希表（改变时会使得OnPlayerPropertiesUpdate响应），)
    /// </summary>
    public Player LocalPlayer
    {
        get
        {
            if (currentRoom == null) { return null; }
            return PhotonNetwork.LocalPlayer;
        }
    }
    /// <summary>
    /// 获取房间内其他玩家的信息
    /// </summary>
    public Player[] PlayerListOthers
    {
        get
        {
            if (currentRoom == null) { return null; }
            return PhotonNetwork.PlayerListOthers;
        }
    }
    /// <summary>
    /// 获取房间内所有玩家的数量
    /// </summary>
    public int CountOfPlayers
    {
        get
        {
            if (currentRoom == null) { return 0; }
            return PhotonNetwork.CountOfPlayers;
        }
    }

    /// <summary>
    /// 获取玩家名字
    /// </summary>
    public string PlayerName
    {
        get; private set;
    }
    /// <summary>
    /// 获取当前所在房间
    /// </summary>
    public Photon.Realtime.Room CurrentRoom
    {
        get { return currentRoom; }
    }
    /// <summary>
    /// 获取当前所在大厅
    /// </summary>
    public Photon.Realtime.TypedLobby CurrentLobby
    {
        get { return currentLobby; }
    }
    /// <summary>
    /// 是否为当前房间的管理者
    /// </summary>
    public bool IsClient
    {
        get { return PhotonNetwork.IsMasterClient; }
    }
    /// <summary>
    /// 是否在大厅内
    /// </summary>
    public bool InLobby
    {
        get
        {
            return PhotonNetwork.InLobby;
        }
    }

    private void ReadPlayerName()
    {
        SetName(string.Empty);
    }

    /// <summary>
    /// 设置玩家名字
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        PlayerName = name;
        PhotonNetwork.NickName = name;
    }
    /// <summary>
    /// 刷新房间列表，使OnRoomListUpdate响应
    /// </summary>
    public void RefreshRoomList()
    {
        PhotonNetwork.GetCustomRoomList(lobby, "1");
    }
    /// <summary>
    /// 加入大厅(SQL类)
    /// </summary>
    /// <returns></returns>
    public bool EnterTypedLubby()
    {
        return PhotonNetwork.JoinLobby(lobby);
    }
    /// <summary>
    /// 离开大厅
    /// </summary>
    /// <returns></returns>
    public bool LeaveLubby()
    {
        return PhotonNetwork.LeaveLobby();
    }
    /// <summary>
    /// 链接服务器
    /// </summary>
    /// <returns></returns>
    public bool ConnectMaster()
    {
        return PhotonNetwork.ConnectUsingSettings();
    }
    /// <summary>
    /// 创建房间
    /// </summary>
    /// <param name="roomName">房间名称</param>
    /// <param name="roomOptions">哈希表，房间参数</param>
    /// <returns></returns>
    public bool CreateRoom(string roomName,Photon.Realtime.RoomOptions roomOptions)
    {
        OnCreatingRoomEvent?.Invoke();
        return PhotonNetwork.CreateRoom(roomName, roomOptions,lobby);
    }
    /// <summary>
    /// 离开当前房间
    /// </summary>
    /// <param name="roomName"></param>
    /// <returns></returns>
    public bool LeaveRoom(string roomName)
    {
        return PhotonNetwork.LeaveRoom(false);
    }
    /// <summary>
    /// 加入房间(通过房间名)
    /// </summary>
    /// <param name="roomName"></param>
    /// <returns></returns>
    public bool JoinRoom(string roomName) { 
        return PhotonNetwork.JoinRoom(roomName);
    }
    /// <summary>
    /// 当加入房间时响应
    /// </summary>
    public override void OnJoinedRoom()
    {
        if(PhotonNetwork.CurrentRoom  == null) { Debug.Log("Joined Room Failed"); return; }
        currentRoom = PhotonNetwork.CurrentRoom;
        Debug.Log("Joined Room : " + currentRoom.Name);
        OnJoinedRoomEvent?.Invoke(PhotonNetwork.CurrentRoom.Name);
    }
    /// <summary>
    /// 当离开房间时响应
    /// </summary>
    public override void OnLeftRoom()
    {
        if(currentRoom == null) { return; }
        Debug.Log("Left Room : " + currentRoom.Name);
        OnLeftRoomEvent?.Invoke(currentRoom.Name);
        currentRoom = null;
    }
    /// <summary>
    /// 当创建房间成功时响应
    /// </summary>
    public override void OnCreatedRoom()
    {
        if (currentRoom == null) { Debug.Log("Create Room : " + PhotonNetwork.CurrentRoom.Name); }
        OnCreatedRoomEvent?.Invoke(PhotonNetwork.CurrentRoom.Name);
    }

    /// <summary>
    /// 当链接上服务器时响应
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Maseter : " + PhotonNetwork.CloudRegion + "  "+ PhotonNetwork.ServerAddress);
        ReadPlayerName();
        OnConnectedServerEvent?.Invoke();
    }
    /// <summary>
    /// 当创建房间失败时响应
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create Room Failed! ErrorCode: "+returnCode+"  Error Message: "+message);
        OnCreateRoomFailedEvent?.Invoke();
    }
    /// <summary>
    /// 当加入大厅时响应
    /// </summary>
    public override void OnJoinedLobby()
    {
        if(PhotonNetwork.CurrentLobby == null) { Debug.Log("Joined Lobby Failed"); return;}
        Debug.Log("Joined Lobby : " + PhotonNetwork.CurrentLobby.Name);
        OnJoinedLobbyEvent?.Invoke();
    }
    /// <summary>
    /// 当离开大厅时响应
    /// </summary>
    public override void OnLeftLobby()
    {
        if(currentLobby == null) { return;}
        Debug.Log("Left Lobby : " + currentLobby.Name);
        currentLobby = null;
        OnLeftLobbyEvent?.Invoke();
    }

    /// <summary>
    /// 当有玩家加入时响应
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        OnPlayerEnteredRoomEvent?.Invoke(newPlayer);
    }
    /// <summary>
    /// 设置玩家属性
    /// </summary>
    /// <param name="table"></param>
    public void SetPlayerCustomProperties(ExitGames.Client.Photon.Hashtable table)
    {
        if(table == null) { Debug.Log("SetPlayerCustomProperties With Null"); return; }
        PhotonNetwork.SetPlayerCustomProperties(table);
    }
    /// <summary>
    /// 当有玩家离开时响应
    /// </summary>
    /// <param name="otherPlayer"></param>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        OnPlayerLeftRoomEvent?.Invoke(otherPlayer);
    }
    /// <summary>
    /// 当有玩家属性改变时响应
    /// </summary>
    /// <param name="targetPlayer"></param>
    /// <param name="changedProps"></param>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        OnPlayerPropertiesUpdateEvent?.Invoke(targetPlayer, changedProps);
    }
    /// <summary>
    /// 当使用FreshRoomList时响应
    /// </summary>
    /// <param name="rooms"></param>
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
        OnRoomListUpdateEvent?.Invoke(Rooms);
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
            RoomInfo temp = null;
            foreach (RoomInfo room in Rooms) {
                if (room.Name == roomName) {
                    temp = room;
                }
            }
            if (temp != null)
            {
                Rooms.Remove(temp);
            }
        }
    }
}
