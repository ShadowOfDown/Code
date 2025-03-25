using UnityEngine;
using System.Collections.Generic;

public static class UniversalArgus
{
  #region SelectBox
  public readonly static string resourcesFolderPath = "Arts/Textures/UniversalComponent/";
  public readonly static string selectBoxImageName = "SelectBoxImage";
  public readonly static string normalSelectBoxButtonImageName = "NormalSelectBoxButtonImage_03";
  public readonly static string pressedSelectBoxButtonImageName = "PressedSelectBoxButtonImage_03";
  public readonly static float selectBoxButtonHeight = 0.24f;
  public readonly static float selectBoxTextHeight = 0.618f;
  public readonly static Dictionary<string, List<float>> textArgu = new()
  {
    {"fontSize", new List<float>{30f}},
    {"color", new List<float>{0.0f, 0.0f, 0.0f, 1.0f}},
    {"alignmentOption", TextComponentBuilder.middle},
    {"font", TextComponentBuilder.song},
  };

  #endregion
}