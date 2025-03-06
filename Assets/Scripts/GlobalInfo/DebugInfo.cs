<<<<<<< HEAD
=======
using UnityEngine;
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
public static class DebugInfo
{
  public static bool PrintDebugInfo{ get; set; } = true;
  public static bool PrintSceneInfo{ get; set; } = true;
  // record what to print
<<<<<<< HEAD
=======
  public static void Print(string content)
  {
    if (PrintDebugInfo)
    {
      Debug.Log(content);
    }
  }
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
}