// 文本框的通用类
using UnityEngine;
using System.Collections.Generic;
using System;

public class TextBoxUniversalBuilder : UniversalGameObjectBuilder
{
  #region Fields
  public readonly static int textBoxLimit = 5;  // 文本框的最大值
  public readonly static int boxLimit = 4;  // 图片框的最大值
  public readonly bool _isDark = true;
  #endregion


  #region Properties
  public override string Type { get; } = "BoxGameObjectBuilder";
  public override string GameObjectName { get { return BoxGameObject.GameObjectName; } }
  public override Transform Transform { get { return BoxGameObject.Transform; } }
  public override RectTransform RectTransform { get { return BoxGameObject.RectTransform; } }
  public TextBoxGameObjectBuilder BoxGameObject { get; }
  #endregion


  #region Methods

  public TextBoxUniversalBuilder(
    string imageName, 
    Transform parentTransform, 
    Vector2 imagePos, 
    int aspectRadio = 2, 
    float scaleRadio = 0.5f, 
    bool isDark = true)  // 文本框是深色的, 但是在提示框中颜色是浅色的, 按钮是深色的
  {
    _isDark = isDark;
    Dictionary<string, string> imageNameArgu = new()
    {
      {"Image", imageName},
      {"Text", "NameCardText"},
    };
    BoxGameObject = new TextBoxGameObjectBuilder(imageNameArgu, parentTransform);
    BoxGameObject.Build(
      new SortedDictionary<string, Dictionary<string, IComponentBuilder>>
      {
        { "Image", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(
              new Dictionary<string, Vector2>
              {
                {"referenceObjectPixels", GetReferencePixel(aspectRadio, scaleRadio, isDark)},
                {"archorMin", imagePos},
                {"archorMax", imagePos},
                {"pivotPos", new Vector2(0.5f, 0.5f)},
              })
            },
            {"Image", new ImageComponentBuilder(
              new Dictionary<string, string>
              {
                {"gameObjectName", GetImageName(aspectRadio, isDark)},
                {"resourcesFolderPath", resourcesFolderPath}
              })
            }
          }
        },
        { "Text", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(
              new Dictionary<string, Vector2>
              {
                {"referenceObjectPixels", GetReferencePixel(aspectRadio, scaleRadio * 0.9f, isDark)},
                {"archorMin", new Vector2(0.5f, 0.5f)},
                {"archorMax", new Vector2(0.5f, 0.5f)},
                {"pivotPos", new Vector2(0.5f, 0.5f)},
              }
            )},
            { "Text", new TextComponentBuilder(textArgu_02)},
            {"ContentSizeFitter", new ContentSizeFitterComponentBuilder(new Dictionary<string, bool>
                {
                  {"horizontalFit", false},
                  {"verticalFit", false},
                }
              )
            },
          }
        }
      }
    );
  }

  public override void SetActive(bool state)
  {
    BoxGameObject.SetActive(state);
  }

  public void ModifyContent(string content)
  {
    BoxGameObject.ModifyContent(content);
  }

  public readonly static Dictionary<string, List<float>> textArgu_01 = new()
  {
    {"fontSize", new List<float>{30f}},
    {"color", new List<float>{0.0f, 0.0f, 0.0f, 1.0f}},
    {"alignmentOption", TextComponentBuilder.middle},
    {"font", TextComponentBuilder.song},
  };

  public readonly static Dictionary<string, List<float>> textArgu_02 = new()
  {
    {"fontSize", new List<float>{40f}},
    {"color", new List<float>{0.0f, 0.0f, 0.0f, 1.0f}},
    {"alignmentOption", TextComponentBuilder.middle},
    {"font", TextComponentBuilder.song},
  };

  public static string GetImageName(int aspectRadio, bool isDark = true)  // 前者是深色的, 后者是浅色的
  {
    if (aspectRadio < 0 || (isDark & aspectRadio > textBoxLimit) || (!isDark & aspectRadio > boxLimit))
    {
      Debug.LogError("AspectRadio is out of range!");
      aspectRadio = 1;
    }

    return isDark ? "TextBoxBackgroundImage_0" + aspectRadio : "BoxBackgroundImage_0" + aspectRadio;
  }

  public static Vector2 GetReferencePixel(int aspectRadio, float scaleRadio = 1.0f, bool isDark = true)
  {
    // TextBox 是深色的, input 是浅色的
    return isDark ? aspectRadio switch
    {
      1 => new Vector2(200, 100) * scaleRadio,
      2 => new Vector2(300, 100) * scaleRadio,
      3 => new Vector2(400, 100) * scaleRadio,
      4 => new Vector2(600, 100) * scaleRadio,
      _ => new Vector2(800, 100) * scaleRadio,
    } : aspectRadio switch
    {
      1 => new Vector2(400, 200) * scaleRadio,
      2 => new Vector2(681, 194) * scaleRadio,
      3 => new Vector2(300, 100) * scaleRadio,
      _ => new Vector2(400, 100) * scaleRadio,
    };
  }

  public static float GetScaleRadio(int aspectRadio, Vector2 referencePixel, bool isDark = true)
  {
    if (referencePixel.x <= 0 && referencePixel.y <= 0)
    {
      Debug.LogWarning("Reference pixel is zero, use 1.0f instead.");
      return 1.0f;
    }

    var stdReferencePixel =  GetReferencePixel(aspectRadio, 1.0f, isDark);
    return Math.Min(referencePixel.x > 0 ? referencePixel.x / stdReferencePixel.x : float.MaxValue, referencePixel.y > 0 ? referencePixel.y / stdReferencePixel.y : float.MaxValue);
  }

  public static float GetReferenceAspectRadio(int aspectRadio, bool isDark = true) 
  {  
    var referencePixel = GetReferencePixel(aspectRadio, 1.0f, isDark);
    return referencePixel.x / referencePixel.y;
  }

  #endregion
}