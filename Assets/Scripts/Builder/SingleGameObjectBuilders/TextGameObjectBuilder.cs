using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class TextGameObjectBuilder : SingleGameObjectBuilder
{
  #region Fields
  public readonly static string gameObjectType = "Text";
  public readonly static HashSet<string> componentNameTable = new()
  {
    "ContentSizeFitter",
    "RectTransform",
    "Text",
  };

  public readonly static Dictionary<string, IComponentBuilder> defaultComponentTable = new()
  {
    {"Text", new TextComponentBuilder()},
    {"RectTransform", new RectTransformComponentBuilder()},
    {"ContentSizeFitter", new ContentSizeFitterComponentBuilder()}
  };
  #endregion


  #region Properties
  public override string GameObjectName { get; }
  public TextComponentBuilder TextBuilder { get { return ComponentTable["Text"] as TextComponentBuilder; } }
  public string Content
  {
    get { return TextBuilder.Content; }
    set { TextBuilder.ModifyContent(value); }
  }

  #endregion


  #region Fields
  public TextGameObjectBuilder(string gameObjectName, Transform parentTransform)
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

  public override void Build(Dictionary<string, IComponentBuilder> componentTable)
  {
    if (componentTable.ContainsKey("ContentSizeFitter") == false)
    {
      componentTable.Add("ContentSizeFitter", new ContentSizeFitterComponentBuilder(ContentSizeFitterComponentBuilder.defaultArguTable));
    }
    else if (componentTable["ContentSizeFitter"] == null)
    {
      componentTable["ContentSizeFitter"] = new ContentSizeFitterComponentBuilder(ContentSizeFitterComponentBuilder.defaultArguTable);
    }

    base.Build(componentTable);
  }

  public void ModifyContent(string content)
  {
    TextBuilder.ModifyContent(content);
  }
  #endregion
}