using UnityEngine;
using System.Collections.Generic;

public abstract class SingleGameObjectBuilder : ISingleGameObjectBuilder
{
  #region Property
  public virtual string GameObjectName { get; }
  public virtual Transform Transform
  {
    get { return GameObject.transform; }
  }
  public virtual RectTransform RectTransform
  {
    get { return GameObject.GetComponent<RectTransform>(); }
  }
  public GameObject GameObject { get; set; }
  public Dictionary<string, IComponentBuilder> ComponentTable { get; set; }
  #endregion


  #region Methods
  public abstract string GetGameObjectType();

  public abstract HashSet<string> GetComponentNameTable();

  public abstract Dictionary<string, IComponentBuilder> GetDefaultConponentTable();

  public virtual void Build(Dictionary<string, IComponentBuilder> componentTable)
  {
    ComponentTable = componentTable;

    if (IsComponentValid(ComponentTable) == false)
    {
      Debug.LogWarning($"gameObject {GameObjectName} lacks argu");
    }

    foreach (IComponentBuilder component in ComponentTable.Values)
    {
      if (component != null)
      {
        component.Build(GameObject);
      }
    }

    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log($"successfully build {GetGameObjectType()} {GameObjectName}!");
    }
  }

  public virtual void SetActive(bool state)
  {
    GameObject.SetActive(state);
  }

  public bool IsComponentValid(Dictionary<string, IComponentBuilder> componentTable)
  {
    bool isComponentValid = true;
    if (GetComponentNameTable() != null)
    {
      foreach (string componentName in GetComponentNameTable())
      {
        if (componentTable != null)
        {
          if (componentTable.ContainsKey(componentName) == false)
          {
            componentTable.Add(componentName, GetDefaultConponentTable()[componentName]);
            Debug.LogWarning($"{GameObjectName} lack component {componentName}");
          }
        }
        else 
        {
          isComponentValid = false;
          Debug.LogWarning($"{GameObjectName} lack component {componentName}");
        }
      }
    }

    return isComponentValid;
  }

  public void AddComponent(string componentName, IComponentBuilder component)
  {
    if (!ComponentTable.ContainsKey(componentName))
    {
      ComponentTable.Add(componentName, component);
    }
    else 
    {
      Debug.LogWarning($"Component {componentName} already exists" );
      ComponentTable[componentName] = component;
    }
  }

  public void clearOffSet()
  {
    RectTransform.offsetMax = RectTransform.offsetMin = Vector2.zero;
  }
  #endregion
}