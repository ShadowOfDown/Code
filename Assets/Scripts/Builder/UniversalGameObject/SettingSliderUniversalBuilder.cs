using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SettingSliderUniversalBuilder : IUniversalGameObjectBuilder
{
  #region Fields
  private SliderGameObjectBuilder _sliderBarBuilder;
  private TextBoxGameObjectBuilder _optionBoxBuilder;
  private TextBoxGameObjectBuilder _valueBoxBuilder;
  private const float _textBoxPosX = 0.1f;
  private const float _valueBoxPosX = 0.92f;
  private const float _sliderPosX = 0.52f;
  private readonly Vector2 _scaleRadioMax = new(0.95f, 0.9f);
  private readonly Vector2 _scaleRadioMin = new(0.05f, 0.1f);
  #endregion

  #region Properties
  public float Height { get; set; }
  public string Type { get; } = "SettingSliderGameObjectBuilder";
  public string GameObjectName { get; }
  public Transform Transform { get; }
  public RectTransform RectTransform { get; }
  #endregion

  #region Methods
  public SettingSliderUniversalBuilder(string sliderName, Transform parentTransform, float posY)
  {
    GameObjectName = sliderName;
    Height = posY;

    // OptionBoxBuilder ===============================================================================
    _optionBoxBuilder = new TextBoxGameObjectBuilder(new Dictionary<string, string>
    {
      {"Text", "OptionText"},
      {"Image", "OptionBox"}, 
    }, parentTransform);
    _optionBoxBuilder.Build(
      new SortedDictionary<string, Dictionary<string, IComponentBuilder>>
      {
        {"Image", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(
              new Dictionary<string, Vector2>
              {
                {"referenceObjectPixels", new Vector2(240, 80)},
                {"archorMin", new Vector2(_textBoxPosX, posY)},
                {"archorMax", new Vector2(_textBoxPosX, posY)},
                {"pivotPos", new Vector2(0.5f, 0.5f)},
              }
            )},
            {"Image", new ImageComponentBuilder(
              new Dictionary<string, string>
              {
                {"gameObjectName", "OptionBoxImage"},
                {"resourcesFolderPath", SettingScene.resourcesFolderPath},
              }
            )},
          }
        },
        {"Text", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(
              new Dictionary<string, Vector2>
              {
                {"referenceObjectPixels", new Vector2(240 * 0.9f, 80 * 0.9f)},
                {"archorMin", new Vector2(0.5f, 0.5f)},
                {"archorMax", new Vector2(0.5f, 0.5f)},
                {"pivotPos", new Vector2(0.5f, 0.5f)},
              }
            )},
            {"Text", new TextComponentBuilder(TextBoxUniversalBuilder.textArgu_01)},
            {"ContentSizeFitter", new ContentSizeFitterComponentBuilder(
              new Dictionary<string, bool>
              {
                {"horizontalFit", false},
                {"verticalFit", false},
              }
            )},
          }
        },
      }
    );
    _optionBoxBuilder.ModifyContent(sliderName);

    // SliderBarSlider ================================================================================
    _sliderBarBuilder = new SliderGameObjectBuilder(sliderName, parentTransform, true);
    _sliderBarBuilder.Build(
      new SortedDictionary<string, Dictionary<string, IComponentBuilder>>
      {
        {"SliderArea", new Dictionary<string, IComponentBuilder>
          { 
            {"RectTransform", new RectTransformComponentBuilder(
              new Dictionary<string, Vector2>
              {
                {"referenceObjectPixels", new Vector2(1280, 40)},
                {"archorMin", new Vector2(_sliderPosX, posY)},
                {"archorMax", new Vector2(_sliderPosX, posY)},
                {"pivotPos", new Vector2(0.5f, 0.5f)},
              } 
            )} 
          }
        },
        {"BackgroundImage", new Dictionary<string, IComponentBuilder>
          {
            {"Image", new ImageComponentBuilder(
              new Dictionary<string, string>
              {
                {"gameObjectName", "SlidingBarImage"},
                {"resourcesFolderPath", SettingScene.resourcesFolderPath}
              }
            )},
            {"RectTransform", new RectTransformComponentBuilder(
              new Dictionary<string, Vector2>
              {
                {"referenceObjectPixels", new Vector2(1280, 40)},
                {"archorMin", new Vector2(0.5f, 0.5f)},
                {"archorMax", new Vector2(0.5f, 0.5f)},
                {"pivotPos", new Vector2(0.5f, 0.5f)},
              }
            )}
          }
        },
        {"FillImage", new Dictionary<string, IComponentBuilder>
          {
            {"Image", new ImageComponentBuilder(
              new Dictionary<string, string>
              {
                {"gameObjectName", "FillingBarImage"},
                {"resourcesFolderPath", SettingScene.resourcesFolderPath}
              }
            )},
            {"RectTransform", new RectTransformComponentBuilder(
              new Dictionary<string, Vector2>
              {
                {"referenceObjectPixels", new Vector2(-1, -1)},
                {"archorMin", _scaleRadioMin},
                {"archorMax", _scaleRadioMax},
                {"pivotPos", new Vector2(0.5f, 0.5f)}, 
              }
            )}
          }
        },
        {
          "FillArea", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(
              new Dictionary<string, Vector2>
              {
                {"referenceObjectPixels", new Vector2(-1, -1)},
                {"archorMin", _scaleRadioMin},
                {"archorMax", _scaleRadioMax},
                {"pivotPos", new Vector2(0.5f, 0.5f)}, 
              })
            }
          }
        },
        {"HandleImage", new Dictionary<string, IComponentBuilder>
          {
            {"Image", new ImageComponentBuilder(
              new Dictionary<string, string>
              {
                {"gameObjectName", "HandleImage"},
                {"resourcesFolderPath", SettingScene.resourcesFolderPath}
              }
            )},
            {"RectTransform", new RectTransformComponentBuilder(
              new Dictionary<string, Vector2>
              {
                {"referenceObjectPixels", new Vector2(30, 50)},
                {"archorMin", new Vector2(-0.1f, -0.1f)},
                {"archorMax", new Vector2(-0.1f, -0.1f)},
                {"pivotPos", new Vector2(0.5f, 0.5f)}, 
              }
            )}
          }
        },
        {
          "HandleArea", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(
              new Dictionary<string, Vector2>
              {
                {"referenceObjectPixels", new Vector2(-1, -1)},
                {"archorMin", _scaleRadioMin},
                {"archorMax", _scaleRadioMax},
                {"pivotPos", new Vector2(0.5f, 0.5f)}, 
              })
            }
          }
        },
      }
    );

  // ValueBoxSlider ===================================================================================
  _valueBoxBuilder = new TextBoxGameObjectBuilder(new Dictionary<string, string>
    {
      {"Text", "ValueText"},
      {"Image", "ValueBox"}, 
    }, parentTransform);
    _valueBoxBuilder.Build(
      new SortedDictionary<string, Dictionary<string, IComponentBuilder>>
      {
        {"Image", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(
              new Dictionary<string, Vector2>
              {
                {"referenceObjectPixels", new Vector2(160, 80)},
                {"archorMin", new Vector2(_valueBoxPosX, posY)},
                {"archorMax", new Vector2(_valueBoxPosX, posY)},
                {"pivotPos", new Vector2(0.5f, 0.5f)},
              }
            )},
            {"Image", new ImageComponentBuilder(
              new Dictionary<string, string>
              {
                {"gameObjectName", "ValueBoxImage"},
                {"resourcesFolderPath", SettingScene.resourcesFolderPath},
              }
            )},
          }
        },
        {"Text", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(
              new Dictionary<string, Vector2>
              {
                {"referenceObjectPixels", new Vector2(160 * 0.9f, 80 * 0.9f)},
                {"archorMin", new Vector2(0.5f, 0.5f)},
                {"archorMax", new Vector2(0.5f, 0.5f)},
                {"pivotPos", new Vector2(0.5f, 0.5f)},
              }
            )},
            {"Text", new TextComponentBuilder(TextBoxUniversalBuilder.textArgu_01)},
            {"ContentSizeFitter", new ContentSizeFitterComponentBuilder(
              new Dictionary<string, bool>
              {
                {"horizontalFit", false},
                {"verticalFit", false},
              }
            )},
          }
        },
      }
    );
    _valueBoxBuilder.ModifyContent(0.ToString());
  }

  public void SetActive(bool state)
  {
    _sliderBarBuilder.SetActive(state);
  }
  #endregion
}