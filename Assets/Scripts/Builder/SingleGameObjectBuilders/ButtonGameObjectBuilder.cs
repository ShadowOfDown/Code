using System.Collections.Generic;
using UnityEngine;

public class ButtonGameObjectBuilder : SingleGameObjectBuilder
{
  #region Fields
  public readonly static string gameObjectType = "Button";
  public readonly static HashSet<string> componentNameTable = new()
  {
    "RectTransform",
    "Image",
    "Button",
    "EventTrigger",
  };

  public readonly static Dictionary<string, IComponentBuilder> defaultComponentTable = new()
  {
    {"RectTransform", new RectTransformComponentBuilder()},
    {"Image", new ImageComponentBuilder()},
    {"Button", new ButtonComponentBuilder()},
    {"EventTrigger", new EventTriggerComponentBuilder()},
  };
  #endregion


  #region Properties
  public override string GameObjectName { get; }
  #endregion


  #region Fields
  public ButtonGameObjectBuilder(string gameObjectName, Transform parentTransform)
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
  #endregion
}