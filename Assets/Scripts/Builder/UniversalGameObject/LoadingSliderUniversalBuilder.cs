// 加载条的通用类
using System.Collections.Generic;
using UnityEngine;

public class LoadingSliderUniversalBuilder : UniversalGameObjectBuilder 
{
  #region Fields
  // Slider ===========================================================================================
  private readonly SliderGameObjectBuilder _loadingBar;
  private readonly Dictionary<string, Vector2> _sliderAreaRectTranform = new()
  {
    {"referenceObjectPixels", referenceSliderPixel},
    {"archorMin", new Vector2(0.5f, 0.382f)},
    {"archorMax", new Vector2(0.5f, 0.382f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  private readonly Dictionary<string, Vector2> _backgroundRectTranform = new()
  {
    {"referenceObjectPixels", referenceSliderPixel},
    {"archorMin", new Vector2(0.5f, 0.5f)},
    {"archorMax", new Vector2(0.5f, 0.5f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  private readonly Dictionary<string, Vector2> _fillingAreaRectTranform = new()
  {
    {"referenceObjectPixels", referenceSliderPixel},
    {"archorMin", new Vector2(0.5f, 0.5f)},
    {"archorMax", new Vector2(0.5f, 0.5f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  private readonly Dictionary<string, string> _backgroundImage = new()
  {
    {"gameObjectName", "LoadingBoxImage"},
    {"resourcesFolderPath", resourcesFolderPath}
  };
  private readonly Dictionary<string, string> _fillAreaImage = new()
  {
    {"gameObjectName", "LoadingBarImage"},
    {"resourcesFolderPath", resourcesFolderPath}
  };
  private readonly Dictionary<string, Vector2> _areaRectTranform = new()
  {
    {"referenceObjectPixels", new Vector2(-1, -1)},
    {"archorMin", ScaleRadioMin},
    {"archorMax", ScaleRadioMax},
    {"pivotPos", new Vector2(0.5f, 0.5f)}, 
  };
  // text =============================================================================================
  private readonly TextGameObjectBuilder _textBuilder = null;
  private readonly Dictionary<string, Vector2> _textRectTranformArgu = new()
  {
    {"referenceObjectPixels", new Vector2(-1, -1)},
    {"archorMin", new Vector2(0.5f, 0.3f)},
    {"archorMax", new Vector2(0.5f, 0.3f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)}, 
  };
  private readonly Dictionary<string, bool> _textContentSizeFitterArgu = new()
  {
    {"verticalFit", true},
    {"horizontalFit", true},
  };
  #endregion


  #region Perproties
  public override string Type { get; } = "LoadingSlider";
  public override string GameObjectName { get { return _loadingBar.GameObjectName; } }
  public override Transform Transform { get { return _loadingBar.Transform; } }
  public override RectTransform RectTransform { get{ return _loadingBar.RectTransform; } }
  public float Value
  {
    get { return _loadingBar.Value; }
    set { _loadingBar.Value = value; }
  }
  public string Content
  {
    get { return _textBuilder.Content;}
    set { _textBuilder.Content = value;}
  }  
  #endregion


  #region Methods
  public LoadingSliderUniversalBuilder(string sliderName, Transform parentTransform)
  {
    _loadingBar = new SliderGameObjectBuilder(sliderName, parentTransform);
    _loadingBar.Build(
      new SortedDictionary<string, Dictionary<string, IComponentBuilder>>
      {
        {"SliderArea", new Dictionary<string, IComponentBuilder>
          { {"RectTransform", new RectTransformComponentBuilder(_sliderAreaRectTranform)} }
        },
        {"BackgroundImage", new Dictionary<string, IComponentBuilder>
          {
            {"Image", new ImageComponentBuilder(_backgroundImage)},
            {"RectTransform", new RectTransformComponentBuilder(_backgroundRectTranform)}
          }
        },
        {"FillImage", new Dictionary<string, IComponentBuilder>
          {
            {"Image", new ImageComponentBuilder(_fillAreaImage)},
            {"RectTransform", new RectTransformComponentBuilder(_fillingAreaRectTranform)}
          }
        },
        {
          "FillArea", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(_areaRectTranform)}
          }
        },
        {"HandleImage", null}
      }
    );

    _textBuilder = new TextGameObjectBuilder("Dialog", parentTransform);
    _textBuilder.Build(
      new Dictionary<string, IComponentBuilder> 
      {
        {"RectTransform", new RectTransformComponentBuilder(_textRectTranformArgu)},
        {"Text", new TextComponentBuilder(TextBoxUniversalBuilder.textArgu_01)},
        {"ContentSizeFitter", new ContentSizeFitterComponentBuilder(_textContentSizeFitterArgu)}
      }
    );
    _textBuilder.ModifyContent("jindu: " + Value.ToString());
  }

  public override void SetActive(bool state)
  {
    _textBuilder.SetActive(state);
    _loadingBar.SetActive(state);
  }

  public readonly static Vector2 referenceSliderPixel = new(1680, 70);
  public readonly static Vector2 ScaleRadioMin = new(0.03f, 0.1f);
  public readonly static Vector2 ScaleRadioMax = new(0.97f, 0.9f);
  #endregion
}