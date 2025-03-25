// 输入密码的那个弹窗
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

class KeyInputInterface : IInterfaceBuilder
{
  public UnityAction OnButtonClick { get; set; }
  private ScreenMaskUniversalBuilder _screenMask;
  private TextBoxUniversalBuilder _boxBuilder;
  private InputBoxUniversalBuilder _keyInputBox;
  private ButtonUniversalbuilder _confirmButton;
  private ButtonUniversalbuilder _exitButtonBuilder;

  public InputBoxUniversalBuilder KeyInputBox { get { return _keyInputBox; } } 

  public KeyInputInterface(Transform parentTransform, UnityAction onKeyInputComfirmClick)
  {
    OnButtonClick = onKeyInputComfirmClick;
    BuildInterface(parentTransform);
    SetActive(false);
  }

  private void BuildInterface(Transform parentTransform)
  {
    // 遮盖屏幕
    _screenMask = new ScreenMaskUniversalBuilder(parentTransform);

    // 那个框
    float referenceBoxWidth = 1280f;
    bool boxIsDark = false;
    int boxType = 4;
    _boxBuilder = new TextBoxUniversalBuilder("InputBox", _screenMask.Transform, new Vector2(0.5f, 0.5f), boxType, 
      TextBoxUniversalBuilder.GetScaleRadio(boxType, new(referenceBoxWidth, -1), boxIsDark), boxIsDark);

    // 密钥输入框
    float referenceInputBoxWidth = 960f;
    int inputBoxType = 3;

    float referenceConfirmButtonHeight = referenceInputBoxWidth / InputBoxUniversalBuilder.GetReferenceAspectRadio(inputBoxType, boxIsDark);
    int comfirmbuttonType = 2;
    float referenceComfirmButtonWidth = referenceConfirmButtonHeight * ButtonUniversalbuilder.GetReferenceAspectRadio(comfirmbuttonType, false);
    float referenceGap = (referenceBoxWidth - referenceInputBoxWidth - referenceComfirmButtonWidth) / 3f;
    float inputBoxPosX = (referenceGap + referenceInputBoxWidth / 2f) / referenceBoxWidth;
    float comfirmBoxButtonPosX = (referenceGap * 2f + referenceInputBoxWidth + referenceComfirmButtonWidth / 2f) / referenceBoxWidth;
    
    _keyInputBox = new InputBoxUniversalBuilder("KeyInputBox", _boxBuilder.Transform, new Vector2(inputBoxPosX, 0.5f), inputBoxType, 
      InputBoxUniversalBuilder.GetScaleRadio(inputBoxType, new(referenceInputBoxWidth, -1), boxIsDark), boxIsDark);

    // 确认按钮
    _confirmButton = new ButtonUniversalbuilder(
      "ConfirmButton", new Vector2(comfirmBoxButtonPosX, 0.5f), _boxBuilder.Transform, new Dictionary<string, UnityAction>
      {
        {"onClickAction", OnButtonClick},
        {"OnHoverAction", null}
      }, comfirmbuttonType, ButtonUniversalbuilder.GetScaleRadio(comfirmbuttonType, new Vector2(referenceComfirmButtonWidth, -1), false), boxIsDark, false
    );

    // 退出按钮
    _exitButtonBuilder = new ButtonUniversalbuilder(
      "ExitButton", new Vector2(1f, 1f), _boxBuilder.Transform, new Dictionary<string, UnityAction>
      {
        {"onClickAction", OnExitBoxButtonClick},
        {"OnHoverAction", null}
      }, ButtonUniversalbuilder.returnButtonIndex, ButtonUniversalbuilder.GetScaleRadio(ButtonUniversalbuilder.returnButtonIndex, new(120, 120), true), !boxIsDark, true);
  }

  public void SetActive(bool state)
  {
    _screenMask.SetActive(state);
    _boxBuilder.SetActive(state);
    _keyInputBox.SetActive(state);
    _confirmButton.SetActive(state);
    _exitButtonBuilder.SetActive(state);
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
    return _keyInputBox.GetContent();
  }
}
