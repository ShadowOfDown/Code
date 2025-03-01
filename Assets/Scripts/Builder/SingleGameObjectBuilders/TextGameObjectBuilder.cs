using UnityEngine;
using System.Collections.Generic;

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
  public string Content 
  { 
    get
    {
      TextComponentBuilder textComponentBuilder= ComponentTable["Text"] as TextComponentBuilder;
      return textComponentBuilder.Content;
    } 
    set
    {
      TextComponentBuilder textComponentBuilder= ComponentTable["Text"] as TextComponentBuilder;
      textComponentBuilder.ModifyContent(value);
    } 
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

  public void ModifyContent(string content)
  {
    TextComponentBuilder textComponentBuilder = ComponentTable["Text"] as TextComponentBuilder;
    textComponentBuilder.ModifyContent(content);
  }
  #endregion
}