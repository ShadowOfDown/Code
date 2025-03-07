// 开始场景
using UnityEngine;
using System.Collections.Generic;

public class StartScene : UIObject
{
    #region Fields 
    private readonly List<List<bool>> _interfaceList = new()
  {
    new() { true, false, false },
    new() { false, true, false },
    new() { false, false, true },
  };
    public static int interfaceIdx = 0;
    public static readonly string resourcesFolderPath = "Arts/Textures/StartScene/";
    private readonly Dictionary<string, Vector2> _fullScreenRectTransformArgu = new()
  {
    {"referenceObjectPixels", PixelInfo.referenceScreenPixel},
    {"archorMin", new Vector2(0.5f, 0.5f)},
    {"archorMax", new Vector2(0.5f, 0.5f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
    private CanvasGameObjectBuilder _canvas = null;
    private ImageGameObjectBuilder _backgroundImage = null;
    private readonly Dictionary<string, string> _backgroundImageArgu = new()
  {
    {"gameObjectName", "BackgroundImage"},
    {"resourcesFolderPath", resourcesFolderPath},
  };
    private LoadingSliderUniversalBuilder _loadingBar = null;
    private StartInterface _startInterface = null;
    private WeiXinLoginInterface _weiXinLoginInterface = null;

    #endregion

    #region Properties
    #endregion


    #region Methods

    public override void OnLoad()
    {
        _backgroundImage = new ImageGameObjectBuilder("BackgroundImage", this.transform);
        _backgroundImage.Build(
          new Dictionary<string, IComponentBuilder>
          {
        {"RectTransform", new RectTransformComponentBuilder(_fullScreenRectTransformArgu)},
        {"Image", new ImageComponentBuilder(_backgroundImageArgu)}
          }
        );

        // 加载场景
        _loadingBar = new LoadingSliderUniversalBuilder("LoadingBar", _backgroundImage.Transform);

        // 按钮
        _startInterface = new StartInterface(_backgroundImage.Transform);
        _startInterface.SetActive(false);

        // 微信登录界面
        _weiXinLoginInterface = new WeiXinLoginInterface(_backgroundImage.Transform);
        _weiXinLoginInterface.SetActive(false);

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
        _weiXinLoginInterface.SetActive(_interfaceList[interfaceIdx][2]);
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
    #endregion
}
