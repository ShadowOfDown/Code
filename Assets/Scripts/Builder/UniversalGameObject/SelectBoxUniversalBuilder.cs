// 选择框的通用类
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using UnityEngine.UIElements;

public class SelectBoxUniversalBuilder : UniversalGameObjectBuilder
{
  #region Fields
  public static int buttonBoxLimit = 3;
  #endregion

  #region Properties
  public override string Type { get; } = "LoadingSlider";
  public override string GameObjectName { get { return TextBoxBuilder.GameObjectName; } }
  public override Transform Transform { get { return TextBoxBuilder.Transform; } }
  public override RectTransform RectTransform { get{ return TextBoxBuilder.RectTransform; } }
  public TextBoxUniversalBuilder TextBoxBuilder { get; }
  public Dictionary<string, ButtonTextGameObjectBuilder> ButtonBuilderList { get; }
  #endregion

  #region Methods
  public SelectBoxUniversalBuilder(string imageName, Vector2 imagePos, Transform parentTransform, SortedDictionary<string, UnityAction> buttonActions, float scaleRadio = 1.0f)
  {

    int buttonCount = buttonActions.Count;
    // SelectBoxImage =================================================================================
    Dictionary<string, string> imageNameArgu = new()
    {
      {"Image", imageName},
      {"Text", "NameCardText"},
    };
    TextBoxBuilder = new TextBoxUniversalBuilder(imageName, parentTransform, imagePos, 2, scaleRadio, true);

    // ButtonBuilderList ==============================================================================
    ButtonBuilderList = new Dictionary<string, ButtonTextGameObjectBuilder>();
    int buttonIdx = 1;
    foreach (string content in buttonActions.Keys)
    {
      Dictionary<string, string> buttonNameArgu = new()
      {
        {"Button", "Button" + buttonIdx.ToString()},
        {"Text", "Text" + buttonIdx.ToString()}
      };

      float posX = buttonIdx / (buttonCount + 1.0f);

      UnityAction OnButtonClick = buttonActions[content];

      ButtonTextGameObjectBuilder button = new(buttonNameArgu, TextBoxBuilder.Transform);
      button.Build(
        new SortedDictionary<string, Dictionary<string, IComponentBuilder>>
        {
          {"Button", new Dictionary<string, IComponentBuilder>
            {
              {"RectTransform", new RectTransformComponentBuilder(
                new Dictionary<string, Vector2>
                {
                  {"referenceObjectPixels", GetReferenceSelectBoxButtonPixel(buttonCount, scaleRadio)},
                  {"archorMin", new Vector2(posX, 0.25f)},
                  {"archorMax", new Vector2(posX, 0.25f)},
                  {"pivotPos", new Vector2(0.5f, 0.5f)},
                }
              )},
              {"Image" , new ImageComponentBuilder(
                new Dictionary<string, string>
                {
                  {"gameObjectName", ButtonUniversalbuilder.GetButtonName(3, false)},
                  {"resourcesFolderPath", resourcesFolderPath},
                }
              )},
              {"Button", new ButtonComponentBuilder(
                new Dictionary<string, string>
                {
                  {"gameObjectName", ButtonUniversalbuilder.GetButtonName(3, true)},
                  {"resourcesFolderPath", resourcesFolderPath},
                }
              )},
              {"EventTrigger", new EventTriggerComponentBuilder(new Dictionary<string, UnityAction>
                {
                  {"onClickAction", OnButtonClick},
                  {"onHoverAction", null}
                }
              )}
            }
          },
          {"Text", new Dictionary<string, IComponentBuilder>
            {
              {"RectTransform" , new RectTransformComponentBuilder(
                new Dictionary<string, Vector2>
                {
                  {"referenceObjectPixels", GetReferenceSelectBoxButtonPixel(buttonCount, scaleRadio)},
                  {"archorMin", new Vector2(0.5f, 0.5f)},
                  {"archorMax", new Vector2(0.5f, 0.5f)},
                  {"pivotPos", new Vector2(0.5f, 0.5f)},
                }
              )},
              {"Text", new TextComponentBuilder(TextBoxUniversalBuilder.textArgu_02)},
              {"ContentSizeFitter", new ContentSizeFitterComponentBuilder(new Dictionary<string, bool>
                {
                  {"horizontalFit", false},
                  {"verticalFit", false},
                }
              )},
            }
          }
        }
      );
      button.ModifyContent(content);

      buttonIdx++;
      Debug.Log(buttonIdx);
    }
  }

  public override void SetActive(bool state)
  {
    TextBoxBuilder.SetActive(state);
    foreach (ButtonTextGameObjectBuilder button in ButtonBuilderList.Values)
    {
      button.SetActive(state);
    }
  }

  public void ModifyContent(string content)
  {
    TextBoxBuilder.ModifyContent(content);
  }

  public static float GetScaleRadio(int buttonCount)
  {
    return buttonCount / 10.0f + 0.7f;
  }

  public static Vector2 GetReferenceSelectBoxButtonPixel(int buttonCount, float scaleRadio = 1.0f)
  {
    return 0.25f * GetScaleRadio(buttonCount) * scaleRadio * TextBoxUniversalBuilder.GetReferencePixel(3, 1);
  }
  #endregion
}