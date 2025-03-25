// 文本输入框的通用类
using UnityEngine;
using System.Collections.Generic;
using System;

class InputBoxUniversalBuilder : UniversalGameObjectBuilder
{
  #region Fields
  public static readonly int inputFieldImageLimit = 4;
  #endregion


  #region Properties
  public override string Type { get; } = "InputBox";
  public override string GameObjectName { get { return InputBoxBuilder.GameObjectName; } }
  public override Transform Transform { get { return InputBoxBuilder.Transform; } }
  public override RectTransform RectTransform { get { return InputBoxBuilder.RectTransform; } }
  public InputBoxGameObjectBuilder InputBoxBuilder { get; }
  public int AspectRadio { get; set; }
  public float ScaleRadio { get; set; }
  #endregion


  #region Methods
  public InputBoxUniversalBuilder(string name, Transform parentTransform, Vector2 pos, int aspectRadio = 3, float scaleRadio = 1.0f, bool isDark = false)
  {
    AspectRadio = aspectRadio;
    ScaleRadio = scaleRadio;

    InputBoxBuilder = new InputBoxGameObjectBuilder(name, parentTransform);
    InputBoxBuilder.Build(
      new SortedDictionary<string, Dictionary<string, IComponentBuilder>>
      {
        {"InputField", new Dictionary<string, IComponentBuilder>
          {
            {"Image", new ImageComponentBuilder(new Dictionary<string, string>
              {
                {"gameObjectName", GetInputFieldImageName(AspectRadio, isDark)},
                {"resourcesFolderPath", resourcesFolderPath},
              })
            },
            {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
              {
                {"referenceObjectPixels", GetReferencePixel(AspectRadio, ScaleRadio, isDark)},
                {"archorMin", pos},
                {"archorMax", pos},
                {"pivotPos", new Vector2(0.5f, 0.5f)},
              })
            },
            {"InputField", new InputFieldComponentBuilder(new Dictionary<string, List<float>>
              {
                {"pressedColor", new List<float>(){0.5f, 0.5f, 0.5f, 1f}}
              }
            )}
          }
        },
        {"TextArea", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
              {
                {"referenceObjectPixels", GetReferencePixel(AspectRadio, scaleRadio * 0.9f, isDark)},
                {"archorMin", Vector2.zero},
                {"archorMax", Vector2.one},
                {"pivotPos", new Vector2(0.5f, 0.5f)},
              }
            )}
          }
        },
        {"PlaceHolder", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransfrom", null},
            {"Text", new TextComponentBuilder(TextBoxUniversalBuilder.textArgu_01)},
          }
        },
        {"TextComponent", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransfrom", null},
            {"Text", new TextComponentBuilder(TextBoxUniversalBuilder.textArgu_01)},
          }
        }
      }
    );
  }
  public override void SetActive(bool state)
  {
    InputBoxBuilder.SetActive(state);
  }

  public static string GetInputFieldImageName(int aspectRadio, bool isDark = false)
  {
    if (aspectRadio < 0 || (!isDark & aspectRadio > inputFieldImageLimit))
    {
      Debug.LogWarning("invalid aspectradio " + aspectRadio);
      aspectRadio = 1;
    }

    return isDark ? TextBoxUniversalBuilder.GetImageName(aspectRadio, true) : "InputBackgroundImage_0" + aspectRadio;
  }

  public static Vector2 GetReferencePixel(int aspectRadio, float scaleRadio, bool isDark = false)
  {
    return isDark ? TextBoxUniversalBuilder.GetReferencePixel(aspectRadio, scaleRadio, true):
      aspectRadio switch
      {
        1 => new Vector2(120f, 20f) * scaleRadio,
        2 => new Vector2(210f, 20f) * scaleRadio,
        3 => new Vector2(210f, 20f) * scaleRadio,
        _ => new Vector2(320f, 20f) * scaleRadio,
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

  public string GetContent()
  {
    return InputBoxBuilder.GetContent();
  }
  #endregion
}