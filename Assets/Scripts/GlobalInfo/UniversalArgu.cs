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
  public readonly static int selectBoxButtonLimit = 3;

  public static float getScaleRadio(int buttonCount)
  {
    return buttonCount / 10.0f + 0.7f;
  }

  public static Vector2 GetReferenceSelectBoxImagePixel(int buttonCount)
  {
    float scaleRadio = getScaleRadio(buttonCount);
    return new Vector2(960 * scaleRadio, 320 * scaleRadio);
  }


  public static Vector2 GetReferenceSelectBoxTextPixel(int buttonCount)
  {
    float scaleRadio = getScaleRadio(buttonCount) * 0.9f;
    return new Vector2(960 * scaleRadio, 320 * scaleRadio);
  }


  public static Vector2 GetReferenceSelectBoxButtonPixel(int buttonCount)
  {
    float scaleRadio = getScaleRadio(buttonCount);
    return new Vector2(240 * scaleRadio, 80 * scaleRadio);
  }


  public static Vector2 GetReferenceSelectBoxButtonTextPixel(int buttonCount)
  {
    float scaleRadio = getScaleRadio(buttonCount) * 0.9f;
    return new Vector2(240 * scaleRadio, 80 * scaleRadio);
  }
  #endregion


  #region Button
  public static string GetButtonName(int aspectRadio, bool pressed = false)
  {
    if (aspectRadio < 1 || aspectRadio > 3)
    {
      aspectRadio = 3;
      Debug.LogWarning("invalid aspect radio in univeral button" + aspectRadio);
    }

    return (pressed ? "Pressed" : "Normal") + "SelectBoxButtonImage_0" + aspectRadio.ToString();
  }

  public static Vector2 GetReferenceButtonPixel(int aspectRadio, bool isText = false)
  {
    if (aspectRadio < 0 || aspectRadio > 3)
    {
      Debug.LogWarning("invalid aspect radio in univeral button" + aspectRadio);
    }

    return aspectRadio switch
    {
      1 => new Vector2(160 * (isText ? 0.9f : 1.0f), 160 * (isText ? 0.9f : 1.0f)),
      2 => new Vector2(160 * (isText ? 0.9f : 1.0f), 80 * (isText ? 0.9f : 1.0f)),
      _ => new Vector2(240 * (isText ? 0.9f : 1.0f), 80 * (isText ? 0.9f : 1.0f)),
    };
    ;
  }
  #endregion
}