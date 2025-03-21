using UnityEngine;

public interface IUniversalGameObjectBuilder
{
  #region Fields
  #endregion


  #region Properties
  public string Type { get; }
  public string GameObjectName { get; }
  public Transform Transform { get; }
  public RectTransform RectTransform { get; }
  #endregion


  #region Methods
  public void SetActive(bool state);
  #endregion
}