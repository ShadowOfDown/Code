// 提示框的那个框
using UnityEngine;
using System.Collections.Generic;

public class TextBoxUniversalBuilder : UniversalGameObjectBuilder
{
  #region Fields
  public readonly static int textBoxLimit = 5 + 1;
  public readonly static int boxLimit = 3 + 1;
  public readonly bool _isTextNeeded = true;
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
    string imageName, Transform parentTransform, Vector2 imagePos, int aspectRadio = 2, float scaleRadio = 0.5f, bool isTextNeeded = true)
  {
    _isTextNeeded = isTextNeeded;
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
                {"referenceObjectPixels", GetReferencePixel(aspectRadio, scaleRadio, isTextNeeded)},
                {"archorMin", imagePos},
                {"archorMax", imagePos},
                {"pivotPos", new Vector2(0.5f, 0.5f)},
              })
            },
            {"Image", new ImageComponentBuilder(
              new Dictionary<string, string>
              {
                {"gameObjectName", GetImageName(aspectRadio, isTextNeeded)},
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
                {"referenceObjectPixels", GetReferencePixel(aspectRadio, scaleRadio, isTextNeeded)},
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

  public static string GetImageName(int aspectRadio, bool isTextNeeded = true)
  {
    return isTextNeeded ? "TextBoxBackgroundImage_0" + aspectRadio % textBoxLimit : "BoxBackgroundImage_0" + aspectRadio % boxLimit;
  }

  public static Vector2 GetReferencePixel(int aspectRadio, float scaleRadio = 1.0f, bool isTextNeeded = true)
  {
    return isTextNeeded ? aspectRadio switch
    {
      1 => new Vector2(640, 320) * scaleRadio,
      3 => new Vector2(1280, 320) * scaleRadio,
      4 => new Vector2(960, 160) * scaleRadio,
      5 => new Vector2(1280, 160) * scaleRadio,
      _ => new Vector2(960, 320) * scaleRadio,
    } : aspectRadio switch
    {
      1 => new Vector2(160, 80) * scaleRadio,
      2 => new Vector2(240, 80) * scaleRadio,
      _ => new Vector2(320, 80) * scaleRadio,
    };
  }
  #endregion
}