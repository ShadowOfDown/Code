using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.UIElements;

<<<<<<< HEAD
public class DramaPerformScene : ISceneState
{
  #region Fields
  private string my_state_name = "DramaPerformScene";
=======
public class DramaPerformScene : MonoBehaviour
{
  #region Fields
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
  public static readonly string resourcesFolderPath = "Arts/Textures/DramaPerformScene/";
  private const int _nameCardNum = 0;
  private const int _characterImageNum = 1;
  private const int _textBoxNum_02 = 2;
  private bool _isDramaPerform = true;
  private readonly List<List<bool>> _screenTypes = new()
  {
    new List<bool> {false, false, false},
    new List<bool> {true, true, false},
    new List<bool> {true, true, true},
    new List<bool> {false, true, true},
  };
  private readonly List<int> _screenArray = new()
  {
    2, 3, 1
  };
  private readonly Dictionary<string, Vector2> _fullScreenRectTransformArgu = new()
  {
    {"referenceObjectPixels", PixelInfo.referenceScreenPixel},
    {"archorMin", new Vector2(0.5f, 0.5f)},
    {"archorMax", new Vector2(0.5f, 0.5f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  private int _screenIndex = 0;


  #region Canvas
  private CanvasGameObjectBuilder _canvas = null;
  #endregion


  #region Background
  private ImageGameObjectBuilder _backgroundImage = null;
  private readonly Dictionary<string, string> _backgroundImageArgu = new()
  {
    {"gameObjectName", "BackgroundImage"},
    {"resourcesFolderPath", resourcesFolderPath},
  };
  #endregion Background


  #region CharacterImage
  private ImageGameObjectBuilder _characterImage = null;
  private readonly Dictionary<string, Vector2> _characterRectTransformArgu = new()
  {
    {"referenceObjectPixels", new Vector2(640, 1080)},
    {"archorMin", new Vector2(0.5f, 0.5f)},
    {"archorMax", new Vector2(0.5f, 0.5f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  private readonly Dictionary<string, string> _characterImageArgu = new()
  {
    {"gameObjectName", "CharacterImage"},
    {"resourcesFolderPath", resourcesFolderPath},
  };
  #endregion


  #region TextBox_01
  private TextBoxGameObjectBuilder _textBox_01 = null;
  private readonly Dictionary<string, string> _textBoxNameArgu_01 = new()
  {
    {"Image", "TextBoxImage_01"},
    {"Text", "TextBoxText_01"},
  };
  private readonly Dictionary<string, Vector2> _textBoxImageRectTransformArgu_01 = new()
  {
    {"referenceObjectPixels", new Vector2(1920, 360)},
    {"archorMin", new Vector2(0.5f, 0.25f)},
    {"archorMax", new Vector2(0.5f, 0.25f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  private readonly Dictionary<string, Vector2> _textBoxTextRectTransformArgu_01 = new()
  {
    {"referenceObjectPixels", new Vector2(1920 * 0.9f, 270 * 0.9f)},
    {"archorMin", new Vector2(0.5f, 0.5f)},
    {"archorMax", new Vector2(0.5f, 0.5f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  private readonly Dictionary<string, string> _textBoxImageArgu_01 = new()
  {
    {"gameObjectName", "TextBoxImage_01"},
    {"resourcesFolderPath", resourcesFolderPath},
  };
  private readonly Dictionary<string, List<float>> _textBoxTextArgu_01 = new()
  {
    {"fontSize", new List<float>{30f}},
    {"color", new List<float>{0.0f, 0.0f, 0.0f, 1.0f}},
    {"alignmentOption", TextComponentBuilder.left},
    {"font", TextComponentBuilder.kai},
  };
  private readonly Dictionary<string, bool> _textBoxContentSizeFitterArgu_01 = new()
  {
    {"horizontalFit", false},
    {"verticalFit", false},
  };
  private string _textBoxTextContent_01 = "hello world";
  #endregion


  #region NameCard
  private TextBoxGameObjectBuilder _nameCard = null;
  private readonly Dictionary<string, string> _nameCardNameArgu = new()
  {
    {"Image", "NameCardImage"},
    {"Text", "NameCardText"},
  };
  private readonly Dictionary<string, Vector2> _nameCardImageRectTransformArgu = new()
  {
    {"referenceObjectPixels", new Vector2(480, 120)},
    {"archorMin", new Vector2(0.2f, 1f)},
    {"archorMax", new Vector2(0.2f, 1f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  private readonly Dictionary<string, Vector2> _nameCardTextRectTransformArgu = new()
  {
    {"referenceObjectPixels", new Vector2(480 * 0.9f, 120 * 0.9f)},
    {"archorMin", new Vector2(0.5f, 0.5f)},
    {"archorMax", new Vector2(0.5f, 0.5f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  private readonly Dictionary<string, string> _nameCardImageArgu = new()
  {
    {"gameObjectName", "NameCardImage"},
    {"resourcesFolderPath", resourcesFolderPath},
  };
  private readonly Dictionary<string, List<float>> _nameCardTextArgu = new()
  {
    {"fontSize", new List<float>{30f}},
    {"color", new List<float>{0.0f, 0.0f, 0.0f, 1.0f}},
    {"alignmentOption", TextComponentBuilder.middle},
    {"font", TextComponentBuilder.kai},
  };
  private readonly Dictionary<string, bool> _nameCardContentSizeFitterArgu = new()
  {
    {"horizontalFit", false},
    {"verticalFit", false},
  };
  private string _nameCardTextContent = "hello world";
  #endregion


  #region TextBox_02
  private ButtonTextGameObjectBuilder _textBox_02 = null;
  private readonly Dictionary<string, string> _textBoxNameArgu_02 = new()
  {
    {"Button", "TextBoxImage_02"},
    {"Text", "TextBoxText_02"},
  };
  private readonly Dictionary<string, Vector2> _textBoxImageRectTransformArgu_02 = new()
  {
    {"referenceObjectPixels", new Vector2(1280, 120)},
    {"archorMin", new Vector2(0.5f, 0.75f)},
    {"archorMax", new Vector2(0.5f, 0.75f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  private readonly Dictionary<string, Vector2> _textBoxTextRectTransformArgu_02 = new()
  {
    {"referenceObjectPixels", new Vector2(1280 * 0.9f, 120 * 0.9f)},
    {"archorMin", new Vector2(0.5f, 0.618f)},
    {"archorMax", new Vector2(0.5f, 0.618f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  private readonly Dictionary<string, string> _normalTextBoxImageArgu_02 = new()
  {
    {"gameObjectName", "NormalTextBoxImage_02"},
    {"resourcesFolderPath", resourcesFolderPath},
  };
  private readonly Dictionary<string, string> _pressedTextBoxImageArgu_02 = new()
  {
    {"gameObjectName", "PressedTextBoxImage_02"},
    {"resourcesFolderPath", resourcesFolderPath},
  };
  private readonly Dictionary<string, List<float>> _textBoxTextArgu_02 = new()
  {
    {"fontSize", new List<float>{30f}},
    {"color", new List<float>{0.0f, 0.0f, 0.0f, 1.0f}},
    {"alignmentOption", TextComponentBuilder.middle},
    {"font", TextComponentBuilder.kai},
  };
  private readonly Dictionary<string, bool> _textBoxContentSizeFitterArgu_02 = new()
  {
    {"horizontalFit", false},
    {"verticalFit", false},
  };
  private string _textBoxTextContent_02 = "hello world";
  #endregion


  #region ReviewButton
<<<<<<< HEAD
  private ButtonBoxGameObjectbuilder _reviewButton = null;
=======
  private ButtonUniversalbuilder _reviewButton = null;
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
  private Vector2 _reviewVector = new(0.1f, 0.9f);
  # endregion


  #region ReturnButton
<<<<<<< HEAD
  private ButtonBoxGameObjectbuilder _returnButton = null;
=======
  private ButtonUniversalbuilder _returnButton = null;
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
  private Vector2 _returnVector = new(0.95f, 0.9f);
  #endregion


  #region ScrollRect
  private ScrollRectGameObjectBuilder _scrollRectGameObejctBuilder = null;
  private readonly Dictionary<string, string> _maskImageArgu = new()
  {
    {"gameObjectName", "MaskImage"},
    {"resourcesFolderPath", resourcesFolderPath},
  };
  private readonly Dictionary<string, Vector2> _maskRectTransformArgu = new()
  {
    {"referenceObjectPixels", new Vector2(1680, 960)},
    {"archorMin", new Vector2(0.45f, 0.53f)},
    {"archorMax", new Vector2(0.45f, 0.53f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  private readonly Dictionary<string, Vector2> _scrollBarRectTransformArgu = new()
  {
    {"referenceObjectPixels", new Vector2(40, 840)},
    {"archorMin", new Vector2(0.95f, 0.42f)},
    {"archorMax", new Vector2(0.95f, 0.42f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  private readonly Dictionary<string, Vector2> _slidingAreaRectTransformArgu = new()
  {
    {"referenceObjectPixels", new Vector2(40, 840)},
    {"archorMin", new Vector2(0.5f, 0.5f)},
    {"archorMax", new Vector2(0.5f, 0.5f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  private readonly Dictionary<string, Vector2> _handleRectTransformArgu = new()
  {
    {"referenceObjectPixels", new Vector2(-1, -1)},
    {"archorMin", new Vector2(0.5f, 0.5f)},
    {"archorMax", new Vector2(0.5f, 0.5f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  ColorBlock _scrollbarColorBlock = new()
  {
    normalColor = new Color(1.0f, 1.0f, 1.0f, 0.5f),
    highlightedColor = new Color(1.0f, 1.0f, 1.0f, 0.65f),
    pressedColor = new Color(1.0f, 1.0f, 1.0f, 0.8f),
    selectedColor = new Color(1.0f, 1.0f, 1.0f, 0.5f),
    disabledColor = new Color(1.0f, 1.0f, 1.0f, 0.3f),
    colorMultiplier = 1.0f,
    fadeDuration = 0.1f,
  };
  Color _handleColor = new(1.0f, 1.0f, 1.0f, 0.5f);
  #endregion


  #region VerticalLayoutGroup
  private VerticalLayoutGroupGameObjectBuilder _verticalLayoutGroupGameObjectBuilder = null;
  private readonly Dictionary<string, Vector2> _verticalLayoutGroupRectTransformArgu = new()
  {
    {"referenceObjectPixels", new Vector2(1680, -1)},
    {"archorMin", new Vector2(0.5f, 0.5f)},
    {"archorMax", new Vector2(0.5f, 0.5f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  #endregion


  #region ReviewText
  private TextGameObjectBuilder _reviewText = null;
  private readonly Dictionary<string, Vector2> _reviewTextRectTransformArgu = new()
  {
    {"referenceObjectPixels", new Vector2(1680 * 0.9f, -1)},
    {"archorMin", new Vector2(0.5f, 0.5f)},
    {"archorMax", new Vector2(0.5f, 0.5f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  private readonly Dictionary<string, List<float>> _reviewTextArgu = new()
  {
    {"fontSize", new List<float>{30f}},
    {"color", new List<float>{0.0f, 0.0f, 0.0f, 1.0f}},
    {"alignmentOption", TextComponentBuilder.left},
    {"font", TextComponentBuilder.kai},
  };
  private readonly Dictionary<string, bool> _reviewTextContentSizeFitterArgu = new()
  {
    {"horizontalFit", false},
    {"verticalFit", true},
  };
  private string _reviewTextContent = string.Concat(Enumerable.Repeat("hello world\n", 100));
  #endregion


  #region EndBar
  private ImageGameObjectBuilder _endBarImage = null;
  private readonly Dictionary<string, Vector2> _endBarImageRectTransformArgu = new()
  {
    {"referenceObjectPixels", new Vector2(1680, 60)},
    {"archorMin", new Vector2(0.5f, 0.5f)},
    {"archorMax", new Vector2(0.5f, 0.5f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  private readonly Dictionary<string, string> _endBarImageArgu = new()
  {
    {"gameObjectName", "EndBarImage"},
    {"resourcesFolderPath", resourcesFolderPath},
  };
  #endregion
  #endregion

  #region Methods
<<<<<<< HEAD
  public DramaPerformScene(SceneStateControl control) : base(control)
  {
    this.StateName = "DramaPerformState";
  }

  public override void StateBegin()
  {
    UI_Manager.Instance.Init();
=======
  public void Start()
  {
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
    Debug.Log($"Screensize: {PixelInfo.screenPixel}, scaleRadio: {PixelInfo.scaleRadio}");

    // canvas =========================================================================================
    _canvas = new CanvasGameObjectBuilder();
    _canvas.Build(
      new Dictionary<string, IComponentBuilder>
      {
        {"RectTransform", new RectTransformComponentBuilder(_fullScreenRectTransformArgu)},
      }
    );
    // backgroundImage ================================================================================
    _backgroundImage = new ImageGameObjectBuilder("BackgroundImage", _canvas.Transform);
    _backgroundImage.Build(
      new Dictionary<string, IComponentBuilder>
      {
        {"RectTransform", new RectTransformComponentBuilder(_fullScreenRectTransformArgu)},
        {"Image", new ImageComponentBuilder(_backgroundImageArgu)}
      }
    );

    // characterImage =================================================================================
    _characterImage = new ImageGameObjectBuilder("CharacterImage", _backgroundImage.Transform);
    _characterImage.Build(
      new Dictionary<string, IComponentBuilder> 
      {
        {"RectTransform", new RectTransformComponentBuilder(_characterRectTransformArgu)},
        {"Image", new ImageComponentBuilder(_characterImageArgu)}
      }
    );

    // textBox_01 =====================================================================================
    _textBox_01 = new TextBoxGameObjectBuilder(_textBoxNameArgu_01, _backgroundImage.Transform);
    _textBox_01.Build(
      new SortedDictionary<string, Dictionary<string, IComponentBuilder>>
      {
        {"Image", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(_textBoxImageRectTransformArgu_01)},
            {"Image", new ImageComponentBuilder(_textBoxImageArgu_01)},
          }
        },
        {"Text", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(_textBoxTextRectTransformArgu_01)},
            {"Text", new TextComponentBuilder(_textBoxTextArgu_01)},
            {"ContentSizeFitter", new ContentSizeFitterComponentBuilder(_textBoxContentSizeFitterArgu_01)},
          }
        }
      }
    );
    _textBox_01.ModifyContent(_textBoxTextContent_01);

    // nameCard =====================================================================================
    _nameCard = new TextBoxGameObjectBuilder(_nameCardNameArgu, _textBox_01.Transform);
    _nameCard.Build(
      new SortedDictionary<string, Dictionary<string, IComponentBuilder>>
      {
        {"Image", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(_nameCardImageRectTransformArgu)},
            {"Image", new ImageComponentBuilder(_nameCardImageArgu)},
          }
        },
        {"Text", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(_nameCardTextRectTransformArgu)},
            {"Text", new TextComponentBuilder(_nameCardTextArgu)},
            {"ContentSizeFitter", new ContentSizeFitterComponentBuilder(_nameCardContentSizeFitterArgu)},
          }
        },
      }
    );
    _nameCard.ModifyContent(_nameCardTextContent);

    // textBox_02 =====================================================================================
    _textBox_02 = new ButtonTextGameObjectBuilder(_textBoxNameArgu_02, _backgroundImage.Transform);
    _textBox_02.Build(
      new SortedDictionary<string, Dictionary<string, IComponentBuilder>>
      {
        {"Button", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(_textBoxImageRectTransformArgu_02)},
            {"Image" , new ImageComponentBuilder(_normalTextBoxImageArgu_02)},
            {"Button", new ButtonComponentBuilder(_pressedTextBoxImageArgu_02)},
            {"EventTrigger", new EventTriggerComponentBuilder(new Dictionary<string, UnityAction>
              {
                {"onClickAction", OnTextBoxButtonClick_02},
                {"onHoverAction", OnTextBoxLongPressed_02}
              }
            )}
          }
        },
        {"Text", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform" , new RectTransformComponentBuilder(_textBoxTextRectTransformArgu_02)},
            {"Text", new TextComponentBuilder(_textBoxTextArgu_02)},
            {"ContentSizeFitter", new ContentSizeFitterComponentBuilder(_textBoxContentSizeFitterArgu_02)},
          }
        }
      }
    );
    _textBox_02.ModifyContent(_textBoxTextContent_02);

    // reviewButton ===================================================================================
<<<<<<< HEAD
    _reviewButton = new ButtonBoxGameObjectbuilder(
=======
    _reviewButton = new ButtonUniversalbuilder(
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
      "ReviewButton", _reviewVector, _backgroundImage.Transform, new Dictionary<string, UnityAction>
        {
          {"onClickAction", OnReviewButtonClick},
          {"OnHoverAction", null}
        }
      );

    // returnbutton ======================================================================================
<<<<<<< HEAD
    _returnButton = new ButtonBoxGameObjectbuilder(
=======
    _returnButton = new ButtonUniversalbuilder(
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
      "ReturnButton", _returnVector, _backgroundImage.Transform, new Dictionary<string, UnityAction>
        {
          {"onClickAction", OnReturnButtonClick},
          {"OnHoverAction", null}
        }, 1
      );

    // scrollRect ====================================================================================
    _scrollRectGameObejctBuilder = new ScrollRectGameObjectBuilder(_backgroundImage.Transform);
    _scrollRectGameObejctBuilder.Build(
      new SortedDictionary<string, Dictionary<string, IComponentBuilder>>
      {
        {"MaskImage", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(_maskRectTransformArgu)},
            {"Image", new ImageComponentBuilder(_maskImageArgu)},
          }
        },
        {"ScrollBar", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(_scrollBarRectTransformArgu)},
          }
        },
        {"SlidingArea", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(_slidingAreaRectTransformArgu)}
          }
        },
        {"HandleImage", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(_handleRectTransformArgu)},
          }
        }
      }
    );

    // verticalLayoutGroup ============================================================================
    _verticalLayoutGroupGameObjectBuilder = new VerticalLayoutGroupGameObjectBuilder("VerticalLayoutGroup", _scrollRectGameObejctBuilder.Transform);
    _verticalLayoutGroupGameObjectBuilder.Build(new Dictionary<string, IComponentBuilder>
    {
      {"RectTransform", new RectTransformComponentBuilder(_verticalLayoutGroupRectTransformArgu)},
    });

    // review Box =====================================================================================
    _reviewText = new TextGameObjectBuilder("reviewText", _verticalLayoutGroupGameObjectBuilder.Transform);
    _reviewText.Build(new Dictionary<string, IComponentBuilder>
    {
      {"RectTransform", new RectTransformComponentBuilder(_reviewTextRectTransformArgu)},
      {"Text", new TextComponentBuilder(_reviewTextArgu)},
      {"ContentSizeFitter", new ContentSizeFitterComponentBuilder(_reviewTextContentSizeFitterArgu)},
    });
    _reviewText.ModifyContent(_reviewTextContent);

    // endBar =========================================================================================
    _endBarImage = new ImageGameObjectBuilder("endBarImage", _verticalLayoutGroupGameObjectBuilder.Transform);
    _endBarImage.Build(new Dictionary<string, IComponentBuilder>
    {
      {"RectTransform", new RectTransformComponentBuilder(_endBarImageRectTransformArgu)},
      {"Image", new ImageComponentBuilder(_endBarImageArgu)},
    });

    _verticalLayoutGroupGameObjectBuilder.AddChild(new List<ISingleGameObjectBuilder> { _reviewText, _endBarImage });
    _scrollRectGameObejctBuilder.SetContent(_verticalLayoutGroupGameObjectBuilder.RectTransform, _scrollbarColorBlock, _handleColor);
  }

<<<<<<< HEAD
  public override void StateUpdate()
=======
  public void Update()
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
  {
    SetActive();
  }

  private void SetActive()
  {
    // dramaPerform ==================================================================================
    _backgroundImage.ModifyColor(_isDramaPerform ? Color.white : Color.gray);
    _characterImage.SetActive(_isDramaPerform & _screenTypes[_screenArray[_screenIndex]][_nameCardNum]);
    _textBox_01.SetActive(_isDramaPerform);
    _nameCard.SetActive(_isDramaPerform & _screenTypes[_screenArray[_screenIndex]][_nameCardNum]);
    _textBox_02.SetActive(_isDramaPerform & _screenTypes[_screenArray[_screenIndex]][_textBoxNum_02]);
    _reviewButton.SetActive(_isDramaPerform);

    // reviewDrama ====================================================================================
    _returnButton.SetActive(!_isDramaPerform);
    _scrollRectGameObejctBuilder.SetActive(!_isDramaPerform);
    _verticalLayoutGroupGameObjectBuilder.SetActive(!_isDramaPerform);
    _reviewText.SetActive(!_isDramaPerform);
    _endBarImage.SetActive(!_isDramaPerform);
  }

<<<<<<< HEAD
  public override void StateEnd()
  {
    // no
  }

  public override string ToString()
  {
    return string.Format("[I_SceneState : StateName = {0}]", StateName);
  }

=======
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
  private void OnTextBoxButtonClick_02()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log("textbox_02 Clicked");
    }
  }

  private void OnTextBoxLongPressed_02()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log("textbox_02 long pressed");
    }
  }

  private void OnReviewButtonClick()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log("Review Button Clicked");
    }

    _isDramaPerform = false;
  }

  private void OnReviewButtonLongPressed()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log("Review Button long pressed");
    }
  }

  private void OnReturnButtonClick()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log("Review Button Clicked");
    }

    _isDramaPerform = true;
  }

  private void OnReturnButtonLongPressed()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log("Review Button long pressed");
    }
  }

  #endregion
}