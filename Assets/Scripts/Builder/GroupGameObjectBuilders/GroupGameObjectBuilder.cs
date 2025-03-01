using UnityEngine;
using System.Collections.Generic;

public abstract class GroupGameObjectBuilder : IGroupGameObjectBuilder
{
  #region Property
  public abstract Transform ParentTransform { get; }
  public abstract Transform Transform{ get; }
  public abstract RectTransform RectTransform { get; }
  public SortedDictionary<string, ISingleGameObjectBuilder> GameObjectTable { get; set; }
  public HashSet<string> LackedGameObjectNameTable { get; set; } = new HashSet<string>();
  #endregion


  #region Methods
  public abstract string GetGroupType();

  public abstract HashSet<string> GetGameObjectTypeTable();

  public HashSet<string> GetLackedGameObjectNameTable()
  {
    return LackedGameObjectNameTable;
  }

  public void Build(SortedDictionary<string, Dictionary<string, IComponentBuilder>> gameObjectComponentTable)
  {
    if (IsGameObjectValid(gameObjectComponentTable) == false)
    {
      Debug.LogWarning($"{GetGroupType()} lack GameObject");
    }

    foreach (string gameObjectName in GetGameObjectTypeTable())
    {
      if (LackedGameObjectNameTable.Contains(gameObjectName) == false)
      {
        GameObjectTable[gameObjectName].Build(gameObjectComponentTable[gameObjectName]);
      }
    }
  }

  public void SetActive(bool state)
  {
    foreach (string gameObjectName in GetGameObjectTypeTable())
    {
      if (GameObjectTable.ContainsKey(gameObjectName))
      {
        GameObjectTable[gameObjectName].SetActive(state);
      }
      else
      {
        if (LackedGameObjectNameTable.Contains(gameObjectName) == false)
        {
          LackedGameObjectNameTable.Add(gameObjectName);
        }
        Debug.LogWarning($"{GetGroupType()} lack component in {gameObjectName}");
      }
    }
  }

  public bool IsGameObjectValid(SortedDictionary<string, Dictionary<string, IComponentBuilder>> gameObjectComponentTable)
  {
    bool isGameObjectValid = true;

    foreach (string gameObjectName in GetGameObjectTypeTable())
    {
      if (GameObjectTable.ContainsKey(gameObjectName))
      {
        if (GameObjectTable[gameObjectName].IsComponentValid(gameObjectComponentTable[gameObjectName]) == false)
        {
          Debug.LogWarning($"{GetGroupType()} lack component in {gameObjectName}");
          isGameObjectValid = false;
        }
      }
      else
      {
        if (LackedGameObjectNameTable.Contains(gameObjectName) == false)
        {
          LackedGameObjectNameTable.Add(gameObjectName);
        }
        Debug.LogWarning($"{GetGroupType()} lack component in {gameObjectName}");
        isGameObjectValid = false;
      }
    }

    return isGameObjectValid;
  }
  #endregion
}