using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public interface IGroupGameObjectBuilder
{
  #region Property
  public Transform ParentTransform { get; }
  public Transform Transform { get; }
  public RectTransform RectTransform { get; }
  public HashSet<string> LackedGameObjectNameTable { get; set; }
  public SortedDictionary<string, ISingleGameObjectBuilder> GameObjectTable { get; set; }
  #endregion


  #region Methods
  public void Build(SortedDictionary<string, Dictionary<string, IComponentBuilder>> gameObjectComponentTable);
  public void SetActive(bool state);
  public string GetGroupType();
  public HashSet<string> GetGameObjectTypeTable();
  public HashSet<string> GetLackedGameObjectNameTable();
  public bool IsGameObjectValid(SortedDictionary<string, Dictionary<string, IComponentBuilder>> componentTable);
  #endregion
}