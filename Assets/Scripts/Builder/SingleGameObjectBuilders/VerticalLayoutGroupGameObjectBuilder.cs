using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class VerticalLayoutGroupGameObjectBuilder : SingleGameObjectBuilder
{
  #region Fields
  public readonly static string gameObjectType = "VerticalLayoutGroup";
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
  public VerticalLayoutGroup VerticalLayoutGroup { get; set; }
  public float Spacing { get { return VerticalLayoutGroup.spacing; } set { VerticalLayoutGroup.spacing = value; } }
  public List<ISingleGameObjectBuilder> Children{ get; set; } = null;
  #endregion


  #region Methods
  public VerticalLayoutGroupGameObjectBuilder(string gameObjectName, Transform parentTransform)
  {
    GameObjectName = gameObjectName;
    GameObject = new GameObject(gameObjectName);
    GameObject.transform.SetParent(parentTransform);

    ContentSizeFitter contentSizeFitter = GameObject.AddComponent<ContentSizeFitter>();
    contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

    VerticalLayoutGroup = GameObject.AddComponent<VerticalLayoutGroup>();
    VerticalLayoutGroup.spacing = 0;
    VerticalLayoutGroup.childControlHeight = true;
    VerticalLayoutGroup.childControlWidth = false;
    VerticalLayoutGroup.childScaleHeight = true;
    VerticalLayoutGroup.childScaleWidth = false;
    VerticalLayoutGroup.childAlignment = TextAnchor.LowerCenter;
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

  public void AddChild(List<ISingleGameObjectBuilder> GameObjectBuilderList) 
  {
    Children = GameObjectBuilderList;
    foreach (ISingleGameObjectBuilder child in GameObjectBuilderList)
    {
      LayoutElement layoutElement = child.GameObject.AddComponent<LayoutElement>();
      layoutElement.preferredHeight = child.RectTransform.rect.height;
      layoutElement.preferredWidth = child.RectTransform.rect.width;
      layoutElement.flexibleHeight = 0;
    }
  }

  public override void SetActive(bool state)
  {
    GameObject.SetActive(state);
    if (Children != null)
    {
      foreach (ISingleGameObjectBuilder child in Children)
      {
        child.SetActive(state);
      }
    }
  }
  #endregion
}