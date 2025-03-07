using UnityEngine;
using System.Collections.Generic;

public class ImageGameObjectBuilder : SingleGameObjectBuilder
{
  #region Fields
  public readonly static string gameObjectType = "Image";
  public readonly static HashSet<string> componentNameTable = new()
  {
    "RectTransform",
    "Image",
  };

  public readonly static Dictionary<string, IComponentBuilder> defaultComponentTable = new()
  {
    {"RectTransform", new RectTransformComponentBuilder()},
    {"Image", new ImageComponentBuilder()},
  };
  #endregion


  #region Properties
  public override string GameObjectName { get; }
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
  public ImageGameObjectBuilder(string gameObjectName, Transform parentTransform)
  {
    GameObjectName = gameObjectName;
    GameObject = new GameObject(gameObjectName);
    GameObject.transform.SetParent(parentTransform);
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

  public void ModifyColor(Color color)
  {
    ImageComponentBuilder imageBuilder = ComponentTable["Image"] as ImageComponentBuilder;
    imageBuilder.ModifyColor(color);
  }

  public void ModifyColor(float r, float g, float b, float a)
  {
    ImageComponentBuilder imageBuilder = ComponentTable["Image"] as ImageComponentBuilder;
    imageBuilder.ModifyColor(new Color(r, g, b, a));
  }
  #endregion
}