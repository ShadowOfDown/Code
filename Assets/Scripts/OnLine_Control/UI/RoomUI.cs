//Author : _SourceCode
//CreateTime : 2025-02-17-21:48:58
//Version : 1.0
//UnityVersion : 2021.3.45f1c1


using ExitGames.Client.Photon.StructWrapping;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomUI : UIObject
{
    private Text nameText;
    private Text IDText;
    private Dictionary<string, PlayerItem> Players = new Dictionary<string, PlayerItem>();
    private GameObject PlayerItem;
    private Transform ContentTrf;
    private Button StartGame;
    private List<string> PlayerIDs = new List<string>();
    public override void OnLoad()
    {
        nameText = transform.Find("bg/title/roomText").GetComponent<Text>();
        IDText = transform.Find("bg/title/IDText").GetComponent<Text>();
        transform.Find("bg/title/closeBtn").GetComponent<Button>().onClick.AddListener(OnCloseBtnClicked);
        PlayerItem = Resources.Load<GameObject>("LoginSystem/UI/Prefabs/PlayerItem");
        ContentTrf = transform.Find("bg/Content");
        StartGame = transform.Find("bg/startBtn").GetComponent<Button>();
        StartGame.onClick.AddListener(Game);
        InitPlayerList();

        EventManager.AddListener<Player,ExitGames.Client.Photon.Hashtable>("OnPlayerPropertiesUpdateEvent", OnPlayerPropertiesUpdate);
        EventManager.AddListener<Player>("OnPlayerEnteredRoomEvent",OnPlayerEnteredRoom);
        EventManager.AddListener<Player>("OnPlayerLeftRoomEvent", OnPlayerLeftRoom);
        EventManager.AddListener<string>("OnLeftRoomEvent", OnLeftRoom);
    }
    public void OnCloseBtnClicked()
    {
        GameLoop.Instance.onlineManager.LeaveRoom();
    }

    public void SetRoomNameAndID(string name,string ID)
    {
        nameText.text = name;
        IDText.text = "RoomID : "+ID;
    }

    public void InitPlayerList()
    {
        OnPlayerEnteredRoom(GameLoop.Instance.onlineManager.MasterClient);
        OnPlayerEnteredRoom(GameLoop.Instance.onlineManager .LocalPlayer);
        foreach (Player p in GameLoop.Instance.onlineManager.PlayerListOthers)
        {
            OnPlayerEnteredRoom(p);
            OnPlayerPropertiesUpdate(p, p.CustomProperties);
        }
    }
    public void OnLeftRoom(string s)
    {
        UI_Manager.Instance.ShowUI<LobbyUI>("LobbyUI");
        UI_Manager.Instance.CloseUI(name);
    }

    public override void OnClose()
    {
        base.OnClose();
    }
    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (newPlayer == null|| PlayerIDs.Contains(newPlayer.UserId)) { return; }
        PlayerItem t = Instantiate(PlayerItem, ContentTrf).AddComponent<PlayerItem>();
        t.Init(newPlayer);
        PlayerIDs.Add(newPlayer.UserId);
        Players.Add(newPlayer.UserId, t);
        UpdateUI();
    }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (otherPlayer == null || !Players.ContainsKey(otherPlayer.UserId)) { return; }
        PlayerItem t = Players[otherPlayer.UserId];
        Players.Remove(otherPlayer.UserId);
        PlayerIDs.Remove(otherPlayer.UserId);
        Destroy(t.gameObject);
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (AllReady() && GameLoop.Instance.onlineManager.IsClient) { StartGame.gameObject.SetActive(true); }
        else { StartGame.gameObject.SetActive(false); }
    }

    public bool AllReady()
    {
        foreach (string s in PlayerIDs)
        {
            if (Players[s] == null || !Players[s].isReady) { return false; }
        }
        return true;
    }

    public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("StartGame")) { 
            
        }
        if (!changedProps.TryGetValue<bool>("isReady", out bool t)) { return; }
        Players[targetPlayer.UserId].isReady = t;
        Players[targetPlayer.UserId].ChangeUI();
        UpdateUI();
    }
    public void Game()
    {
        //if (OnLine_Manager.Instance.CountOfPlayers <= 1)
        //{
        //    UI_Manager.Instance.LogWarnning("你怎么一个人啊，是没有人陪你玩吗？");
        //    return;
        //}
        ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable();
        table.Add("StartGame", true);
        GameLoop.Instance.onlineManager.SetPlayerCustomProperties(table);
        GameLoop.Instance.onlineManager.CurrentRoom.IsOpen = false;
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<Player, ExitGames.Client.Photon.Hashtable>("OnPlayerPropertiesUpdateEvent", OnPlayerPropertiesUpdate);
        EventManager.RemoveListener<Player>("OnPlayerEnteredRoomEvent", OnPlayerEnteredRoom);
        EventManager.RemoveListener<Player>("OnPlayerLeftRoomEvent", OnPlayerLeftRoom);
        EventManager.RemoveListener<string>("OnLeftRoomEvent", OnLeftRoom);
    }
}
