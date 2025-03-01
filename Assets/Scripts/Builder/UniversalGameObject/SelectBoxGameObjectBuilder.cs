// 选择框的通用类
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

public class SelectBoxGameObjectBuilder
{
  #region Properties
  public TextBoxGameObjectBuilder TextBoxBuilder { get; }
  public RectTransform RectTransform { get { return TextBoxBuilder.RectTransform; } }
  public Dictionary<string, ButtonTextGameObjectBuilder> ButtonBuilderList { get; }
  #endregion

  #region Methods
  public SelectBoxGameObjectBuilder(string imageName, Vector2 imagePos, Transform parentTransform, SortedDictionary<string, UnityAction> buttonActions)
  {

    int buttonCount = buttonActions.Count;
    // SelectBoxImage =================================================================================
    Dictionary<string, string> imageNameArgu = new()
    {
      {"Image", imageName},
      {"Text", "NameCardText"},
    };
    TextBoxBuilder = new TextBoxGameObjectBuilder(imageNameArgu, parentTransform);
    TextBoxBuilder.Build(
      new SortedDictionary<string, Dictionary<string, IComponentBuilder>>
      {
        { "Image", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(
              new Dictionary<string, Vector2>
              {
                {"referenceObjectPixels", UniversalArgus.GetReferenceSelectBoxImagePixel(buttonCount) },
                {"archorMin", imagePos},
                {"archorMax", imagePos},
                {"pivotPos", new Vector2(0.5f, 0.5f)},
              })
            },
            {"Image", new ImageComponentBuilder(
              new Dictionary<string, string>
              {
                {"gameObjectName", UniversalArgus.selectBoxImageName},
                {"resourcesFolderPath", UniversalArgus.resourcesFolderPath}
              })
            }
          }
        },
        { "Text", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(
              new Dictionary<string, Vector2>
              {
                {"referenceObjectPixels", UniversalArgus.GetReferenceSelectBoxTextPixel(buttonCount)},
                {"archorMin", new Vector2(0.5f, UniversalArgus.selectBoxTextHeight)},
                {"archorMax", new Vector2(0.5f, UniversalArgus.selectBoxTextHeight)},
                {"pivotPos", new Vector2(0.5f, 0.5f)},
              }
            )},
            { "Text", new TextComponentBuilder(UniversalArgus.textArgu)},
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
                  {"referenceObjectPixels", UniversalArgus.GetReferenceSelectBoxButtonPixel(buttonCount)},
                  {"archorMin", new Vector2(posX, 0.25f)},
                  {"archorMax", new Vector2(posX, 0.25f)},
                  {"pivotPos", new Vector2(0.5f, 0.5f)},
                }
              )},
              {"Image" , new ImageComponentBuilder(
                new Dictionary<string, string>
                {
                  {"gameObjectName", UniversalArgus.normalSelectBoxButtonImageName},
                  {"resourcesFolderPath", UniversalArgus.resourcesFolderPath},
                }
              )},
              {"Button", new ButtonComponentBuilder(
                new Dictionary<string, string>
                {
                  {"gameObjectName", UniversalArgus.pressedSelectBoxButtonImageName},
                  {"resourcesFolderPath", UniversalArgus.resourcesFolderPath},
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
                  {"referenceObjectPixels", UniversalArgus.GetReferenceSelectBoxButtonTextPixel(buttonCount)},
                  {"archorMin", new Vector2(0.5f, 0.5f)},
                  {"archorMax", new Vector2(0.5f, 0.5f)},
                  {"pivotPos", new Vector2(0.5f, 0.5f)},
                }
              )},
              {"Text", new TextComponentBuilder(UniversalArgus.textArgu)},
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

  public void SetActive(bool state)
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
  #endregion
}