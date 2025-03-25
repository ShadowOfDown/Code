using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectGameObjectBuilder : GroupGameObjectBuilder
{
  #region Fields
  public override GameObject GameObject { get { return MaskBuilder.GameObject; }}
  public override string GameObjectName { get { return MaskBuilder.GameObjectName; } }
  public override Transform ParentTransform { get; }
  public override Transform Transform { get { return MaskBuilder.Transform; } }
  public override RectTransform RectTransform { get { return MaskBuilder.RectTransform; } }
  public readonly static string groupType = "TextBox";
  public readonly static HashSet<string> gameObjectTypeTable = new()
  {
    "MaskImage",
    "ScrollBar",
    "SlidingArea",
    "HandleImage",
  };
  #endregion

  #region Properties
  public ImageGameObjectBuilder MaskBuilder { get; set; }
  public RectTransformGameObjectBuilder ScrollbarBuilder { get; set; }
  public RectTransformGameObjectBuilder SlidingAreaBuilder { get; set; }
  public RectTransformGameObjectBuilder HandleBuilder { get; set; }
  public ScrollRect ScrollRect { get; set; }
  public float InitPos 
  { 
    get { return ScrollRect.verticalNormalizedPosition; } 
    set { ScrollRect.verticalNormalizedPosition = value;}
  }
  #endregion

  #region Methods
  public ScrollRectGameObjectBuilder(Transform parentTransform)
  {
    ParentTransform = parentTransform;

    GameObjectTable = new SortedDictionary<string, ISingleGameObjectBuilder>();

    MaskBuilder = new ImageGameObjectBuilder("MaskImage", parentTransform);
    GameObjectTable.Add("MaskImage", MaskBuilder);

    ScrollbarBuilder = new RectTransformGameObjectBuilder("ScrollBar", ParentTransform);
    GameObjectTable.Add("ScrollBar", ScrollbarBuilder);

    SlidingAreaBuilder = new RectTransformGameObjectBuilder("SlidingArea", GameObjectTable["ScrollBar"].Transform);
    GameObjectTable.Add("SlidingArea", SlidingAreaBuilder);

    HandleBuilder = new RectTransformGameObjectBuilder("HandleImage", GameObjectTable["SlidingArea"].Transform);
    GameObjectTable.Add("HandleImage", HandleBuilder);


    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log($"TextBoxGameObjectBuilder created with {GameObjectTable.Count} GameObjects");
    }
  }

  public override string GetGroupType()
  {
    return groupType;
  }

  public override HashSet<string> GetGameObjectTypeTable()
  {
    return gameObjectTypeTable;
  }

  public void SetContent(RectTransform contentRectTransform, string handleImagePath)
  {
    Mask mask = MaskBuilder.GameObject.AddComponent<Mask>();
    mask.showMaskGraphic = true;

    ScrollRect = MaskBuilder.GameObject.AddComponent<ScrollRect>();
    ScrollRect.vertical = true;
    ScrollRect.horizontal = false;
    ScrollRect.content = contentRectTransform;

    // 设置滑动区域的图像
    Image slidingAreaImage = SlidingAreaBuilder.GameObject.AddComponent<Image>();
    slidingAreaImage.color = new Color(154f / 255f, 131f / 255f, 112f / 255f, 1f);
    slidingAreaImage.preserveAspect = false;

    // 设置手柄的图像
    Image handleImage = HandleBuilder.GameObject.AddComponent<Image>();
    handleImage.sprite = Resources.Load<Sprite>(handleImagePath);
    
    handleImage.preserveAspect = false;
    

    Scrollbar scrollbar = ScrollbarBuilder.GameObject.AddComponent<Scrollbar>();
    ColorBlock scrollBarColors = new()
    {
        normalColor = Color.white,
        highlightedColor = Color.white,
        pressedColor = Color.white,
        disabledColor = Color.white,
        colorMultiplier = 1f,
        fadeDuration = 0f
    };
    scrollbar.direction = Scrollbar.Direction.BottomToTop;
    scrollbar.transition = Selectable.Transition.ColorTint;
    scrollbar.targetGraphic = handleImage;
    scrollbar.handleRect = HandleBuilder.RectTransform;
    scrollbar.colors = scrollBarColors;

    ScrollRect.viewport = MaskBuilder.GameObject.GetComponent<RectTransform>();
    ScrollRect.verticalNormalizedPosition = 0.0f;
    ScrollRect.verticalScrollbar = scrollbar;
    ScrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
    ScrollRect.verticalScrollbarSpacing = 0;
  }
  #endregion
}
