using UnityEngine;
using System.Collections.Generic;

public interface ISingleGameObjectBuilder
{
  #region Property
  public string GameObjectName { get; }
  public Transform Transform { get; }
  public RectTransform RectTransform{ get; }
  public GameObject GameObject { get; set; }
  #endregion


  #region Methods
  public string GetGameObjectType();
  public HashSet<string> GetComponentNameTable();
  public Dictionary<string, IComponentBuilder> GetDefaultConponentTable();
  public void Build(Dictionary<string, IComponentBuilder> componentTable);
  public void SetActive(bool state);
  public bool IsComponentValid(Dictionary<string, IComponentBuilder> componentTable);
  public void AddComponent(string componentName, IComponentBuilder component);
  #endregion
}