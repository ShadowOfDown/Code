using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MaskGameObjectBuilder : SingleGameObjectBuilder
{
  #region Fields
  public readonly static string gameObjectType = "Mask";
  public readonly static HashSet<string> componentNameTable = new()
  {
    "RectTransform",
  };

  public readonly static Dictionary<string, IComponentBuilder> defaultComponentTable = new()
  {
    {"RectTransform", new RectTransformComponentBuilder()},
  };
  #endregion


  #region Properties
  public override string GameObjectName { get; }
  public Mask Mask { get; set; }
  public ImageComponentBuilder Image
  {
    get
    {
      return ComponentTable["Image"] as ImageComponentBuilder;
    }
    set
    {
      ComponentTable["Image"] = value;
    }
  }
  public bool PreserveAspect
  {
    get
    {
      ImageComponentBuilder imageBuilder = ComponentTable["Image"] as ImageComponentBuilder;
      return imageBuilder.PreserveAspect;
    }
    set
    {
      ImageComponentBuilder imageBuilder = ComponentTable["Image"] as ImageComponentBuilder;
      imageBuilder.PreserveAspect = value;
    }
  }
  #endregion


  #region Methods
  public MaskGameObjectBuilder(string gameObjectName, Transform parentTransform)
  {
    GameObjectName = gameObjectName;
    GameObject = new GameObject(gameObjectName);
    GameObject.transform.SetParent(parentTransform);
  }

  public override void Build(Dictionary<string, IComponentBuilder> componentTable)
  {
    if (componentTable == null || componentTable.ContainsKey("RectTransform") == false)
    {
      Debug.LogWarning("RectTransform component is required for MaskGameObjectBuilder");
      return;
    }

    base.Build(new Dictionary<string, IComponentBuilder>
    {
      {"Image", new ImageComponentBuilder(new Dictionary<string, string>
        {
          {"gameObjectName", null},
          {"resourceFolderName", null},
        }
      )},
      {"RectTransform", componentTable["RectTransform"]},
    });

    Image.ModifyColor(1, 1, 1, 1.0f / 255.0f);

    Mask = GameObject.AddComponent<Mask>();
    Mask.showMaskGraphic = true;
  }

  public override string GetGameObjectType()
  {
    return gameObjectType;
  }

  public override HashSet<string> GetComponentNameTable()
  {
    return componentNameTable;
  }

  public override Dictionary<string, IComponentBuilder> GetDefaultConponentTable()
  {
    return defaultComponentTable;
  }

  public void ModifyGraphic(bool showGraphic)
  {
    if (Mask == null)
    {
      Debug.LogWarning("Mask component is not found");
      return;
    }

    Mask.showMaskGraphic = showGraphic;
  }

  #endregion
}