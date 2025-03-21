// 全局的屏蔽
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public class ScreenMaskUniversalBuilder : IUniversalGameObjectBuilder
{
  #region Fields
  #endregion


  #region Properties
  public string Type { get; } = "ScreenMaskGameObjectBuilder";
  public string GameObjectName { get; } = "ScreenMask";
  public Transform Transform { get { return ScreenMask.Transform;} }
  public RectTransform RectTransform { get { return ScreenMask.RectTransform;} }
  public ImageGameObjectBuilder ScreenMask { get; }
  #endregion


  #region Methods 
  public ScreenMaskUniversalBuilder(Transform parentTransform, bool blackCanvas = false)
  {
    ScreenMask = new ImageGameObjectBuilder("ScreenMask", parentTransform);
    ScreenMask.Build(
      new Dictionary<string, IComponentBuilder>
      {
        {"RectTransform", new RectTransformComponentBuilder(
          new Dictionary<string, Vector2>{
            {"referenceObjectPixels", PixelInfo.referenceScreenPixel},
            {"archorMin", new Vector2(0.5f, 0.5f)},
            {"archorMax", new Vector2(0.5f, 0.5f)},
            {"pivotPos", new Vector2(0.5f, 0.5f)},
          }
        )},
        {"Image", new ImageComponentBuilder(
          new Dictionary<string, string>{
            {"gameObjectName", null},
            {"resourcesFolderPath", null}    
          }
        )}
      }
    );
    ScreenMask.ModifyColor(blackCanvas? new Color(0, 0, 0, 0.5f) : new Color(0.5f, 0.5f, 0.5f, 0.5f));
  
    ScreenMask.SetActive(blackCanvas);
  }
  
  public void SetActive(bool state)
  {
    ScreenMask.SetActive(state);
  }
  #endregion
}