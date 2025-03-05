using UnityEngine;
using System.Collections.Generic;

public interface IComponentBuilder
{
  #region Properities
  public bool Enable { get; set; }
  #endregion

  #region Methods
  public void Build(GameObject gameObject);
  public string GetComponentType();
  public HashSet<string> GetArguNameTable();
  #endregion
}