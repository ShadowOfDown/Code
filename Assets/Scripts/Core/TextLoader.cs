using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextLoader : GameObjectLoader
{
    #region field
    private TextMeshProUGUI _text; // 用于存储 Text 组件的引用
    private float _textRadio;   // 调整占据文本框的大小

    #endregion

    #region property
    public TextMeshProUGUI Text
    {
        get { return _text; }
        set { _text = value; }
    }
    public float TextRadio
    {
        get { return _textRadio; }
        set
        {
            Vector2 oriSizeDelta = _rectTransform.sizeDelta;
            _rectTransform.sizeDelta = new Vector2(oriSizeDelta.x / _textRadio * value, oriSizeDelta.x / _textRadio * value);
            _textRadio = value;
        }
    }
    #endregion

    #region method
    // 构造函数
    public TextLoader(
        string objectName, Transform parentTransform, Vector2 referencePixel,
        string textContent, int fontSize, Color textColor, TextAlignmentOptions textAlignmentOptions, TMP_FontAsset font, bool isDynamicGrawing = false,
        float scaleRatio = 1, bool isHeightBench = true, Vector2? objectPos = null, Vector2? pivotPos = null, float textRadio = 1) :
        base(objectName, parentTransform, referencePixel, scaleRatio * textRadio, isHeightBench, objectPos, pivotPos)
    {
        if (_gameObject == null)
        {
            Debug.LogError($"fail to initailize GameObject {_objectName}");
            return;
        }

        _textRadio = textRadio;
        _text = SetText(_gameObject, textContent, fontSize, textColor, textAlignmentOptions, font, isDynamicGrawing);

        Debug.Log($"successfully load {_objectName}");
    }
    
    public static TextMeshProUGUI SetText(
        GameObject gameObject, string textContent, int fontSize, Color textColor, TextAlignmentOptions textAlignmentOptions, TMP_FontAsset font, bool isDynamicGrawing = false)
    {
        Debug.Log("set text");
        TextMeshProUGUI text = gameObject.AddComponent<TextMeshProUGUI>();
        text.text = textContent;
        text.fontSize = fontSize;
        text.color = textColor;
        text.alignment = textAlignmentOptions;

        text.enableWordWrapping = true;
        text.overflowMode = TextOverflowModes.Overflow;
        text.font = font;
        text.fontSharedMaterial = font.material;

        ContentSizeFitter contentSizeFitter = gameObject.AddComponent<ContentSizeFitter>();
        contentSizeFitter.verticalFit = isDynamicGrawing ? ContentSizeFitter.FitMode.PreferredSize : ContentSizeFitter.FitMode.Unconstrained;
        contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

        return text;
    }
    #endregion
}