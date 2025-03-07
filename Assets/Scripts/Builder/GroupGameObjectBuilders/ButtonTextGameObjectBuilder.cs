using System.Collections.Generic;
using UnityEngine;

public class ButtonTextGameObjectBuilder : GroupGameObjectBuilder
{
  #region Fields
  public override GameObject GameObject { get { return ButtonBuilder.GameObject; }}
  public override Transform ParentTransform { get; }
  public override Transform Transform { get { return ButtonBuilder.Transform; } }
  public override RectTransform RectTransform { get { return ButtonBuilder.RectTransform; } }
  public readonly static string groupType = "ButtonText";
  public readonly static HashSet<string> gameObjectTypeTable = new()
  {
    "Button",
    "Text",
  };
  #endregion


  #region Properties
  public TextGameObjectBuilder TextBuilder { get; set; }
  public ButtonGameObjectBuilder ButtonBuilder { get; set; }
  public string Content
  {
    get { return TextBuilder.Content; }
    set { TextBuilder.Content = value; }
  }
  public override string GameObjectName { get { return ButtonBuilder.GameObjectName; } }
  #endregion


  #region Methods
  public ButtonTextGameObjectBuilder(Dictionary<string, string> gameObjectNameTable, Transform parentTransform)
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
      GameObjectTable.Add("Button", ButtonBuilder = new(gameObjectNameTable["Button"], parentTransform));

      if (LackedGameObjectNameTable.Contains("Text") == false)
      {
        GameObjectTable.Add("Text", TextBuilder = new(gameObjectNameTable["Text"], GameObjectTable["Button"].GameObject.transform));
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