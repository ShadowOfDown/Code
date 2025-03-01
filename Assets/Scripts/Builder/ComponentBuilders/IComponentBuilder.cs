using UnityEngine;
using System.Collections.Generic;

public interface IComponentBuilder
{
  #region Methods
  public void Build(GameObject gameObject);
  public string GetComponentType();
  public HashSet<string> GetArguNameTable();
  #endregion
}