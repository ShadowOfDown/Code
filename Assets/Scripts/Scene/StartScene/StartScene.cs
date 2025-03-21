// 开始场景
using UnityEngine;
using System.Collections.Generic;

public class StartScene : UIObject
{
  #region Fields 
  // 加载条, 开始界面, 微信界面
  private readonly List<List<bool>> _interfaceList = new()
  {
    new() { true, false, false },
    new() { false, true, false },
  };
  public static int interfaceIdx = 0;
  public static readonly string resourcesFolderPath = "Arts/Textures/StartScene/";

  private ImageGameObjectBuilder _backgroundImage = null;
  private LoadingSliderUniversalBuilder _loadingBar = null;
  private StartInterface _startInterface = null;

  #endregion

  #region Methods
  public override void OnLoad()
  {
    BuildBackgroundImage();
    BuildLoadingBar();
    BuildStartInterface();
  }

  public void Init(float value)
  {
    SetValue(value);
  }

  public override void OnUpdate()
  {
    // TODO: 更新加载进度
    SetValue(_loadingBar.Value + 0.01f);
    SetActive();
  }

  public void SetActive()
  {
    _loadingBar.SetActive(_interfaceList[interfaceIdx][0]);
    _startInterface.SetActive(_interfaceList[interfaceIdx][1]);
  }

  // 设置加载进度
  public void SetValue(float value)
  {
    if (_loadingBar.Value < 1)
    {
      _loadingBar.Value = value;
      if (_loadingBar.Value >= 1)
      {
        DebugInfo.Print("move to start interface");
        interfaceIdx++;
      }
    }
  }

  public void OnCloseButtonClicked()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log("close button clicked");
    }
    interfaceIdx--;
  }

  public override void OnClose()
  {
    interfaceIdx--;
  }

  private void BuildBackgroundImage()
  {
    _backgroundImage = new ImageGameObjectBuilder("BackgroundImage", transform);
    _backgroundImage.Build(
      new Dictionary<string, IComponentBuilder>
      {
        {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
          {
            {"referenceObjectPixels", PixelInfo.referenceScreenPixel},
            {"archorMin", new Vector2(0.5f, 0.5f)},
            {"archorMax", new Vector2(0.5f, 0.5f)},
            {"pivotPos", new Vector2(0.5f, 0.5f)},
          })},
        {"Image", new ImageComponentBuilder(new Dictionary<string, string>
          {
            {"gameObjectName", "BackgroundImage"},
            {"resourcesFolderPath", resourcesFolderPath},
          })}
      }
    );
  }

  private void BuildLoadingBar()
  {
    _loadingBar = new LoadingSliderUniversalBuilder("LoadingBar", _backgroundImage.Transform);
  }

  private void BuildStartInterface()
  {
    _startInterface = new StartInterface(_backgroundImage.Transform);
    _startInterface.SetActive(false);
  }
  #endregion
}
