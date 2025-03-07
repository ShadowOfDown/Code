//Author : _SourceCode
//CreateTime : 2025-02-17-21:42:39
//Version : 1.0
//UnityVersion : 2021.3.45f1c1

using ExitGames.Client.Photon.StructWrapping;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;



public class CreateRoomUI : UIObject
{
    const int RoomIDBit = 6;

    public InputField inputField;
    public RoomOptions roomOptions;
    public string roomID;
    public bool selfCreateRoomID = false;
    public override void OnLoad()
    {
        transform.Find("bg/title/closeBtn").GetComponent<Button>().onClick.AddListener(OnCloseBtnClicked);
        transform.Find("bg/okBtn").GetComponent<Button>().onClick.AddListener(OnCreateBtnClicked);
        transform.Find("bg/AdvancedSettings").GetComponent<Button>().onClick.AddListener(OnAdvancedSettingsButtonClicked);
        inputField = transform.Find("bg/InputField").GetComponent<InputField>();

        inputField.text = GameLoop.Instance.onlineManager.PlayerName + "的房间";

        roomOptions = CreateDefultOption();

        EventManager.AddListener<string>("OnJoinedRoomEvent", OnJoinedRoom);
        EventManager.AddListener<short,string>("OnCreateRoomFailedEvent", OnCreateRoomFailed);

    }
    public RoomOptions CreateDefultOption()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 8;
        roomOptions.PublishUserId = true;
        ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable();
        table.Add(OnlineManager.RoomNameSearchFilter, GameLoop.Instance.onlineManager.PlayerName + "的房间");
        table.Add(OnlineManager.RoomIDSearchFilter, "1");
        roomOptions.CustomRoomProperties = table;
        roomOptions.CustomRoomPropertiesForLobby = new string[] { OnlineManager.RoomIDSearchFilter,OnlineManager.RoomNameSearchFilter};
        return roomOptions;
    }
    private string CreateRoomID()
    {
        System.Random random = new System.Random();
        int id = random.Next((int)Mathf.Pow(10, RoomIDBit-1), (int)Mathf.Pow(10, RoomIDBit) - 1);
        return id.ToString();
    }

    public void OnAdvancedSettingsButtonClicked()
    {
        UI_Manager.Instance.ShowUI<CreateRoomAdvancedSettings>("CreateRoomAdvancedSettings").Init(this);
    }
    public void OnCloseBtnClicked()
    {
        UI_Manager.Instance.CloseUI(this.name);
    }
    public void OnCreateBtnClicked()
    {
        if (inputField.text.Length < 2) { return; }

        roomOptions.CustomRoomProperties[OnlineManager.RoomNameSearchFilter] = inputField.text;

        if (!selfCreateRoomID)
        {
            roomID = CreateRoomID();
        }
        roomOptions.CustomRoomProperties[OnlineManager.RoomIDSearchFilter] = roomID;
        GameLoop.Instance.onlineManager.CreateRoom(roomID, roomOptions);

        UI_Manager.Instance.ShowUI<MaskUI>("MaskUI").ShowMessage("正在创建房间...");
    }
    public void OnJoinedRoom(string name)
    {
        EventManager.RemoveListener<string>("OnJoinedRoomEvent", OnJoinedRoom);
        EventManager.RemoveListener<short,string>("OnCreateRoomFailedEvent", OnCreateRoomFailed);
        roomOptions.CustomRoomProperties.TryGetValue(OnlineManager.RoomNameSearchFilter, out object temp);
        UI_Manager.Instance.CloseAllUI();
        UI_Manager.Instance.ShowUI<RoomUI>("RoomUI").SetRoomNameAndID((string) temp,roomID);

    }
    public void OnCreateRoomFailed(short returnCode,string message)
    {
        if(returnCode == 32766)
        {
            if (selfCreateRoomID)
            {
                UI_Manager.Instance.LogWarnning("房间ID已存在");
                UI_Manager.Instance.CloseUI("MaskUI");
                UI_Manager.Instance.ShowUI<CreateRoomAdvancedSettings>("CreateRoomAdvancedSettings").Init(this);
                return;
            }
            roomID = CreateRoomID();
            roomOptions.CustomRoomProperties[OnlineManager.RoomIDSearchFilter] = roomID;
            GameLoop.Instance.onlineManager.CreateRoom(roomID, roomOptions);
            return;
        }
        UI_Manager.Instance.CloseUI("MaskUI");
        UI_Manager.Instance.LogWarnning("创建房间失败");
        UI_Manager.Instance.CloseUI(name);
    }
}
