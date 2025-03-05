//Author : _SourceCode
//CreateTime : 2025-02-17-20:21:36
//Version : 1.0
//UnityVersion : 2021.3.45f1c1


using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : UIObject
{
    private Transform ContentTrf;
    private GameObject RoomPrefab;
    private Dictionary<RoomInfo, RoomItemUI> roomList = new Dictionary<RoomInfo, RoomItemUI>();
    [SerializeField]
    private IEnumerator autoFlash;
    private bool MaskOn = false;
    private string SearchKey = "µÄ";
    public override void OnLoad()
    {
        if(GameLoop.Instance.onlineManager.IsConnectedAndReady)
        {
            GameLoop.Instance.onlineManager.EnterTypedLubby();
            EventManager.AddListener("OnJoinedLobbyEvent", OnJoinedLobby);
        }
        else
        {
            if (!GameLoop.Instance.onlineManager.IsConnected) { GameLoop.Instance.onlineManager.ConnectMaster(); }
            EventManager.AddListener("OnConnectedServerEvent", EnterTypedLobbyLater);
        }
        transform.Find("content/title/closeBtn").GetComponent<Button>().onClick.AddListener(OnCloseButtonClicked);
        transform.Find("content/updateBtn").GetComponent<Button>().onClick.AddListener(OnUpdateButtonClicked);
        transform.Find("content/createBtn").GetComponent<Button>().onClick.AddListener(OnCreateButtonClicked);

        ContentTrf = transform.Find("content/ScrollView/Viewport/Content");

        transform.Find("NameButton").GetComponent<Button>().onClick.AddListener(OnNameButtonClicked);
        transform.Find("NameButton/Name").GetComponent<Text>().text = GameLoop.Instance.onlineManager.PlayerName;
        RoomPrefab = Resources.Load<GameObject>("LoginSystem/UI/Prefabs/RoomItem");

        EventManager.AddListener<List<RoomInfo>>("OnRoomListUpdateEvent", FreshRoomList);
    }
    private void EnterTypedLobbyLater()
    {
        EventManager.RemoveListener("OnConnectedServerEvent", EnterTypedLobbyLater);
        GameLoop.Instance.onlineManager.EnterTypedLubby();
        EventManager.AddListener("OnJoinedLobbyEvent", OnJoinedLobby);
    }
    public void OnNameButtonClicked()
    {
        transform.Find("NameButton").GetComponent<Button>().enabled = false;
        UI_Manager.Instance.ShowUI<InputUserNameUI>("InputUserNameUI").OnNameChanged += ChangeName;
        MaskOn = true;
        transform.Find("NameButton").GetComponent<Button>().enabled = true;
    }
    private void ChangeName()
    {
        transform.Find("NameButton/Name").GetComponent<Text>().text = GameLoop.Instance.onlineManager.PlayerName;
    }
    public void OnJoinedLobby()
    {
        OnUpdateButtonClicked();
        autoFlash = AutoFlashRoomList();
        IEnumeratorSystem.Instance.startCoroutine(autoFlash);
    }
    public void OnCreateButtonClicked()
    {
        if (!GameLoop.Instance.onlineManager.InLobby || GameLoop.Instance.onlineManager.CurrentRoom!=null) { return; }
        UI_Manager.Instance.ShowUI<CreateRoomUI>("CreateRoomUI");
    }
    public void OnUpdateButtonClicked()
    {
        if (!GameLoop.Instance.onlineManager.InLobby) { return; }
        UI_Manager.Instance.ShowUI<MaskUI>("MaskUI").ShowMessage("Ë¢ÐÂÖÐ...");
        MaskOn = true;
        GameLoop.Instance.onlineManager.RefreshRoomList(SearchKey);
    }

    private void FreshRoomList(List<RoomInfo > rooms)
    {
        if (MaskOn)
        {
            UI_Manager.Instance.CloseUI("MaskUI");
            MaskOn = false;
        }
        if (!GameLoop.Instance.onlineManager.InLobby)
        {
            Debug.Log("Not In Lobby");
            return;
        }
        foreach (RoomItemUI roomItemUI in roomList.Values) {
            Destroy(roomItemUI.gameObject);
            Debug.Log("Destory "+ roomItemUI.gameObject.name);
        }
        roomList.Clear();
        foreach (RoomInfo room in rooms) {
            RoomItemUI t = Instantiate(RoomPrefab, ContentTrf).AddComponent<RoomItemUI>();
            t.Init();
            t.setRoomInfo(room);
            roomList.Add(room, t);
        }
    }


    public void OnCloseButtonClicked()
    {
        CloseAutoFlash();
        UI_Manager.Instance.ShowUI<LoginUI>("LoginUI");
        UI_Manager.Instance.CloseUI("LobbyUI");
    }

    public void CloseAutoFlash()
    {
        if (autoFlash != null)
        {
            IEnumeratorSystem.Instance.stopCoroutine(autoFlash);
        }
    }
    IEnumerator AutoFlashRoomList()
    {
        while (true) {
            yield return new WaitForSeconds(3);
            if (GameLoop.Instance.onlineManager.InLobby)
            {
                GameLoop.Instance.onlineManager.RefreshRoomList(SearchKey);
            }
        }
    }

    public override void OnClose()
    {
        base.OnClose();
        if (autoFlash != null && IEnumeratorSystem.isLoaded) {
            IEnumeratorSystem.Instance.stopCoroutine(autoFlash);
        }
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener("OnJoinedLobbyEvent", OnJoinedLobby);
    }
}
