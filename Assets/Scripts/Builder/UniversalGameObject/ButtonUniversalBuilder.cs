// 按钮的通用类
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

public class ButtonUniversalbuilder : UniversalGameObjectBuilder
{
  #region Fields
  public readonly static int normalButtonLimit = 4;
  public readonly static int specialButtonLimit = 4;
  public const int searchButtonIndex = 1;
  public const int settingButtonIndex = 2;
  public const int exitButtonIndex = 3;
  public const int returnButtonIndex = 4;
  #endregion

  #region Properties
  public override string Type { get; } = "ButtonUniversalBuilder";
  public override string GameObjectName { get { return ButtonBuilder.GameObjectName; } }
  public override Transform Transform { get { return ButtonBuilder.Transform; } }
  public override RectTransform RectTransform { get{ return ButtonBuilder.RectTransform; } }
  public ButtonTextGameObjectBuilder ButtonBuilder { get; }
  #endregion

  #region Methods
  public ButtonUniversalbuilder(
    string buttonName, 
    Vector2 imagePos, 
    Transform parentTransform, 
    Dictionary<string, UnityAction> buttonActions, 
    int buttonCount = 3,    // 你要哪一个按钮呀?
    float scaleRadio = 1.0f, 
    bool isDark = true,
    bool isSpecial = false) // 是否是特殊按钮, 在序号中使用 9 开始
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
                {"referenceObjectPixels", GetReferencePixel(buttonCount, scaleRadio, isSpecial)},
                {"archorMin", imagePos},
                {"archorMax", imagePos},
                {"pivotPos", new Vector2(0.5f, 0.5f)},
              }
            )},
            {"Image" , new ImageComponentBuilder(
              new Dictionary<string, string>
              {
                {"gameObjectName", GetButtonName(buttonCount, isDark, isSpecial)},
                {"resourcesFolderPath", resourcesFolderPath}
              }
            )},
            {"Button", new ButtonComponentBuilder(
              new Dictionary<string, string>
              {
                {"gameObjectName", GetButtonName(buttonCount, !isDark, isSpecial)},
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
  }

  public override void SetActive(bool state)
  {
    ButtonBuilder.SetActive(state);
  }

  public void ModifyContent(string content)
  {
    ButtonBuilder.ModifyContent(content);
  }

  
  public static string GetButtonName(int aspectRadio, bool pressed = false, bool isSpecial = false)
  {
    int firstDigit = isSpecial ? 9 : 0;

    if (isSpecial)
    {
      if (aspectRadio < 0 || aspectRadio > specialButtonLimit)
      {
        aspectRadio = exitButtonIndex;
        Debug.LogWarning("Aspect radio out of range, use exit button instead.");
      }
    } 
    else
    {
      if (aspectRadio < 0 || aspectRadio > normalButtonLimit)
      {
        aspectRadio = 4;
        Debug.LogWarning("Aspect radio out of range, use search button instead.");
      }
    }

    return (pressed ? "Pressed" : "Normal") + "ButtonImage_" + firstDigit.ToString() + aspectRadio.ToString();
  }

  public static Vector2 GetReferencePixel(int aspectRadio, float scaleRadio = 1.0f, bool isSpecial = false)
  {
    return isSpecial ? new Vector2(100, 100) * scaleRadio :
    aspectRadio switch
    {
      1 => new Vector2(160, 160) * scaleRadio,
      2 => new Vector2(160, 80) * scaleRadio,
      3 => new Vector2(240, 80) * scaleRadio,
      _ => new Vector2(240, 60) * scaleRadio,
    };
    ;
  }

  public static float GetScaleRadio(int aspectRadio, Vector2 referencePixel, bool isSpecial)
  {
    if (referencePixel.x <= 0 && referencePixel.y <= 0)
    {
      Debug.LogWarning("Reference pixel is zero, use 1.0f instead.");
      return 1.0f;
    }

    var stdReferencePixel =  GetReferencePixel(aspectRadio, 1.0f, isSpecial);
    return Math.Min(referencePixel.x > 0 ? referencePixel.x / stdReferencePixel.x : float.MaxValue, referencePixel.y > 0 ? referencePixel.y / stdReferencePixel.y : float.MaxValue);
  }

  public static float GetReferenceAspectRadio(int aspectRadio, bool isSpecial = true) 
  {  
    var referencePixel = GetReferencePixel(aspectRadio, 1.0f, isSpecial);
    return referencePixel.x / referencePixel.y;
  }

  public static Vector2 GetPos(float height, Vector2 referencePixel)
  {
    Debug.Log($"========================================================================================={new Vector2(referencePixel.x - height * 1.5f / referencePixel.x, referencePixel.y - height * 1.5f / referencePixel.y)}");
    return new Vector2((referencePixel.x - height * 0.6f) / referencePixel.x, (referencePixel.y - height * 0.6f) / referencePixel.y);
  }

  #endregion
}