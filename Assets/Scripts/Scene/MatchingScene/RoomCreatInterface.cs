// 那个房间号的输入框
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

class RoomCreateInterface : IInterfaceBuilder
{

  #region Properities
  public ScreenMaskUniversalBuilder ScreenMask { get; }
  public TextBoxUniversalBuilder BoxBuilder { get; } 
  public ButtonUniversalbuilder ExitButton { get; }
  public CheckBoxUniversalBuilder CheckBox { get; }
  public InputBoxUniversalBuilder RoomIdInputBox { get; }
  public ButtonUniversalbuilder ComfirmButton { get; }
  public TextBoxGameObjectBuilder CheckBoxBackground { get; }
  public ButtonTextGameObjectBuilder KeyInputButton { get; }
  public KeyInputInterface KeyInputInterface{ get; }
  #endregion 

  #region Methods
  public RoomCreateInterface(Transform parentTransform)
  {
    // 全屏屏蔽
    ScreenMask = new ScreenMaskUniversalBuilder(parentTransform);

    // 背景框
    BoxBuilder = new TextBoxUniversalBuilder("InputBox", ScreenMask.Transform, new Vector2(0.5f, 0.5f), 3, 
      1280f / TextBoxUniversalBuilder.GetReferencePixel(3, 1, false).x, false);
  
    // 退出按钮
    ExitButton = new ButtonUniversalbuilder(
    "ExitButton", new Vector2(1.0f, 1.0f), BoxBuilder.Transform, new Dictionary<string, UnityAction>
      {
        {"onClickAction", OnExitButtonClick},
        {"OnHoverAction", null}
      }, -1, 80.0f / 100.0f
    );

    // roomId 输入框
    RoomIdInputBox = new InputBoxUniversalBuilder("RoomIdImputBox", BoxBuilder.Transform, new Vector2(0.39f, 0.7f), 4);

    // 确认按钮
    ComfirmButton = new ButtonUniversalbuilder(
    "ComfirmButtom", new Vector2(0.84375f, 0.7f), BoxBuilder.Transform, new Dictionary<string, UnityAction>
      {
        {"onClickAction", OnConfirmButtonClick},
        {"OnHoverAction", null}
      }, 3, 80.0f / ButtonUniversalbuilder.GetReferencePixel(3, 1).y
    );

    // 是否开启密钥的勾选框
    CheckBoxBackground = new TextBoxGameObjectBuilder(new Dictionary<string, string>
    {
      {"Image", "CheckBoxBackgroundImage"},
      {"Text", "Prompt"},
    }, BoxBuilder.Transform);
    CheckBoxBackground.Build(new SortedDictionary<string, Dictionary<string, IComponentBuilder>>
    {
      {"Image", new Dictionary<string, IComponentBuilder>
        {
          {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
            {
              {"referenceObjectPixels", new Vector2(400, 80)},
              {"archorMin", new Vector2(280f / 1280f, 0.4f)},
              {"archorMax", new Vector2(280f / 1280f, 0.4f)},
              {"pivotPos", new Vector2(0.5f, 0.5f)},
            })
          },
          {"Image", new ImageComponentBuilder(new Dictionary<string, string>
            {
              {"gameObjectName", "CheckBoxBackgroundImage"},
              {"resourcesFolderPath", MatchingScene.resourcesFolderPath},
            })
          }
        }
      },
      {"Text", new Dictionary<string, IComponentBuilder>
        {
          {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
            {
              {"referenceObjectPixels", new Vector2(320, 80)},
              {"archorMin", new Vector2(240f / 400f, 0.5f)},
              {"archorMax", new Vector2(240f / 400f, 0.5f)},
              {"pivotPos", new Vector2(0.5f, 0.5f)},
            }
          )},
          {"Text", new TextComponentBuilder(TextBoxUniversalBuilder.textArgu_01)}
        }
      }
    });
    CheckBoxBackground.ModifyContent("Inputthere");
    
    CheckBox = new CheckBoxUniversalBuilder("IsKeyNeed?", new Vector2(60f / 400f, 0.5f), CheckBoxBackground.Transform, 0.4f, OnCheckedButtonClick);

    // 密钥输入的按钮
    KeyInputButton = new ButtonTextGameObjectBuilder(new Dictionary<string, string>
    {
      {"Button", "InputButton"},
      {"Text", "Fuck"},
    }, BoxBuilder.Transform);
    KeyInputButton.Build(
      new SortedDictionary<string, Dictionary<string, IComponentBuilder>>
      {
        {"Button", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(
              new Dictionary<string, Vector2>
              {
                {"referenceObjectPixels", new Vector2(640, 60)},
                {"archorMin", new Vector2(0.65625f, 0.4f)},
                {"archorMax", new Vector2(0.65625f, 0.4f)},
                {"pivotPos", new Vector2(0.5f, 0.5f)},
              }
            )},
            {"Image" , new ImageComponentBuilder(
              new Dictionary<string, string>
              {
                {"gameObjectName", "NormalKeyInputBoxImage"},
                {"resourcesFolderPath", MatchingScene.resourcesFolderPath}
              }
            )},
            {"Button", new ButtonComponentBuilder(
              new Dictionary<string, string>
              {
                {"gameObjectName", "PressedKeyInputBoxImage"},
                {"resourcesFolderPath", MatchingScene.resourcesFolderPath}
              }
            )},
            {"EventTrigger", new EventTriggerComponentBuilder(new Dictionary<string, UnityAction>
              {
                {"onClickAction", OnKeyInputButtonClick},
                {"onHoverAction", null},
              }
            )}
          }
        },
        {"Text", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform" , new RectTransformComponentBuilder(
              new Dictionary<string, Vector2>
              {
                {"referenceObjectPixels", new Vector2(640, 60) * 0.9f},
                {"archorMin", new Vector2(0.5f, 0.5f)},
                {"archorMax", new Vector2(0.5f, 0.5f)},
                {"pivotPos", new Vector2(0.5f, 0.5f)},
              }
            )},
            {"Text", new TextComponentBuilder(TextBoxUniversalBuilder.textArgu_02)},
            {"ContentSizeFitter", new ContentSizeFitterComponentBuilder(
              new Dictionary<string, bool>
              {
                {"horizontalFit", false},
                {"verticalFit", false},
              }
            )},
          }
        }
      }
    );
  
    // 密钥输入的那个框
    KeyInputInterface = new KeyInputInterface(BoxBuilder.Transform, OnKeyInputComfirmClick);
    KeyInputInterface.SetActive(false);
    KeyInputButton.SetActive(false);
  }

  public void SetActive(bool state)
  {
    ScreenMask.SetActive(state);
    BoxBuilder.SetActive(state); 
    ExitButton.SetActive(state);
    CheckBox.SetActive(state);
    RoomIdInputBox.SetActive(state);
    ComfirmButton.SetActive(state);
    CheckBoxBackground.SetActive(state);
    KeyInputButton.SetActive(state & CheckBox.Checked);
  }

  // 勾选控制密钥输入是否出现
  public void OnCheckedButtonClick()
  {
    DebugInfo.Print("checked button clicked");
    KeyInputButton.SetActive(!CheckBox.Checked);  // 不知道为什么这个是反过来的
  }

  // 点击关闭按钮后退出弹窗
  public void OnExitButtonClick()
  {
    DebugInfo.Print("exit button clicked");
    SetActive(false);
  }

  // 启动密钥后开启密钥输入界面
  public void OnKeyInputButtonClick()
  {
    DebugInfo.Print("key input key clicked");
    KeyInputInterface.SetActive(true); 
  }

  // 点击确认后保存密钥
  public void OnConfirmButtonClick()
  {
    DebugInfo.Print("comfirm button clicked");
    // TODO: 输入房间号之后的逻辑
  }

  // 输入完成确认后创建房间
  public void OnKeyInputComfirmClick()
  {
    DebugInfo.Print("key input");
    // TODO: 输入密钥后设置房间密钥
    KeyInputInterface.SetActive(false);
  }
  #endregion
}