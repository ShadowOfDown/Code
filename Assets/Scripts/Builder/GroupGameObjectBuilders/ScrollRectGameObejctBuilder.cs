using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectGameObjectBuilder : GroupGameObjectBuilder
{
  #region Fields
<<<<<<< HEAD
=======
  public override GameObject GameObject { get { return MaskBuilder.GameObject; }}
  public override string GameObjectName { get { return MaskBuilder.GameObjectName; } }
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
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
<<<<<<< HEAD
=======
  public ScrollRect ScrollRect { get; set; }
  public float InitPos 
  { 
    get { return ScrollRect.verticalNormalizedPosition; } 
    set { ScrollRect.verticalNormalizedPosition = value;}
  }
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
  #endregion


  #region Methods
  public ScrollRectGameObjectBuilder(Transform parentTransform)
  {
    ParentTransform = parentTransform;


    GameObjectTable = new SortedDictionary<string, ISingleGameObjectBuilder>
    {
<<<<<<< HEAD
        { "MaskImage", MaskBuilder = new("MaskImage", parentTransform) },
        { "ScrollBar", ScrollbarBuilder = new("ScrollBar", ParentTransform) }
=======
      { "MaskImage", MaskBuilder = new("MaskImage", parentTransform) },
      { "ScrollBar", ScrollbarBuilder = new("ScrollBar", ParentTransform) }
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
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

<<<<<<< HEAD
    ScrollRect scrollRect = MaskBuilder.GameObject.AddComponent<ScrollRect>();
    scrollRect.vertical = true;
    scrollRect.horizontal = false;
    scrollRect.content = contentRectTransform;
=======
    ScrollRect = MaskBuilder.GameObject.AddComponent<ScrollRect>();
    ScrollRect.vertical = true;
    ScrollRect.horizontal = false;
    ScrollRect.content = contentRectTransform;
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3

    Image handleImage = HandleBuilder.GameObject.AddComponent<Image>();
    handleImage.color = handleColor;

    Scrollbar scrollbar = ScrollbarBuilder.GameObject.AddComponent<Scrollbar>();
    scrollbar.direction = Scrollbar.Direction.BottomToTop;
    scrollbar.transition = Selectable.Transition.ColorTint;
    scrollbar.targetGraphic = HandleBuilder.GameObject.GetComponent<Image>();
    scrollbar.handleRect = HandleBuilder.RectTransform;
    scrollbar.colors = scrollBarColors;

<<<<<<< HEAD
    scrollRect.viewport = MaskBuilder.GameObject.GetComponent<RectTransform>();
    scrollRect.verticalNormalizedPosition = 0.0f;
    scrollRect.verticalScrollbar = scrollbar;
    scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
    scrollRect.verticalScrollbarSpacing = 0;
=======
    ScrollRect.viewport = MaskBuilder.GameObject.GetComponent<RectTransform>();
    ScrollRect.verticalNormalizedPosition = 0.0f;
    ScrollRect.verticalScrollbar = scrollbar;
    ScrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
    ScrollRect.verticalScrollbarSpacing = 0;
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
  }
  #endregion
}