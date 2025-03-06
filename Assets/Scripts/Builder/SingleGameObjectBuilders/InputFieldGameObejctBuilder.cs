using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class InputFieldGameObjectBuilder : SingleGameObjectBuilder
{
  #region Fields
  public readonly static string gameObjectType = "InputField";
  public readonly static HashSet<string> componentNameTable = new()
  {
    "Image",
    "RectTransform",
    "InputField",
  };

  public readonly static Dictionary<string, IComponentBuilder> defaultComponentTable = new()
  {
    {"Image", new ImageComponentBuilder()},
    {"RectTransform", new RectTransformComponentBuilder()},
    {"InputField", new InputFieldComponentBuilder()}
  };
  #endregion


  #region Properties
  public override string GameObjectName { get; }
  public InputFieldComponentBuilder InputField 
  { 
    get { return ComponentTable["InputField"] as InputFieldComponentBuilder; }  
  }
  #endregion


  #region Methods
  public InputFieldGameObjectBuilder(string gameObjectName, Transform parentTransform)
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

  public void ModifyText(RectTransform textViewpoint, TextMeshProUGUI textComponent = null, TextMeshProUGUI placeHolder = null)
  {
    InputField. ModifyText(textViewpoint, textComponent, placeHolder);  
  }

  public string GetContent()
  {
    return InputField.GetContent();
  }
  #endregion
}