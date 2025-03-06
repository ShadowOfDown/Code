using UnityEngine;
using System.Collections.Generic;

public interface IComponentBuilder
{
<<<<<<< HEAD
=======
  #region Properities
  public bool Enable { get; set; }
  #endregion

>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
  #region Methods
  public void Build(GameObject gameObject);
  public string GetComponentType();
  public HashSet<string> GetArguNameTable();
  #endregion
}