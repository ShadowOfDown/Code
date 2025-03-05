//Author : _SourceCode
//CreateTime : 2025-02-17-13:16:25
//Version : 1.0
//UnityVersion : 2021.3.45f1c1

using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class OnlineManager : MonoBehaviourPunCallbacks
{
    public OnlineManager() { }

    #region paramaters
    private Photon.Realtime.Room currentRoom;
    private Photon.Realtime.TypedLobby currentLobby;
    private static Photon.Realtime.TypedLobby lobby = new Photon.Realtime.TypedLobby("MainLobby", Photon.Realtime.LobbyType.SqlLobby);
    public List<RoomInfo> Rooms = new List<RoomInfo>();

    #region connect value
    public bool IsConnectedAndReady
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
    /// 是否在大厅内
    /// </summary>
    public bool InLobby
    {
        get
        {
            return PhotonNetwork.InLobby;
        }
    }
    #endregion

    #region room value
    /// <summary>
    /// 获取当前所在房间
    /// </summary>
    public Photon.Realtime.Room CurrentRoom
    {
        get { return currentRoom; }
    }
    /// <summary>
    /// 是否为当前房间的管理者
    /// </summary>
    public bool IsClient
    {
        get { return PhotonNetwork.IsMasterClient; }
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


    #endregion
    /// <summary>
    /// 获取玩家名字
    /// </summary>
    public string PlayerName
    {
        get;
        private set;
    }

    /// <summary>
    /// 获取当前所在大厅
    /// </summary>
    public Photon.Realtime.TypedLobby CurrentLobby
    {
        get { return currentLobby; }
    }


    #endregion

    #region Name Methods
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

    #endregion.

    #region getRoomList Methods
    /// <summary>
    /// 刷新房间列表，使OnRoomListUpdate响应
    /// </summary>
    public void RefreshRoomList(string SearchKey)
    {
        if(SearchKey == null||SearchKey.Length == 0) { SearchKey = "1"; }
        PhotonNetwork.GetCustomRoomList(lobby, SearchKey);
    }
    public void RefreshRoomList()
    {
        PhotonNetwork.GetCustomRoomList(lobby, "1");
    }
    public void RefreshRoomList(TypedLobby typedLobby,string SearchKey)
    {
        if (SearchKey == null || SearchKey.Length == 0) { SearchKey = "1"; }
        PhotonNetwork.GetCustomRoomList(typedLobby, SearchKey);
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
        EventManager.BroadCast<List<RoomInfo>>("OnRoomListUpdateEvent",rooms);
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
            foreach (RoomInfo room in Rooms)
            {
                if (room.Name == roomName)
                {
                    temp = room;
                }
            }
            if (temp != null)
            {
                Rooms.Remove(temp);
            }
        }
    }
    #endregion

    #region Connect Methods

    /// <summary>
    /// 链接服务器
    /// </summary>
    /// <returns></returns>
    public bool ConnectMaster()
    {
        return PhotonNetwork.ConnectUsingSettings();
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
    /// 创建房间
    /// </summary>
    /// <param name="roomName">房间名称</param>
    /// <param name="roomOptions">哈希表，房间参数</param>
    /// <returns></returns>
    public bool CreateRoom(string roomID,Photon.Realtime.RoomOptions roomOptions)
    {
        bool res = false;
        if (PhotonNetwork.IsConnectedAndReady)
        {
            res = PhotonNetwork.CreateRoom(roomID, roomOptions, lobby);
            //EventManager.BroadCast<string>("OnCreatingRoomEvent",roomID);
        }
        return res;
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
    #endregion

    #region OnlineCallbacks
    #region selfConnect Callbacks
    /// <summary>
    /// 当链接上服务器时响应
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Maseter : " + PhotonNetwork.CloudRegion + "  " + PhotonNetwork.ServerAddress);
        ReadPlayerName();
        EventManager.BroadCast("OnConnectedServerEvent");
    }
    /// <summary>
    /// 当加入大厅时响应
    /// </summary>
    public override void OnJoinedLobby()
    {
        if (PhotonNetwork.CurrentLobby == null) { Debug.Log("Joined Lobby Failed"); return; }
        Debug.Log("Joined Lobby : " + PhotonNetwork.CurrentLobby.Name);
        EventManager.BroadCast("OnJoinedLobbyEvent");
    }
    /// <summary>
    /// 当离开大厅时响应
    /// </summary>
    public override void OnLeftLobby()
    {
        if (currentLobby == null) { return; }
        Debug.Log("Left Lobby : " + currentLobby.Name);
        currentLobby = null;
        EventManager.BroadCast("OnLeftLobbyEvent");
    }
    ///// <summary>
    ///// 当创建房间成功时响应
    ///// </summary>

    //public override void OnCreatedRoom()
    //{
    //    if (currentRoom == null) { Debug.Log("Create Room : " + PhotonNetwork.CurrentRoom.Name); }
    //    EventManager.BroadCast<string>("OnCreatedRoomEvent",PhotonNetwork.CurrentRoom.Name);
    //}
    /// <summary>
    /// 当创建房间失败时响应
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create Room Failed! ErrorCode: " + returnCode + "  Error Message: " + message);
        EventManager.BroadCast("OnCreateRoomFailedEvent",returnCode,message);
    }
    /// <summary>
    /// 当加入房间时响应
    /// </summary>
    public override void OnJoinedRoom()
    {
        if(PhotonNetwork.CurrentRoom  == null) { Debug.Log("Joined Room Failed"); return; }
        currentRoom = PhotonNetwork.CurrentRoom;
        Debug.Log("Joined Room : " + currentRoom.Name);
        EventManager.BroadCast<string>("OnJoinedRoomEvent", PhotonNetwork.CurrentRoom.Name);
    }
    /// <summary>
    /// 当离开房间时响应
    /// </summary>
    public override void OnLeftRoom()
    {
        if(currentRoom == null) { return; }
        Debug.Log("Left Room : " + currentRoom.Name);
        EventManager.BroadCast<string>("OnLeftRoomEvent", currentRoom.Name);
        currentRoom = null;
    }

    #endregion
    #region multiply Players Callbacks
    /// <summary>
    /// 当有玩家离开时响应
    /// </summary>
    /// <param name="otherPlayer"></param>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        EventManager.BroadCast<Player>("OnPlayerLeftRoomEvent", otherPlayer);
    }
    /// <summary>
    /// 当有玩家属性改变时响应
    /// </summary>
    /// <param name="targetPlayer"></param>
    /// <param name="changedProps"></param>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        EventManager.BroadCast<Player,ExitGames.Client.Photon.Hashtable>("OnPlayerPropertiesUpdateEvent", targetPlayer, changedProps);
    }
    /// <summary>
    /// 当使用FreshRoomList时响应
    /// </summary>
    /// <param name="rooms"></param>
    /// 
    public override void OnRoomListUpdate(List<RoomInfo> rooms)
    {
        UpdateRoomList(rooms);
    }

    /// <summary>
    /// 当有玩家加入时响应
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
       EventManager.BroadCast<Player>("OnPlayerEnteredRoomEvent", newPlayer);
    }
    #endregion
    #endregion


    /// <summary>
    /// 设置玩家属性
    /// </summary>
    /// <param name="table"></param>
    public void SetPlayerCustomProperties(ExitGames.Client.Photon.Hashtable table)
    {
        if (table == null) { Debug.Log("SetPlayerCustomProperties With Null"); return; }
        PhotonNetwork.SetPlayerCustomProperties(table);
    }



}
