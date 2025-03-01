//Author : _SourceCode
//CreateTime : 2025-02-17-19:39:49
//Version : 1.0
//UnityVersion : 2021.3.45f1c1


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : UIObject
{
    [SerializeField]
    private Button OnLine_StartButton;
    private Button QuitButton;
    private Button SettingsButton;

    public override void OnLoad()
    {
        OnLine_StartButton = transform.Find("OnLine_StartButton").GetComponent<Button>();
        QuitButton = transform.Find("QuitButton").GetComponent<Button>();
        SettingsButton = transform.Find("SettingsButton").GetComponent<Button>();

        OnLine_StartButton.onClick.AddListener(On_OnLine_StartButtonClicked);
        QuitButton.onClick.AddListener(On_QuitButtonClicked);
        SettingsButton.onClick.AddListener(On_SettingsButtonClicked);
    }

    public void On_OnLine_StartButtonClicked()
    {
        if (OnLine_Manager.Instance.IsConnected||OnLine_Manager.Instance.isInMaster) {
            UI_Manager.Instance.ShowUI<LobbyUI>("LobbyUI");
            UI_Manager.Instance.CloseUI("LoginUI");
            return;
        }
        OnLine_Manager.Instance.ConnectMaster();
        UI_Manager.Instance.ShowUI<MaskUI>("MaskUI").ShowMessage("正在连接服务器...");
        OnLine_Manager.Instance.OnConnectedServerEvent += EnterLobbyUI;
    }

    public void On_QuitButtonClicked()
    {
        Application.Quit();
    }

    public void On_SettingsButtonClicked()
    {

    }

    public void EnterLobbyUI()
    {
        UI_Manager.Instance.CloseUI("MaskUI");

        UI_Manager.Instance.ShowUI<LobbyUI>("LobbyUI");


        OnLine_Manager.Instance.OnConnectedServerEvent -= EnterLobbyUI;
        UI_Manager.Instance.CloseUI("LoginUI");
    }
}
