using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CanvasGameObjectBuilder : SingleGameObjectBuilder
{
  #region Fields
  public readonly static string gameObjectType = "Canvas";
  public readonly static HashSet<string> componentNameTable = new()
  {
    "RectTransform",
  };
  public readonly static Dictionary<string, IComponentBuilder> defaultComponentTable = new()
  {
    {"RectTransform", new RectTransformComponentBuilder()}
  };
  #endregion


  #region Properties
  public override string GameObjectName { get; }
  #endregion


  #region Methods
  public CanvasGameObjectBuilder()
  {
    GameObjectName = "Canvas";
    GameObject = new GameObject("Canvas");
  }

  public override string GetGameObjectType()
  {
    return gameObjectType;
  }

  public override HashSet<string> GetComponentNameTable()
  {
    return componentNameTable;
  }

  public override void Build(Dictionary<string, IComponentBuilder> componentTable)
  {
    ComponentTable = componentTable;
    foreach (IComponentBuilder component in ComponentTable.Values)
    {
      component.Build(GameObject);
    }

    Canvas canvas = GameObject.AddComponent<Canvas>();
    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    canvas.pixelPerfect = false;
    canvas.scaleFactor = 1.0f;

    CanvasScaler canvasScaler = GameObject.AddComponent<CanvasScaler>();
    canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
    canvasScaler.referenceResolution = PixelInfo.referenceScreenPixel;

    GameObject.AddComponent<GraphicRaycaster>();
  }

  public override Dictionary<string, IComponentBuilder> GetDefaultConponentTable()
  {
    return defaultComponentTable;
  }
  #endregion

}