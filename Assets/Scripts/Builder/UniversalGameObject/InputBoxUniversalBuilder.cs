using UnityEngine;
using System.Collections.Generic;

class InputBoxUniversalBuilder : UniversalGameObjectBuilder
{
  #region Fields
  public static readonly int inputFieldImageLimit = 4;
  #endregion


  #region Properties
  public override string Type { get; } = "InputBox";
  public override string GameObjectName { get { return InputBoxBuilder.GameObjectName;} }
  public override Transform Transform { get { return InputBoxBuilder.Transform; } }
  public override RectTransform RectTransform { get { return InputBoxBuilder.RectTransform;} }
  public InputBoxGameObjectBuilder InputBoxBuilder{ get; }
  public int AspectRadio { get; set; }
  public float ScaleRadio { get; set; }
  #endregion


  #region Methods
  public InputBoxUniversalBuilder(string name, Transform parentTransform, Vector2 pos, int aspectRadio = 3, float scaleRadio = 1.0f)
  {
    if (aspectRadio < 0 || aspectRadio > inputFieldImageLimit)
    {
      Debug.LogWarning("invalid scaleradio " + aspectRadio);
      aspectRadio = inputFieldImageLimit;
    }

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
                {"gameObjectName", GetInputFieldImageName(AspectRadio)},
                {"resourcesFolderPath", resourcesFolderPath},
              })
            },
            {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
              {
                {"referenceObjectPixels", GetReferencePixel(AspectRadio, ScaleRadio)},
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
                {"referenceObjectPixels", GetReferencePixel(AspectRadio, scaleRadio, true)},
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

  public static string GetInputFieldImageName(int aspectRadio)
  {
    if (aspectRadio < 1 || aspectRadio > inputFieldImageLimit)
    {
      return "InputBackgroundImage_03";
    }

    return "InputBackgroundImage_0" + aspectRadio.ToString();
  }  

  public static Vector2 GetReferencePixel(int aspectRadio, float scaleRadio, bool isText = false)
  {
    return aspectRadio switch
    {
      1 => (isText ? 0.9f : 1) * scaleRadio * new Vector2(480, 80),
      2 => (isText ? 0.9f : 1) * scaleRadio * new Vector2(640, 80),
      3 => (isText ? 0.9f : 1) * scaleRadio * new Vector2(1080, 80),
      _ => (isText ? 0.9f : 1) * scaleRadio * new Vector2(840, 80),
    };
  }

  public string GetContent()
  {
    return InputBoxBuilder.GetContent();
  }
  #endregion
}