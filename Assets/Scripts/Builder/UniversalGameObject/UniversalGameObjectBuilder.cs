using UnityEngine;
using System.Collections.Generic;

public abstract class UniversalGameObjectBuilder : IUniversalGameObjectBuilder
{
  #region Fields
  public readonly static string resourcesFolderPath = "Arts/Textures/UniversalComponent/";
  #endregion


  #region Properties
  public abstract string Type { get; }
  public abstract string GameObjectName { get; }
  public abstract Transform Transform { get; }
  public abstract RectTransform RectTransform { get; }
  #endregion


  #region Methods
  public abstract void SetActive(bool state);
  #endregion
}