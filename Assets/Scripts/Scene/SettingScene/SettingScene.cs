// 这个设置没办法做

using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class SettingScene : MonoBehaviour
{
  #region Fields;
  private readonly Dictionary<string, Vector2> _fullScreenRectTransformArgu = new()
  {
    {"referenceObjectPixels", PixelInfo.referenceScreenPixel},
    {"archorMin", new Vector2(0.5f, 0.5f)},
    {"archorMax", new Vector2(0.5f, 0.5f)},
    {"objectPos", new Vector2(0.5f, 0.5f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  private CanvasGameObjectBuilder _canvas = null;
  private ImageGameObjectBuilder _backgroundImage = null;
  private readonly Dictionary<string, string> _backgroundImageArgu = new()
  {
    {"gameObjectName", "BackgroundImage"},
    {"resourcesFolderPath", resourcesFolderPath},
  };
  public static readonly string resourcesFolderPath = "Arts/Textures/SettingScene/";
  private ImageGameObjectBuilder _settingBackground;
  private VolumnInterface _volumnInterface; // 到时候应该会做成一个 List 来控制
  private ButtonUniversalbuilder _exitButton;
  private ButtonUniversalbuilder _volumnButton;
  private ButtonUniversalbuilder _imageButton;
  #endregion

  #region Properties

  #endregion

  #region Methods
  public void Start()
  {
    // Canvas =========================================================================================
    _canvas = new CanvasGameObjectBuilder();
    _canvas.Build(
      new Dictionary<string, IComponentBuilder>
      {
        {"RectTransform", new RectTransformComponentBuilder(_fullScreenRectTransformArgu)},
      }
    );

    // Background =====================================================================================
    _backgroundImage = new ImageGameObjectBuilder("BackgroundImage", _canvas.Transform);
    _backgroundImage.Build(
      new Dictionary<string, IComponentBuilder>
      {
        {"RectTransform", new RectTransformComponentBuilder(_fullScreenRectTransformArgu)},
        {"Image", new ImageComponentBuilder(_backgroundImageArgu)}
      }
    );

    // SettingBackground ==============================================================================
    _settingBackground = new ImageGameObjectBuilder("SettingBackground", _backgroundImage.Transform);
    _settingBackground.Build(
      new Dictionary<string, IComponentBuilder>
      {
        {"RectTransform", new RectTransformComponentBuilder(
          new Dictionary<string, Vector2>
          {
            {"referenceObjectPixels", new Vector2(1860, 840)},
            {"archorMin", new Vector2(0.5f, 0.45f)},
            {"archorMax", new Vector2(0.5f, 0.45f)},
            {"pivotPos", new Vector2(0.5f, 0.5f)},
          }
        )},
        {"Image", new ImageComponentBuilder(
          new Dictionary<string, string>
          {
            {"gameObjectName", "SettingBackgroundImage"},
            {"resourcesFolderPath", resourcesFolderPath}
          })
        }
      }
    );
    _volumnInterface = new VolumnInterface(_settingBackground.Transform);

    // ExitButton =====================================================================================
    _exitButton = new ButtonUniversalbuilder("StartButton", new Vector2(0.9f, 0.9f), _backgroundImage.Transform, new Dictionary<string, UnityAction>
    {
      {"onClickAction", OnExitButtonClick},
    }, -1, 1);

    _volumnButton = new ButtonUniversalbuilder("StartButton", new Vector2(0.6f, 0.9f), _backgroundImage.Transform, new Dictionary<string, UnityAction>
    {
      {"onClickAction", OnVolumnButtonClick},
    }, -1, 1);

    _imageButton = new ButtonUniversalbuilder("StartButton", new Vector2(0.4f, 0.9f), _backgroundImage.Transform, new Dictionary<string, UnityAction>
    {
      {"onClickAction", OnImageButtonClick},
    }, -1, 1);
  }

  public void Update()
  {
    // 因为还没有写其他的 interface(界面), 所以就没有做
  }

  public void SetActive(bool state)
  {
    // 因为还没有写其他的 interface(界面), 所以就没有做
  }

  public void OnExitButtonClick()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log("exit button clicked");
    }
  }

  public void OnVolumnButtonClick()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log("exit button clicked");
    }
  }

  public void OnImageButtonClick()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log("exit button clicked");
    }
  }
  #endregion
}
