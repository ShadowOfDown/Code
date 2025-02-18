using UnityEngine;
using UnityEngine.UI;
using System;

public class CanvasLoader
{
    #region Field
    private readonly Canvas _canvas = null;
    private readonly CanvasScaler _canvasScaler = null;
    private readonly GraphicRaycaster _graphicRaycaster = null;
    #endregion

    #region Property
    public Canvas Canvas => _canvas;
    public CanvasScaler CanvasScaler => _canvasScaler;
    public GraphicRaycaster GraphicRaycaster => _graphicRaycaster;
    #endregion

    #region Method
    public CanvasLoader(string objectName, Vector2 referenceResolution)
    {
        GameObject gameObject = new GameObject(objectName);
        _canvas = gameObject.AddComponent<Canvas>();
        _canvasScaler = gameObject.AddComponent<CanvasScaler>();
        _canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        _graphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();

        // 设置Canvas的渲染模式为Screen Space - Overlay
        _canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // 设置参考分辨率
        _canvasScaler.referenceResolution = referenceResolution;

        // 计算屏幕宽高比和参考分辨率宽高比
        float aspectRatio = Math.Min(Screen.width / referenceResolution.x, Screen.height / referenceResolution.y);
        RectTransform rectTransform = _canvasScaler.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(referenceResolution.x * aspectRatio, referenceResolution.y * aspectRatio);
    }
    #endregion
}