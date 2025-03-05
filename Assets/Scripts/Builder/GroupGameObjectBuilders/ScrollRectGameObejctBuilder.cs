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


    GameObjectTable = new SortedDictionary<string, ISingleGameObjectBuilder>
    {
      { "MaskImage", MaskBuilder = new("MaskImage", parentTransform) },
      { "ScrollBar", ScrollbarBuilder = new("ScrollBar", ParentTransform) }
    };
    GameObjectTable.Add("SlidingArea", SlidingAreaBuilder = new("SlidingArea", GameObjectTable["ScrollBar"].Transform));
    GameObjectTable.Add("HandleImage", HandleBuilder = new("HandleImage", GameObjectTable["SlidingArea"].Transform));

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

  public void SetContent(RectTransform contentRectTransform, ColorBlock scrollBarColors, Color handleColor)
  {
    Mask mask = MaskBuilder.GameObject.AddComponent<Mask>();
    mask.showMaskGraphic = true;

    ScrollRect = MaskBuilder.GameObject.AddComponent<ScrollRect>();
    ScrollRect.vertical = true;
    ScrollRect.horizontal = false;
    ScrollRect.content = contentRectTransform;

    Image handleImage = HandleBuilder.GameObject.AddComponent<Image>();
    handleImage.color = handleColor;

    Scrollbar scrollbar = ScrollbarBuilder.GameObject.AddComponent<Scrollbar>();
    scrollbar.direction = Scrollbar.Direction.BottomToTop;
    scrollbar.transition = Selectable.Transition.ColorTint;
    scrollbar.targetGraphic = HandleBuilder.GameObject.GetComponent<Image>();
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