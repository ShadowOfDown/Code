using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ContentSizeFitterComponentBuilder : ComponentBuilder<bool>
{
  #region Fields
  public readonly static string componentType = "ContentSizeFitter";
  public readonly static HashSet<string> arguNameTable = new()
  {
    "verticalFit",
    "horizontalFit",
  };
  public readonly static Dictionary<string, bool> defaultArguTable = new()
  {
    {"verticalFit", false},
    {"horizontalFit", false},
  };
  #endregion
  
  
  #region Properities
  public ContentSizeFitter ContentSizeFitter { set; get; }
<<<<<<< HEAD
=======
  public override bool Enable { get { return ContentSizeFitter.enabled; } set { ContentSizeFitter.enabled = value; } }
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
  #endregion


  #region Methods
  public ContentSizeFitterComponentBuilder()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log($"start to create {GetComponentType()}");
    } 
  }
  public ContentSizeFitterComponentBuilder(Dictionary<string, bool> arguTable)
  {
    ArguTable = arguTable;
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
    return ContentSizeFitter;
  }
  
  public override HashSet<string> GetArguNameTable()
  {
    return arguNameTable;
  }
  
  public override Dictionary<string, bool> GetDefaultArguTable()
  {
    return defaultArguTable;
  }

  public override void Modify(Dictionary<string, bool> arguTable)
  {
    ArguTable = arguTable;
<<<<<<< HEAD
    ContentSizeFitter.verticalFit = ArguTable["verticalFit"] ? 
      ContentSizeFitter.FitMode.PreferredSize : ContentSizeFitter.FitMode.Unconstrained;
    ContentSizeFitter.horizontalFit = ArguTable["horizontalFit"] ? 
      ContentSizeFitter.FitMode.PreferredSize : ContentSizeFitter.FitMode.Unconstrained;
=======
    if (arguTable != null && ContentSizeFitter != null)
    {
      ContentSizeFitter.verticalFit = ArguTable["verticalFit"] ? 
        ContentSizeFitter.FitMode.PreferredSize : ContentSizeFitter.FitMode.Unconstrained;
      ContentSizeFitter.horizontalFit = ArguTable["horizontalFit"] ? 
        ContentSizeFitter.FitMode.PreferredSize : ContentSizeFitter.FitMode.Unconstrained;
    }
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
  }

  public void Modify(ContentSizeFitter contentSizeFitter)
  {
    ContentSizeFitter = contentSizeFitter;
  }

  public override void Build(GameObject gameObject)  
  {
    if (gameObject.TryGetComponent(out ContentSizeFitter contentSizeFitter))
    {
      ContentSizeFitter = contentSizeFitter;
    }
    else 
    {
      ContentSizeFitter = gameObject.AddComponent<ContentSizeFitter>();
    }

    Modify(ArguTable);

    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log($"successfully load {componentType} in {gameObject.name}");
    }
  }
  #endregion
}