using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScrollbarLoader : ImageLoader
{
    #region field
    private readonly ImageLoader _EndBar;
    private readonly GameObjectLoader _handle;
    private readonly TextLoader _text;
    private readonly GameObjectLoader _verticalFrame;
    private readonly ScrollRect _scrollRect;
    private readonly GameObjectLoader _slidingAreaObject;
    private readonly GameObjectLoader _scrollBarObject;
    private readonly Mask _mask;
    #endregion

    #region Properties
    public RectTransform HandleRect
    {
        get => _handle.RectTransform;
        set => _handle.RectTransform = value;
    }
    public RectTransform SlidingAreaRect
    {
        get => _slidingAreaObject.GameObject.GetComponent<RectTransform>();
        set => _slidingAreaObject.GameObject.GetComponent<RectTransform>().position = value.position;
    }
    public ScrollRect ScrollRect => _scrollRect;
    public GameObjectLoader Handle => _handle;
    public TextLoader Text => _text;
    public GameObjectLoader VerticalFrame => _verticalFrame;
    public GameObjectLoader SlidingAreaObject => _slidingAreaObject;
    #endregion

    #region method
    public ScrollbarLoader(
        string resourcesPath, string maskName, string textName, string endBarName, 
        Transform parentTransform,
        Vector2 referenceMaskPixel, Vector2 referenceEndBarPixel, Vector2 referenceScrollAreaPixel, Color handleColor,
        string textContent, int fontSize, Color textColor, TextAlignmentOptions textAlignmentOptions, TMP_FontAsset font,
        float scaleRatio = 1, float textRadio = 1, bool isHeightBench = true, 
        Vector2? objectPos = null, Vector2? slidingAreaPos = null, Vector2? pivotPos = null) :
        base(resourcesPath, maskName, parentTransform, referenceMaskPixel, scaleRatio, isHeightBench, objectPos)
    {
        _mask = _gameObject.AddComponent<Mask>();
        _mask.showMaskGraphic = true;

        _verticalFrame = new GameObjectLoader("VerticalFrame", _gameObject.transform, referenceMaskPixel, scaleRatio, isHeightBench, objectPos);
        _text = new TextLoader(
            textName, _verticalFrame.GameObject.transform, referenceMaskPixel, textContent, fontSize, textColor, textAlignmentOptions, font, true, textRadio, false, objectPos);
        _EndBar = new ImageLoader(resourcesPath, endBarName, _verticalFrame.GameObject.transform, referenceEndBarPixel, 1, false, objectPos);

        ContentSizeFitter contentSizeFitter= _verticalFrame.GameObject.AddComponent<ContentSizeFitter>();
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

        VerticalLayoutGroup verticalLayoutGroup = _verticalFrame.GameObject.AddComponent<VerticalLayoutGroup>();
        verticalLayoutGroup.spacing = 0;
        verticalLayoutGroup.childControlHeight = true;
        verticalLayoutGroup.childControlWidth = false;
        verticalLayoutGroup.childScaleHeight = true;
        verticalLayoutGroup.childScaleWidth = false;
        verticalLayoutGroup.childAlignment = TextAnchor.LowerCenter;

        LayoutElement endLayoutElement = _EndBar.GameObject.AddComponent<LayoutElement>();
        endLayoutElement.preferredHeight = _EndBar.GameObject.GetComponent<RectTransform>().rect.height;
        endLayoutElement.preferredWidth = _EndBar.GameObject.GetComponent<RectTransform>().rect.width;
        endLayoutElement.flexibleHeight = 0;

        _scrollRect = _gameObject.AddComponent<ScrollRect>();
        _scrollRect.vertical = true;
        _scrollRect.horizontal = false;
        _scrollRect.content = _verticalFrame.GameObject.GetComponent<RectTransform>();

        _scrollBarObject = new("ScrollBar", parentTransform, referenceScrollAreaPixel, scaleRatio, true, slidingAreaPos);
        Scrollbar scrollbar = _scrollBarObject.GameObject.AddComponent<Scrollbar>();
        scrollbar.direction = Scrollbar.Direction.BottomToTop;

        _slidingAreaObject = new("slidingArea", _scrollBarObject.GameObject.transform, referenceScrollAreaPixel, referenceScrollAreaPixel.y / _referencePixel.y, true, slidingAreaPos);

        _handle = new GameObjectLoader("handle", _slidingAreaObject.GameObject.transform, referenceScrollAreaPixel, 1, false, pivotPos);
        _handle.RectTransform.sizeDelta = new Vector2(-1, -1);
        Image handleImage = _handle.GameObject.AddComponent<Image>();
        handleImage.color = Color.white;
        
        scrollbar.handleRect = _handle.RectTransform;

        _scrollRect.viewport = _rectTransform;
        _scrollRect.verticalScrollbar = scrollbar;
        _scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
        _scrollRect.verticalScrollbarSpacing = 0;
    }
    #endregion
}
