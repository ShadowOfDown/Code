using UnityEngine;
using System;

public static class PixelInfo
{
  public static Vector2 referenceScreenPixel = new(1920, 1080);
  public static Vector2 screenPixel = new(Screen.width, Screen.height);
  public static float scaleRadio = screenPixel.x / referenceScreenPixel.x < screenPixel.y / referenceScreenPixel.y ? 1  : screenPixel.y / referenceScreenPixel.y / (screenPixel.x / referenceScreenPixel.x);
}