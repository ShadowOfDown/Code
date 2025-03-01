// 按钮的通用类
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

public class ButtonBoxGameObjectbuilder
{
  #region Properties
  public ButtonTextGameObjectBuilder ButtonBuilder { get; }
  #endregion

  #region Methods
  public ButtonBoxGameObjectbuilder(string buttonName, Vector2 imagePos, Transform parentTransform, Dictionary<string, UnityAction> buttonActions, int aspectRedio = 3)
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
                {"referenceObjectPixels", UniversalArgus.GetReferenceButtonPixel(aspectRedio)},
                {"archorMin", imagePos},
                {"archorMax", imagePos},
                {"pivotPos", new Vector2(0.5f, 0.5f)},
              }
            )},
            {"Image" , new ImageComponentBuilder(
              new Dictionary<string, string>
              {
                {"gameObjectName", UniversalArgus.GetButtonName(aspectRedio)},
                {"resourcesFolderPath", UniversalArgus.resourcesFolderPath}
              }
            )},
            {"Button", new ButtonComponentBuilder(
              new Dictionary<string, string>
              {
                {"gameObjectName", UniversalArgus.GetButtonName(aspectRedio, true)},
                {"resourcesFolderPath", UniversalArgus.resourcesFolderPath}
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
                {"referenceObjectPixels", UniversalArgus.GetReferenceButtonPixel(aspectRedio, true)},
                {"archorMin", new Vector2(0.5f, 0.5f)},
                {"archorMax", new Vector2(0.5f, 0.5f)},
                {"pivotPos", new Vector2(0.5f, 0.5f)},
              }
            )},
            {"Text", new TextComponentBuilder(UniversalArgus.textArgu)},
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

  public void SetActive(bool state)
  {
    ButtonBuilder.SetActive(state);
  }

  public void ModifyContent(string content)
  {
    ButtonBuilder.ModifyContent(content);
  }
  #endregion
}