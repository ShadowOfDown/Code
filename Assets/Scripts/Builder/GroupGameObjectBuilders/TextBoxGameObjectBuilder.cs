using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class TextBoxGameObjectBuilder : GroupGameObjectBuilder
{
  #region Fields
  public override GameObject GameObject { get { return ImageBuilder.GameObject; }}
  public override string GameObjectName { get { return ImageBuilder.GameObjectName; } }
  public override Transform ParentTransform { get; }
  public override Transform Transform { get { return ImageBuilder.Transform; } }
  public override RectTransform RectTransform { get { return ImageBuilder.RectTransform;}}
  public readonly static string groupType = "TextBox";
  public readonly static HashSet<string> gameObjectTypeTable = new()
  {
    "Image",
    "Text",
  };
  #endregion


  #region Properties
  public TextGameObjectBuilder TextBuilder { get; set; }
  public ImageGameObjectBuilder ImageBuilder { get; set; }
  public string Content
  {
    get { return TextBuilder.Content; }
    set { TextBuilder.Content = value; }
  }
  #endregion


  #region Methods
  public TextBoxGameObjectBuilder(Dictionary<string, string> gameObjectNameTable, Transform parentTransform)
  {
    ParentTransform = parentTransform;

    foreach (var gameObjectName in GetGameObjectTypeTable())
    {
      if (!gameObjectNameTable.ContainsKey(gameObjectName))
      {
        Debug.LogWarning($"{gameObjectName} not found in gameObjectNameTable");
        LackedGameObjectNameTable.Add(gameObjectName);
      }
    }

    GameObjectTable = new SortedDictionary<string, ISingleGameObjectBuilder>();
    if (LackedGameObjectNameTable.Contains("Image") == false)
    {
      GameObjectTable.Add("Image", ImageBuilder = new(gameObjectNameTable["Image"], parentTransform));

      if (LackedGameObjectNameTable.Contains("Text") == false)
      {
        GameObjectTable.Add("Text", TextBuilder = new(gameObjectNameTable["Text"], GameObjectTable["Image"].GameObject.transform));
      }
    }

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

  public void ModifyContent(string content)
  {
    TextBuilder.ModifyContent(content);
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log($"TextBoxGameObjectBuilder content set to {TextBuilder.Content}");
    }
  }
  #endregion
}