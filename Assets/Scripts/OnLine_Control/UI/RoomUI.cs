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

public class RoomUI : MonoBehaviour
{
    private Text text;
    private Dictionary<string, PlayerItem> Players = new Dictionary<string, PlayerItem>();
    private GameObject PlayerItem;
    private Transform ContentTrf;
    private Button StartGame;
    private List<string> PlayerIDs = new List<string>();
    public void Awake()
    {
        text = transform.Find("bg/title/Text").GetComponent<Text>();
        text.text = OnLine_Manager.Instance.CurrentRoom.Name;
        transform.Find("bg/title/closeBtn").GetComponent<Button>().onClick.AddListener(OnCloseBtnClicked);
        PlayerItem = Resources.Load<GameObject>("UI/PlayerItem");
        ContentTrf = transform.Find("bg/Content");
        StartGame = transform.Find("bg/startBtn").GetComponent<Button>();
        StartGame.onClick.AddListener(Game);

        OnLine_Manager.Instance.OnPlayerPropertiesUpdateEvent += OnPlayerPropertiesUpdate;
        OnLine_Manager.Instance.OnPlayerEnteredRoomEvent += OnPlayerEnteredRoom;
        OnLine_Manager.Instance.OnPlayerLeftRoomEvent += OnPlayerLeftRoom;
        OnLine_Manager.Instance.OnLeftRoomEvent += OnLeftRoom;

        InitPlayerList();
    }
    public void OnCloseBtnClicked()
    {
        OnLine_Manager.Instance.LeaveRoom(OnLine_Manager.Instance.CurrentRoom.Name);
    }

    public void InitPlayerList()
    {
        OnPlayerEnteredRoom(OnLine_Manager.Instance.MasterClient);
        OnPlayerEnteredRoom(OnLine_Manager.Instance.LocalPlayer);
        foreach (Player p in OnLine_Manager.Instance.PlayerListOthers)
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
    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PlayerIDs.Contains(newPlayer.UserId)) { return; }
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
        if (AllReady() && OnLine_Manager.Instance.IsClient) { StartGame.gameObject.SetActive(true); }
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
        if (!OnLine_Manager.Instance.IsClient && targetPlayer.IsMasterClient && changedProps.ContainsKey("StartGame")) { 
            
        }
        if (!changedProps.TryGetValue<bool>("isReady", out bool t)) { return; }
        Players[targetPlayer.UserId].isReady = t;
        Players[targetPlayer.UserId].ChangeUI();
        UpdateUI();
    }
    public void Game()
    {
        if (OnLine_Manager.Instance.CountOfPlayers <= 1)
        {
            UI_Manager.Instance.ShowUI<MaskUI>("MaskUI").ShowMessage("你怎么一个人啊，是没有人陪你玩吗？");
            return;
        }
        ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable();
        table.Add("StartGame", true);
        OnLine_Manager.Instance.SetPlayerCustomProperties(table);
        OnLine_Manager.Instance.CurrentRoom.IsOpen = false;
    }
}
