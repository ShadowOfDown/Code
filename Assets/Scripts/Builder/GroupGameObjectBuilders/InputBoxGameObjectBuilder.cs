using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class InputBoxGameObjectBuilder : GroupGameObjectBuilder
{
  #region Fields
  public override GameObject GameObject { get { return InputFieldBuilder.GameObject; }}
  public override string GameObjectName { get { return InputFieldBuilder.GameObjectName; } }
  public override Transform ParentTransform { get; }
  public override Transform Transform { get { return InputFieldBuilder.Transform; } }
  public override RectTransform RectTransform { get { return InputFieldBuilder.RectTransform; } }
  public readonly static string groupType = "TextBox";
  public readonly static HashSet<string> gameObjectTypeTable = new()
  {
    "InputField",
    "TextComponent",
    "PlaceHolder",
    "TextArea",
  };
  #endregion


  #region Properties
  public InputFieldGameObjectBuilder InputFieldBuilder { get; set; }
  public TextGameObjectBuilder TextComponentBuilder { get; set; }
  public TextGameObjectBuilder PlaceHolderBuilder { get; set; }
  public RectTransformGameObjectBuilder TextAreaBuilder { get; set; }
  public string Content
  {
    get { return TextComponentBuilder.Content; }
    set { TextComponentBuilder.Content = value; }
  }
  #endregion


  #region Methods
  public InputBoxGameObjectBuilder(string inputFieldName, Transform parentTransform)
  {
    ParentTransform = parentTransform;

    GameObjectTable = new SortedDictionary<string, ISingleGameObjectBuilder>
    {
      {"InputField", InputFieldBuilder = new InputFieldGameObjectBuilder(inputFieldName, parentTransform)},
      {"TextArea", TextAreaBuilder = new RectTransformGameObjectBuilder("TextArea",  InputFieldBuilder.Transform)},
      {"TextComponent", TextComponentBuilder = new TextGameObjectBuilder("TextComponent", TextAreaBuilder.Transform)},
      {"PlaceHolder", PlaceHolderBuilder = new TextGameObjectBuilder("PlaceHolder", TextAreaBuilder.Transform)}
    };

    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log($"inputbox created with {GameObjectTable.Count} GameObjects");
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

  public override void Build(SortedDictionary<string, Dictionary<string, IComponentBuilder>> gameObjectComponentTable)
  {
    base.Build(gameObjectComponentTable);
    InputFieldBuilder.ModifyText(TextAreaBuilder.RectTransform, TextComponentBuilder.TextBuilder.Text, PlaceHolderBuilder.TextBuilder.Text);
    TextAreaBuilder.GameObject.AddComponent<RectMask2D>();
  }

  public void ModifyContent(string content)
  {
    TextComponentBuilder.ModifyContent(content);
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log($"TextBoxGameObjectBuilder content set to {TextComponentBuilder.Content}");
    }
  }

  public string GetContent()
  {
    return InputFieldBuilder.GetContent();
  }
  #endregion
}