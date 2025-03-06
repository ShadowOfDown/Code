// 提示框, 会屏蔽掉所有的操作

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public class PromptBoxUniversalBuilder : IUniversalGameObjectBuilder
{
  #region Properties
  public string Type { get; } = "LoadingSlider";
  public string GameObjectName { get { return TextBoxBuilder.GameObjectName; } }
  public Transform Transform { get { return TextBoxBuilder.Transform; } }
  public RectTransform RectTransform { get { return TextBoxBuilder.RectTransform; } }
  public ScreenMaskUniversalBuilder ScreenMask { get; }
  public TextBoxUniversalBuilder TextBoxBuilder { get; }
  public ButtonUniversalbuilder PromptButtonBuilder { get; }
  #endregion

  #region Methods
  public PromptBoxUniversalBuilder(string imageName, Vector2 imagePos, Transform parentTransform, float scaleRadio = 1.0f)
  {
    // BackgroundImage ===============================================================================
    ScreenMask = new ScreenMaskUniversalBuilder(parentTransform);
 
    // SelectBoxImage =================================================================================
    TextBoxBuilder = new TextBoxUniversalBuilder(imageName, ScreenMask.Transform, imagePos, 2, scaleRadio, false);

    // PromptButtonBuilder ============================================================================
    PromptButtonBuilder = new("CloseButton", new Vector2(0.5f, 0.0f), TextBoxBuilder.Transform, 
    new Dictionary<string, UnityAction>
    {
      {"onClickAction", OnPromptButtonClick},
    }, 4);

    SetActive(false);
  }

  public void ModifyButtonContent(string content)
  {
    PromptButtonBuilder.ModifyContent(content);
  }

  public void ModifyPromptContent(string content)
  {
    TextBoxBuilder.ModifyContent(content);
  }

  public void SetActive(bool state)
  {
    ScreenMask.SetActive(state);
    TextBoxBuilder.SetActive(state);
    PromptButtonBuilder.SetActive(state);
    DebugInfo.Print("set to " + state);
  }

  public void ModifyContent(string content)
  {
    TextBoxBuilder.ModifyContent(content);
  }

  public void OnPromptButtonClick()
  {
    if (DebugInfo.PrintSceneInfo)
    {
      Debug.Log("prompt button clicked");
    }
    SetActive(false);
  }
  #endregion
}