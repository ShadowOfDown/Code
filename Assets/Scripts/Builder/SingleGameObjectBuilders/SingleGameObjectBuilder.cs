using UnityEngine;
using System.Collections.Generic;

public abstract class SingleGameObjectBuilder : ISingleGameObjectBuilder
{
  #region Property
  public virtual string GameObjectName { get; }
  public virtual Transform Transform
  {
<<<<<<< HEAD
    get
    {
      return GameObject.transform;
    }
  }
  public virtual RectTransform RectTransform
  {
    get
    {
      return GameObject.GetComponent<RectTransform>();
    }
=======
    get { return GameObject.transform; }
  }
  public virtual RectTransform RectTransform
  {
    get { return GameObject.GetComponent<RectTransform>(); }
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
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
<<<<<<< HEAD
      component.Build(GameObject);
=======
      if (component != null)
      {
        component.Build(GameObject);
      }
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
    }

    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log($"successfully build {GetGameObjectType()} {GameObjectName}!");
    }
  }

<<<<<<< HEAD
  public void SetActive(bool state)
=======
  public virtual void SetActive(bool state)
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
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
<<<<<<< HEAD
        if (componentTable.ContainsKey(componentName) == false)
        {
          componentTable.Add(componentName, GetDefaultConponentTable()[componentName]);
          Debug.LogWarning($"{GameObjectName} lack component {componentName}");
          isComponentValid = false;
=======
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
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
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
<<<<<<< HEAD
=======

  public void clearOffSet()
  {
    RectTransform.offsetMax = RectTransform.offsetMin = Vector2.zero;
  }
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
  #endregion
}