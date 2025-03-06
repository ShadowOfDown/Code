// 控制音量的, 有什么东西之后再加上去
using UnityEngine;
using System.Collections.Generic;

public class VolumnInterface : IInterfaceBuilder
{
  #region Fields
  private SettingSliderUniversalBuilder _musicVolumeSlidering = null;
  #endregion

  #region Properties
  #endregion

  #region Methods
  public VolumnInterface(Transform parentTransform)
  {
    _musicVolumeSlidering = new SettingSliderUniversalBuilder("Music", parentTransform, 0.9f);
  }

  public void SetActive(bool state)
  {
    _musicVolumeSlidering.SetActive(state);
  }
  #endregion
}