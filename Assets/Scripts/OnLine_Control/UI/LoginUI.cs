//Author : _SourceCode
//CreateTime : 2025-02-17-19:39:49
//Version : 1.0
//UnityVersion : 2021.3.45f1c1


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    [SerializeField]
    private Button OnLine_StartButton;
    private Button QuitButton;
    private Button SettingsButton;

    private void Awake()
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
        OnLine_Manager.Instance.ConnectMaster();
        UI_Manager.Instance.ShowUI<MaskUI>("MaskUI").ShowMessage("�������ӷ�����...");
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

    private void OnDestroy()
    {
        
    }
}
