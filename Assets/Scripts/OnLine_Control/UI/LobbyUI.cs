//Author : _SourceCode
//CreateTime : 2025-02-17-20:21:36
//Version : 1.0
//UnityVersion : 2021.3.45f1c1


using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    private Transform ContentTrf;
    private GameObject RoomPrefab;
    private Dictionary<RoomInfo, RoomItemUI> roomList = new Dictionary<RoomInfo, RoomItemUI>();
    private bool isRefesh = false;
    private Coroutine autoFlash;
    private void Awake()
    {
        OnLine_Manager.Instance.EnterTypedLubby();
        OnLine_Manager.Instance.OnJoinedLobbyEvent += OnJoinedLobby;
        OnLine_Manager.Instance.OnLeftLobbyEvent += OnLeftLobby;
        transform.Find("content/title/closeBtn").GetComponent<Button>().onClick.AddListener(OnCloseButtonClicked);
        transform.Find("content/updateBtn").GetComponent<Button>().onClick.AddListener(OnUpdateButtonClicked);
        transform.Find("content/createBtn").GetComponent<Button>().onClick.AddListener(OnCreateButtonClicked);

        ContentTrf = transform.Find("content/ScrollView/Viewport/Content");

        transform.Find("NameButton").GetComponent<Button>().onClick.AddListener(OnNameButtonClicked);
        transform.Find("NameButton/Name").GetComponent<Text>().text = OnLine_Manager.Instance.PlayerName;
        RoomPrefab = Resources.Load<GameObject>("UI/RoomItem");
    }
    public void OnNameButtonClicked()
    {
        transform.Find("NameButton").GetComponent<Button>().enabled = false;
        UI_Manager.Instance.ShowUI<InputUserNameUI>("InputUserNameUI").OnNameChanged += ChangeName;
        transform.Find("NameButton").GetComponent<Button>().enabled = true;
    }
    private void ChangeName()
    {
        transform.Find("NameButton/Name").GetComponent<Text>().text = OnLine_Manager.Instance.PlayerName;
    }
    public void OnJoinedLobby()
    {
        OnUpdateButtonClicked();
        autoFlash = StartCoroutine(AutoFlashRoomList());
        OnLine_Manager.Instance.OnJoinedLobbyEvent -= OnJoinedLobby;
    }
    public void OnLeftLobby()
    {
        if (autoFlash != null)
        {
            StopCoroutine(autoFlash);
        }
        OnLine_Manager.Instance.OnLeftLobbyEvent -= OnLeftLobby;
    }
    public void OnCreateButtonClicked()
    {
        UI_Manager.Instance.ShowUI<CreateRoomUI>("CreateRoomUI");

    }
    public void OnUpdateButtonClicked()
    {
        UI_Manager.Instance.ShowUI<MaskUI>("MaskUI").ShowMessage("Ë¢ÐÂÖÐ...");
        isRefesh = true;
        OnLine_Manager.Instance.OnRoomListUpdateEvent += FreshRoomList;
        OnLine_Manager.Instance.RefreshRoomList();
    }

    private void FreshRoomList(List<RoomInfo > rooms)
    {
        UI_Manager.Instance.CloseUI("MaskUI");
        if (!OnLine_Manager.Instance.InLobby)
        {
            Debug.Log("Not In Lobby");
            OnLine_Manager.Instance.OnRoomListUpdateEvent -= FreshRoomList;
            return;
        }
        foreach (RoomItemUI roomItemUI in roomList.Values) {
            Destroy(roomItemUI.gameObject);
        }
        roomList.Clear();
        foreach (RoomInfo room in rooms) {
            RoomItemUI t = Instantiate(RoomPrefab, ContentTrf).AddComponent<RoomItemUI>();
            t.setRoomInfo(room);
            roomList.Add(room, t);
        }
        Debug.Log(roomList.Count);
        OnLine_Manager.Instance.OnRoomListUpdateEvent -= FreshRoomList;
    }


    public void OnCloseButtonClicked()
    {
        UI_Manager.Instance.ShowUI<LoginUI>("LoginUI");
        UI_Manager.Instance.CloseUI("LobbyUI");
    }
    IEnumerator AutoFlashRoomList()
    {
        while (true) {
            yield return new WaitForSeconds(3);
            if (OnLine_Manager.Instance.InLobby)
            {
                OnLine_Manager.Instance.RefreshRoomList();
            }
        }
    }
}
