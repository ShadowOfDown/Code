using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HorizontalLayoutGroupGameObjectBuilder : SingleGameObjectBuilder
{
  #region Fields
  public readonly static string gameObjectType = "HorizontalLayoutGroup";
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
  #endregion


  #region Methods
  public HorizontalLayoutGroupGameObjectBuilder(string gameObjectName, Transform parentTransform)
  {
    GameObjectName = gameObjectName;
    GameObject = new GameObject(gameObjectName);
    GameObject.transform.SetParent(parentTransform);

    // 添加 ContentSizeFitter 组件
    ContentSizeFitter contentSizeFitter = GameObject.AddComponent<ContentSizeFitter>();
    contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize; // 横向适配
    contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained; // 纵向不约束

    // 添加 HorizontalLayoutGroup 组件
    HorizontalLayoutGroup horizontalLayoutGroup = GameObject.AddComponent<HorizontalLayoutGroup>();
    horizontalLayoutGroup.spacing = 0; // 子对象之间的间距
    horizontalLayoutGroup.childControlWidth = true; // 控制子对象的宽度
    horizontalLayoutGroup.childControlHeight = false; // 不控制子对象的高度
    horizontalLayoutGroup.childScaleWidth = true; // 缩放子对象的宽度
    horizontalLayoutGroup.childScaleHeight = false; // 不缩放子对象的高度
    horizontalLayoutGroup.childAlignment = TextAnchor.MiddleLeft; // 子对象对齐方式
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
    foreach (ISingleGameObjectBuilder child in GameObjectBuilderList)
    {
      // 为每个子对象添加 LayoutElement 组件
      LayoutElement layoutElement = child.GameObject.AddComponent<LayoutElement>();
      layoutElement.preferredWidth = child.RectTransform.rect.width; // 设置子对象的首选宽度
      layoutElement.preferredHeight = child.RectTransform.rect.height; // 设置子对象的首选高度
      layoutElement.flexibleWidth = 0; // 不拉伸宽度
      layoutElement.flexibleHeight = 0; // 不拉伸高度
    }
  }
  #endregion
}