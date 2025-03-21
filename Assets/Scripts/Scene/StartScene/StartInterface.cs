// 开始按钮的那个界面
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using Photon.Realtime;

public class StartInterface : IInterfaceBuilder
{
  private ButtonUniversalbuilder _settingButton;
  private ButtonUniversalbuilder _exitButton;
  private ButtonUniversalbuilder _startButton;

  public StartInterface(Transform parentTransform)
  {
    BuildSettingButton(parentTransform);
    BuildExitButton(parentTransform);
    BuildStartButton(parentTransform);
  }

  // 设置按钮
  private void OnSettingButtonClick()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log("setting button is clicked");
    }
    // TODO: 切换到设置场景
    UI_Manager.Instance.ShowUI<SettingScene>("SettingsUI");
    UI_Manager.Instance.CloseUI("LoginUI");
  }

  private void EnterLobbyUI()
  {
    UI_Manager.Instance.CloseUI("MaskUI");
    UI_Manager.Instance.ShowUI<MatchingScene>("MatchingUI");

    EventManager.RemoveListener("OnConnectedServerEvent", EnterLobbyUI);
    UI_Manager.Instance.CloseUI("LoginUI");
  }

  // 退出按钮
  private void OnExitButtonClick()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log("exit button is clicked");
    }
    // TODO: 直接退出游戏, 或者弹出一个选择框
    SetActive(false);
  }

  // 开始游戏的那个按钮
  private void OnStartButtonClick()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log("start button is clicked");
    }

    // TODO: 进入登录逻辑
    if (GameLoop.Instance.onlineManager.IsConnected || GameLoop.Instance.onlineManager.IsConnectedAndReady)
    {
      UI_Manager.Instance.ShowUI<MatchingScene>("MatchingUI");
      UI_Manager.Instance.CloseUI("LoginUI");
      return;
    }
    EventManager.AddListener("OnConnectedServerEvent", EnterLobbyUI);
    GameLoop.Instance.onlineManager.ConnectMaster();
    UI_Manager.Instance.ShowUI<MaskUI>("MaskUI").ShowMessage("正在连接服务器...");
  }

  public void SetActive(bool state)
  {
    _settingButton.SetActive(state);
    _exitButton.SetActive(state);
    _startButton.SetActive(state);
  }

  private void BuildSettingButton(Transform parentTransform)
  {
    _settingButton = new ButtonUniversalbuilder(
      "SettingButton",
      new Vector2(0.9f, 0.9f),
      parentTransform,
      new Dictionary<string, UnityAction>
      {
        {"onClickAction", OnSettingButtonClick},
      },
      ButtonUniversalbuilder.settingButtonIndex,
      1.2f,
      true,
      true);
  }

  private void BuildExitButton(Transform parentTransform)
  {
    _exitButton = new ButtonUniversalbuilder(
      "ExitButton",
      new Vector2(0.9f, 0.6f),
      parentTransform,
      new Dictionary<string, UnityAction>
      {
        {"onClickAction", OnExitButtonClick},
      },
      ButtonUniversalbuilder.exitButtonIndex,
      1.2f,
      true,
      true);
  }

  private void BuildStartButton(Transform parentTransform)
  {
    _startButton = new ButtonUniversalbuilder(
      "StartButton",
      new Vector2(0.5f, 0.382f),
      parentTransform,
      new Dictionary<string, UnityAction>
      {
        {"onClickAction", OnStartButtonClick},
      },
      3,
      2);
    _startButton.ModifyContent("start game");
  }
}
