using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

class KeyInputInterface : IInterfaceBuilder
{
  public UnityAction OnButtonClick { get; set; }
  public ScreenMaskUniversalBuilder ScreenMask { get; private set; }
  public TextBoxUniversalBuilder BoxBuilder { get; private set; }
  public InputBoxUniversalBuilder KeyInputBox { get; }
  public ButtonUniversalbuilder ComfirmButton { get; }
  public ButtonUniversalbuilder ExitButtonBuilder { get; }

  public KeyInputInterface(Transform parentTransform, UnityAction OnKeyInputComfirmClick)
  {
    OnButtonClick = OnKeyInputComfirmClick;

    // 遮盖屏幕
    ScreenMask = new ScreenMaskUniversalBuilder(parentTransform);

    // 那个框
    BoxBuilder = new TextBoxUniversalBuilder("InputBox", ScreenMask.Transform, new Vector2(0.5f, 0.5f), 2, 
      320f / TextBoxUniversalBuilder.GetReferencePixel(2, 1, false).y, false);

    // 密钥输入框
    KeyInputBox = new InputBoxUniversalBuilder("KeyImputBox", BoxBuilder.Transform, new Vector2(0.39f, 0.7f), 4, 0.75f);

    // 确认按钮
    ComfirmButton = new ButtonUniversalbuilder(
    "ComfirmButtom", new Vector2(0.84375f, 0.7f), BoxBuilder.Transform, new Dictionary<string, UnityAction>
      {
        {"onClickAction", OnKeyInputComfirmClick},
        {"OnHoverAction", null}
      }, 3, 60.0f / ButtonUniversalbuilder.GetReferencePixel(3, 1).y
    );

    // 退出按钮
    ExitButtonBuilder = new ButtonUniversalbuilder(
    "ExitButtom", new Vector2(1f, 1f), BoxBuilder.Transform, new Dictionary<string, UnityAction>
      {
        {"onClickAction", OnButtonClick},
        {"OnHoverAction", null}
      }, -1, 60.0f / ButtonUniversalbuilder.GetReferencePixel(3, 1).y
    );

    // 隐藏
    SetActive(false);
  }

  public void SetActive(bool state)
  {
    ScreenMask.SetActive(state);
    BoxBuilder.SetActive(state);
    KeyInputBox.SetActive(state);
    ComfirmButton.SetActive(state);
    ExitButtonBuilder.SetActive(state);
  }

  // 退出
  public void OnExitBoxButtonClick()
  {
    DebugInfo.Print("exit button clicked!");
    SetActive(false);
  }

  // 获取文本输入框内容
  public string GetContent()
  {
    return KeyInputBox.GetContent();
  }
}