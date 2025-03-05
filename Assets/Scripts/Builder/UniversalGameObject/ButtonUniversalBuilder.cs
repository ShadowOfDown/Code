// 按钮的通用类
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

public class ButtonUniversalbuilder : UniversalGameObjectBuilder
{
  #region Properties
  public override string Type { get; } = "LoadingSlider";
  public override string GameObjectName { get { return ButtonBuilder.GameObjectName; } }
  public override Transform Transform { get { return ButtonBuilder.Transform; } }
  public override RectTransform RectTransform { get{ return ButtonBuilder.RectTransform; } }
  public ButtonTextGameObjectBuilder ButtonBuilder { get; }
  #endregion

  #region Methods
  public ButtonUniversalbuilder(string buttonName, Vector2 imagePos, Transform parentTransform, Dictionary<string, UnityAction> buttonActions, int buttonCount = 3, float scaleRadio = 1.0f)
  {
    Dictionary<string, string> buttonNameArgu = new()
    {
      {"Button", buttonName},
      {"Text", "buttonText"},
    };

    ButtonBuilder = new ButtonTextGameObjectBuilder(buttonNameArgu, parentTransform);
    ButtonBuilder.Build(
      new SortedDictionary<string, Dictionary<string, IComponentBuilder>>
      {
        {"Button", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(
              new Dictionary<string, Vector2>
              {
                {"referenceObjectPixels", GetReferencePixel(buttonCount, scaleRadio)},
                {"archorMin", imagePos},
                {"archorMax", imagePos},
                {"pivotPos", new Vector2(0.5f, 0.5f)},
              }
            )},
            {"Image" , new ImageComponentBuilder(
              new Dictionary<string, string>
              {
                {"gameObjectName", GetButtonName(buttonCount)},
                {"resourcesFolderPath", resourcesFolderPath}
              }
            )},
            {"Button", new ButtonComponentBuilder(
              new Dictionary<string, string>
              {
                {"gameObjectName", GetButtonName(buttonCount, true)},
                {"resourcesFolderPath", resourcesFolderPath}
              }
            )},
            {"EventTrigger", new EventTriggerComponentBuilder(buttonActions)}
          }
        },
        {"Text", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform" , new RectTransformComponentBuilder(
              new Dictionary<string, Vector2>
              {
                {"referenceObjectPixels", GetReferencePixel(buttonCount, scaleRadio , true)},
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

    if (buttonCount < 1 || buttonCount > SelectBoxUniversalBuilder.buttonLimit)
    {
      ButtonBuilder.ButtonBuilder.ImageBuilder.Image.alphaHitTestMinimumThreshold = 0.1f;
      ButtonBuilder.ButtonBuilder.ImageBuilder.Sprite.texture.Apply(true);
    }
  }

  public override void SetActive(bool state)
  {
    ButtonBuilder.SetActive(state);
  }

  public void ModifyContent(string content)
  {
    ButtonBuilder.ModifyContent(content);
  }

  
  public static string GetButtonName(int aspectRadio, bool pressed = false)
  {
    if (aspectRadio < 1 || aspectRadio > SelectBoxUniversalBuilder.buttonLimit)
    {
      return (pressed ? "Pressed" : "Normal") + "SelectBoxButtonImage_0c";
    }

    return (pressed ? "Pressed" : "Normal") + "SelectBoxButtonImage_0" + aspectRadio.ToString();
  }

  public static Vector2 GetReferencePixel(int aspectRadio, float scaleRadio = 1.0f, bool isText = false)
  {
    return aspectRadio switch
    {
      1 => new Vector2(160 * (isText ? 0.9f : 1.0f), 160 * (isText ? 0.9f : 1.0f)) * scaleRadio,
      2 => new Vector2(160 * (isText ? 0.9f : 1.0f), 80 * (isText ? 0.9f : 1.0f)) * scaleRadio,
      3 => new Vector2(240 * (isText ? 0.9f : 1.0f), 80 * (isText ? 0.9f : 1.0f)) * scaleRadio,
      4 => new Vector2(240 * (isText ? 0.9f : 1.0f), 60 * (isText ? 0.9f : 1.0f)) * scaleRadio,
      _ => new Vector2(100 * (isText ? 0.9f : 1.0f), 100 * (isText ? 0.9f : 1.0f)) * scaleRadio,
    };
    ;
  }
  #endregion
}