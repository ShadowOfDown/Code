// 微信登录界面
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class WeiXinLoginInterface : IInterfaceBuilder
{
  private ImageGameObjectBuilder _buttonBoxImageBuilder;
  private readonly Dictionary<string, string> _buttonBoxImageArgu = new()
  {
    {"gameObjectName", "ButtonBoxImage"},
    {"resourcesFolderPath", StartScene.resourcesFolderPath}
  };
  private readonly Dictionary<string, Vector2> _buttonBoxRectTransformArgu = new()
  {
    {"referenceObjectPixels", new Vector2(1280, 320)},
    {"archorMin", new Vector2(0.5f, 0.382f)},
    {"archorMax", new Vector2(0.5f, 0.382f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };

  private readonly ButtonTextGameObjectBuilder _loginButtonBuilder;
  private readonly Dictionary<string, string> _loginButtonNameArgu = new()
  {
    {"Button", "LoginButton"},
    {"Text", "LoginButtonText"},
  };
  private readonly Dictionary<string, string> _loginButtonImageArgu = new()
  {
    {"gameObjectName", "NormalLoginButtonImage"},
    {"resourcesFolderPath", StartScene.resourcesFolderPath}
  };
  private readonly Dictionary<string, string> _loginButtonArgu = new()
  {
    {"gameObjectName", "PressedLoginButtonImage"},
    {"resourcesFolderPath", StartScene.resourcesFolderPath}
  };
  private readonly Dictionary<string, Vector2> _loginButtonRectTransformArgu = new()
  {
    {"referenceObjectPixels", new Vector2(640, 160)},
    {"archorMin", new Vector2(0.5f, 0.5f)},
    {"archorMax", new Vector2(0.5f, 0.5f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  private readonly Dictionary<string, Vector2> _loginButtonTextRectTransformArgu = new()
  {
    {"referenceObjectPixels", new Vector2(640 * 0.9f, 160 * 0.9f)},
    {"archorMin", new Vector2(0.5f, 0.5f)},
    {"archorMax", new Vector2(0.5f, 0.5f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  private readonly Dictionary<string, bool> _loginButtonContentSizeFitterArgu = new()
  {
    {"horizontalFit", false},
    {"verticalFit", false},
  };
  private readonly ButtonUniversalbuilder _closeButtonBoxBuilder = null;

  public WeiXinLoginInterface(Transform parentTransform)
  {
    _buttonBoxImageBuilder = new ImageGameObjectBuilder("ButtonBoxImage", parentTransform);
    _buttonBoxImageBuilder.Build(
      new Dictionary<string, IComponentBuilder>
      {
        {"RectTransform", new RectTransformComponentBuilder(_buttonBoxRectTransformArgu)},
        {"Image", new ImageComponentBuilder(_buttonBoxImageArgu)},
      }
    );

    _loginButtonBuilder = new ButtonTextGameObjectBuilder(_loginButtonNameArgu, _buttonBoxImageBuilder.Transform);
    _loginButtonBuilder.Build(
      new SortedDictionary<string, Dictionary<string, IComponentBuilder>>
      {
        {"Button", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(_loginButtonRectTransformArgu)},
            {"Image", new ImageComponentBuilder(_loginButtonImageArgu)},
            {"Button", new ButtonComponentBuilder(_loginButtonArgu)},
            {"EventTrigger", new EventTriggerComponentBuilder(new Dictionary<string, UnityAction>
              {
                {"onClickAction", OnLoginButtonClicked},
                {"onHoverAction", null},
              }
            )}
          }
        },
        {"Text", new Dictionary<string, IComponentBuilder>
          {
            {"Text", new TextComponentBuilder(TextBoxUniversalBuilder.textArgu_01)},
            {"RectTransform", new RectTransformComponentBuilder(_loginButtonTextRectTransformArgu)},
            {"ContentSizeFitter", new ContentSizeFitterComponentBuilder(_loginButtonContentSizeFitterArgu)}
          }
        },        
      }
    );

    _closeButtonBoxBuilder = new("CloseButton", Vector2.one, _buttonBoxImageBuilder.Transform, 
      new Dictionary<string, UnityAction>
      {
        {"onClickAction", OnCloseButtonClicked},
      }, -1, 0.8f);
  }

  // 登录
  public void OnLoginButtonClicked()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log("login button clicked");
    }
    // TODO: 进行微信登录
  }

  public void OnCloseButtonClicked()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log("close button clicked");
    }
    StartScene.interfaceIdx--;
  }

  public void SetActive(bool state)
  {
    _buttonBoxImageBuilder.SetActive(state);
    _loginButtonBuilder.SetActive(state);
    _closeButtonBoxBuilder.SetActive(state);
  }
}