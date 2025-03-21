using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;


public class ScrollRectUniversalBuilder : UniversalGameObjectBuilder
{
  #region Fields
  public enum PosArgusType { ReferenceContentPixel, ReferenceScrollbarPixel, ContentPos, HandlePos };
  private readonly ScrollRectGameObjectBuilder _sliderBuilder;
  private readonly VerticalLayoutGroupGameObjectBuilder _contentBuilder;
  private readonly ImageGameObjectBuilder _slidingAreaUBuilder;
  private readonly ImageGameObjectBuilder _slidingAreaDBuilder;
  #endregion

  #region Properties
  public override string Type { get; } = "ScrollRectGameObjectBuilder";
  public override string GameObjectName { get { return _sliderBuilder.GameObjectName; } }
  public override Transform Transform { get { return _contentBuilder.Transform; } }
  public override RectTransform RectTransform { get{ return _sliderBuilder.RectTransform; } }
  public TextBoxUniversalBuilder TextBoxBuilder { get; }
  #endregion

  #region Methods
  public ScrollRectUniversalBuilder(
    Transform parentTransform,
    List<Vector2> posArgus)
  {
    if (posArgus.Count != 4)
    {
      Debug.LogError("ScrollRectUniversalBuilder: posArgus.Count != 4");
      return;
    }

    _sliderBuilder = new ScrollRectGameObjectBuilder(parentTransform);
    _sliderBuilder.Build(new SortedDictionary<string, Dictionary<string, IComponentBuilder>>
    {
      {"MaskImage", new Dictionary<string, IComponentBuilder>
        {
          {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
            {
              {"referenceObjectPixels", posArgus[(int) PosArgusType.ReferenceContentPixel]},
              {"archorMin", posArgus[(int) PosArgusType.ContentPos]},
              {"archorMax", posArgus[(int) PosArgusType.ContentPos]},
              {"pivotPos", new Vector2(0.5f, 0.5f)},
            }
          )},
          {"Image", new ImageComponentBuilder(new Dictionary<string, string>
            {
              {"gameObjectName", null},
              {"resourcesFolderPath", null},
            })
          }
        }
      },
      {"ScrollBar", new Dictionary<string, IComponentBuilder>
        {
          {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
            {
              {"referenceObjectPixels", posArgus[(int) PosArgusType.ReferenceScrollbarPixel]},
              {"archorMin", posArgus[(int) PosArgusType.HandlePos]},
              {"archorMax", posArgus[(int) PosArgusType.HandlePos]},
              {"pivotPos", new Vector2(0.5f, 0.5f)},
            })
          }
        }
      },
      {"SlidingArea", new Dictionary<string, IComponentBuilder>
        {
          {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
            {
              {"referenceObjectPixels", posArgus[(int) PosArgusType.ReferenceScrollbarPixel]},
              {"archorMin", new Vector2(0.5f, 0.5f)},
              {"archorMax", new Vector2(0.5f, 0.5f)},
              {"pivotPos", new Vector2(0.5f, 0.5f)},
            })
          }
        }
      },
      {"HandleImage", new Dictionary<string, IComponentBuilder>
        {
          {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
            {
              {"referenceObjectPixels", new Vector2(-1, -1)},
              {"archorMin", Vector2.zero},
              {"archorMax", Vector2.one},
              {"pivotPos", new Vector2(0.5f, 0.5f)},
            })
          },
        }
      },
    });

    // 竖直排列
    _contentBuilder = new VerticalLayoutGroupGameObjectBuilder("VerticalLayoutGroup", _sliderBuilder.MaskBuilder.Transform);
    _contentBuilder.Build(new Dictionary<string, IComponentBuilder>
    {
      {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
        {
          {"referenceObjectPixels", posArgus[(int) PosArgusType.ReferenceContentPixel]},
          {"archorMin", new Vector2(0.5f, 0.5f)},
          {"archorMax", new Vector2(0.5f, 0.5f)},
          {"pivotPos", new Vector2(0.5f, 0.5f)},
        }
      )},
    });

    _contentBuilder.Spacing = 1.0f;

    // 上下的装饰
    float referenceX = posArgus[(int) PosArgusType.ReferenceScrollbarPixel].x * 0.85f;
    Vector2 referenceTPixel = new Vector2(referenceX, referenceX * 1.5f);
    _slidingAreaUBuilder = new ImageGameObjectBuilder("SlidingAreaU", _sliderBuilder.HandleBuilder.Transform);
    _slidingAreaUBuilder.Build(new Dictionary<string, IComponentBuilder>
    {
      {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
        {
          {"referenceObjectPixels", referenceTPixel},  
          {"archorMin", new Vector2(0.5f, 0.0f)},
          {"archorMax", new Vector2(0.5f, 0.0f)},
          {"pivotPos", new Vector2(0.5f, 1.0f)},
        })
      },
      {"Image", new ImageComponentBuilder(new Dictionary<string, string>
      {
        {"gameObjectName", "SlidingAreaUImage"},
        {"resourcesFolderPath", resourcesFolderPath},
      })}
    });

    _slidingAreaDBuilder = new ImageGameObjectBuilder("SlidingAreaU", _sliderBuilder.HandleBuilder.Transform);
    _slidingAreaDBuilder.Build(new Dictionary<string, IComponentBuilder>
    {
      {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
        {
          {"referenceObjectPixels", referenceTPixel},  
          {"archorMin", new Vector2(0.5f, 1.0f)},
          {"archorMax", new Vector2(0.5f, 1.0f)},
          {"pivotPos", new Vector2(0.5f, 1.0f)},
        })
      },
      {"Image", new ImageComponentBuilder(new Dictionary<string, string>
      {
        {"gameObjectName", "SlidingAreaDImage"},
        {"resourcesFolderPath", resourcesFolderPath},
      })}
    }); 
  }

  public void SetContent()
  {
    _sliderBuilder.SetContent(_contentBuilder.RectTransform, resourcesFolderPath + "HandleImage");

    _sliderBuilder.MaskBuilder.ModifyColor(53f / 255f, 24f / 255f, 5f / 255f, 0.5f);
    _sliderBuilder.InitPos = 1.0f;
  }

  public override void SetActive(bool state)
  {
    _sliderBuilder.SetActive(state);
    _contentBuilder.SetActive(state);
  }

  #endregion 
}