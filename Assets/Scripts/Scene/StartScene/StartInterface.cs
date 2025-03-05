// 开始按钮的那个界面
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class StartInterface : IInterfaceBuilder
{
  private ButtonUniversalbuilder _settingButton = null;
  private ButtonUniversalbuilder _loginButton = null;
  private ButtonUniversalbuilder _exitButton = null;
  private ButtonUniversalbuilder _startButton = null; 

  public StartInterface(Transform parentTransform)
  {
    _settingButton = new ButtonUniversalbuilder("SettingButton", new Vector2(0.9f, 0.9f), parentTransform, new Dictionary<string, UnityAction>
    {
      {"onClickAction", OnSettingButtonClick},
    }, -1, 1.2f);

    _loginButton = new ButtonUniversalbuilder("LoginButton", new Vector2(0.9f, 0.75f), parentTransform, new Dictionary<string, UnityAction>
    {
      {"onClickAction", OnLoginButtonClick},
    }, -1, 1.2f);

    _exitButton = new ButtonUniversalbuilder("ExitButton", new Vector2(0.9f, 0.6f), parentTransform, new Dictionary<string, UnityAction>
    {
      {"onClickAction", OnExitButtonClick},
    }, -1, 1.2f);

    _startButton = new ButtonUniversalbuilder("StartButton", new Vector2(0.5f, 0.382f), parentTransform, new Dictionary<string, UnityAction>
    {
      {"onClickAction", OnStartButtonClick},
    }, 3, 2);
  }

  // 设置按钮
  private void OnSettingButtonClick()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log("setting button is clicked");
    }
    // TODO: 切换到设置场景
  }

  // 登录按钮: 
  private void OnLoginButtonClick()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log("login button is clicked");
    }
    // TODO: 进入登录逻辑
  }

  // 退出按钮
  private void OnExitButtonClick()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log("exit button is clicked");
    }
    SetActive(false);
  }

  // 开始游戏的那个按钮
  private void OnStartButtonClick()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log("start button is clicked");
    }
    // TODO: 对于登录的用户进入房间, 未登录的进入登录
    DebugInfo.Print("move to login interface");
    StartScene.interfaceIdx++;
  }

  public void SetActive(bool state)
  {
    _settingButton.SetActive(state);
    _loginButton.SetActive(state);
    _exitButton.SetActive(state);
    _startButton.SetActive(state); 
  }
}