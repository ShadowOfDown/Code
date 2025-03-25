//Author : _SourceCode
//CreateTime : 2025-02-17-21:31:12
//Version : 1.0
//UnityVersion : 2021.3.45f1c1


using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomItemUI : MonoBehaviour
{
    private Text roomName;
    private Text roomCount;
    private RoomInfo roomInfo;
    private Button joinBtn;
    public void Init ()
    {
        roomName = transform.Find("roomName").GetComponent<Text>();
        roomCount = transform.Find("Count").GetComponent<Text>();
        joinBtn = transform.Find("joinBtn").GetComponent<Button>();
        joinBtn.onClick.AddListener(JoinRoom);
    }

    public void setRoomInfo(RoomInfo roomInfo)
    {
        if(!roomInfo.CustomProperties.TryGetValue(OnlineManager.RoomNameSearchFilter, out object temp)) { Debug.Log("roomInfo Do Not Contain name!"); } 
        roomName.text =(string)temp;
        roomCount.text = roomInfo.PlayerCount + " / " + roomInfo.MaxPlayers;
        this.roomInfo = roomInfo;
        if (roomInfo.PlayerCount >= roomInfo.MaxPlayers)
        {
            joinBtn.gameObject.SetActive(false);
        }
        else if (!joinBtn.gameObject.activeSelf)
        {
            joinBtn.gameObject.SetActive(true);
        }

    }

    public void JoinRoom()
    {
        if (roomInfo.PlayerCount < roomInfo.MaxPlayers)
        {
            if(GameLoop.Instance.onlineManager.JoinRoom(roomInfo.Name))
            {
                UI_Manager.Instance.ShowUI<MaskUI>("MaskUI").ShowMessage("正在加入房间...");
                EventManager.AddListener<string>("OnJoinedRoomEvent", OnJoinedRoom);
            }
        }
    }


    public void OnJoinedRoom(string name)
    {
        EventManager.RemoveListener<string>("OnJoinedRoomEvent", OnJoinedRoom);
        UI_Manager.Instance.ShowUI<RoomUI>("RoomUI").SetRoomNameAndID(roomName.text,roomInfo.Name);
        UI_Manager.Instance.CloseUI("LobbyUI");
        UI_Manager.Instance.CloseUI("MaskUI");
    }
}
