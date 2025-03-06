using UnityEngine;
using System.Collections.Generic;
<<<<<<< HEAD
=======
using TMPro;
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3

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
<<<<<<< HEAD
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
  
=======
  public TextComponentBuilder TextBuilder { get { return ComponentTable["Text"] as TextComponentBuilder; } }
  public string Content
  {
    get { return TextBuilder.Content; }
    set { TextBuilder.ModifyContent(value); }
  }

>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
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

<<<<<<< HEAD
  public void ModifyContent(string content)
  {
    TextComponentBuilder textComponentBuilder = ComponentTable["Text"] as TextComponentBuilder;
    textComponentBuilder.ModifyContent(content);
=======
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
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
  }
  #endregion
}