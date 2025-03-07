using UnityEngine;
using System.Collections.Generic;

public class RectTransformComponentBuilder : ComponentBuilder<Vector2>
{
  #region Fields
  public readonly static string componentType = "RectTranForm";
  public static readonly HashSet<string> arguNameTable = new()
  {
    "referenceObjectPixels",
    "archorMin",
    "archorMax",
    "pivotPos",
  };
  public static readonly Dictionary<string, Vector2> defaultArguTable = new()
  {
    {"referenceObjectPixels", PixelInfo.referenceScreenPixel},
    {"archorMin", Vector2.zero},
    {"archorMax", Vector2.one},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  #endregion


  #region Properties
  private RectTransform RectTransform { get; set; }
  public Dictionary<string, Vector2> PosArgus { get; set; }
  #endregion


  #region Methods

  public RectTransformComponentBuilder()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log($"start to create {GetComponentType()}");
    }
  }

  public RectTransformComponentBuilder(Dictionary<string, Vector2> posArgus)
  {
    PosArgus = posArgus;
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log($"start to create {GetComponentType()}");
    }
  }

  public override string GetComponentType()
  {
    return componentType;
  }

  public override Component GetComponent()
  {
    return RectTransform;
  }

  public override HashSet<string> GetArguNameTable()
  {
    return arguNameTable;
  }

  public override Dictionary<string, Vector2> GetDefaultArguTable()
  {
    return defaultArguTable;
  }

  public override void Modify(Dictionary<string, Vector2> posArgus)
  {
    if (posArgus == null)
    {
      posArgus = defaultArguTable;
    }
    
    RectTransform.anchorMin = posArgus["archorMin"];
    RectTransform.anchorMax = posArgus["archorMax"];
    RectTransform.pivot = posArgus["pivotPos"];
    RectTransform.localScale = Vector2.one;
    RectTransform.anchoredPosition = Vector2.zero;

    RectTransform.sizeDelta = new Vector2(
      PixelInfo.scaleRadio * posArgus["referenceObjectPixels"].x, PixelInfo.scaleRadio * posArgus["referenceObjectPixels"].y);

    Debug.Log($"RectTransform sizeDelta: {RectTransform.sizeDelta}");
  }

  public void Modify(RectTransform rectTransform)
  {
    RectTransform = rectTransform;
  }

  public override void Build(GameObject gameObject)
  {
    IsArgusValid(PosArgus);

    if (gameObject.TryGetComponent(out RectTransform rectTransform))
    {
      RectTransform = rectTransform;
    }
    else
    {
      RectTransform = gameObject.AddComponent<RectTransform>();
    }

    Modify(PosArgus);

    if (!DebugInfo.PrintDebugInfo)
    {
      Debug.Log($"successfully load {componentType} in {gameObject.name}");
    }
  }

  public void clearOffSet()
  {
    RectTransform.offsetMax = RectTransform.offsetMin = Vector2.zero;
  }
  #endregion
}