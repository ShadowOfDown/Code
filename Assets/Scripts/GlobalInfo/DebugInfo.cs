using UnityEngine;
public static class DebugInfo
{
  public static bool PrintDebugInfo{ get; set; } = false;
  public static bool PrintSceneInfo{ get; set; } = true;
  // record what to print
  public static void Print(string content)
  {
    if (PrintDebugInfo)
    {
      Debug.Log(content);
    }
  }
}