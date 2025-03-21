using UnityEngine;
using System.Collections.Generic;

public abstract class ComponentBuilder<T> : IComponentBuilder
{
  #region Property
  public virtual bool Enable { get; set; }  // 有的组件是没有这个东西的
  public Dictionary<string, T> ArguTable { get; set; }

  #endregion


  #region Methods
  public abstract string GetComponentType();

  public abstract Component GetComponent();
  
  public abstract HashSet<string> GetArguNameTable();
  
  public abstract Dictionary<string, T> GetDefaultArguTable();

  public abstract void Build(GameObject gameObject);

  public abstract void Modify(Dictionary<string, T> arguTable);

  public bool IsArgusValid(Dictionary<string, T> arguTable)
  {
    bool isArgusValid = false;
    if (GetArguNameTable() != null)
    {
      foreach (string arguName in GetArguNameTable())
      {
        if (arguTable != null && arguTable.ContainsKey(arguName) == false)
        {    
          arguTable.Add(arguName, GetDefaultArguTable()[arguName]);
          if (DebugInfo.PrintDebugInfo)
          {
            Debug.Log($"{arguName} is lacked, which be set to default value {arguTable[arguName]}");
          }
        }
        isArgusValid = false;
      }
    }
    return isArgusValid;
  }
  #endregion
}