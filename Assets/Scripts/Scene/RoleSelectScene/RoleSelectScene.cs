using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class RoleSelectScene : ISceneState
{
  public static readonly string resourcesFolderPath = "Arts/Textures/RoleSelectScene/";
  private static readonly int _interfaecLimit = 2;
  private static int _interfaceCount = 0;
  public static int roleNum = 0;
  private static readonly List<List<bool>> _interfaceList = new()
  {
    new List<bool> { true, false },
    new List<bool> { false, true },
  };

  private CanvasGameObjectBuilder _canvas;
  private ScreenMaskUniversalBuilder _baseImage; // 防止底层泄露
  private ImageGameObjectBuilder _backgroundImage;
  private readonly List<RoleImageBox> roleInfoBoxList = new();
  private RoleInfoInterface _RoleInfoInterface;

  public RoleSelectScene(SceneStateControl control) : base(control)
  {
    this.StateName = "RoleSelectScene";
  }

  public override void StateBegin()
  {
    UI_Manager.Instance.Init();
    BuildCanvas();
    BuildBaseImage();
    BuildBackgroundImage();
    BuildRoleInfoBoxes();
    BuildRoleInfoInterface();
  }

  public override void StateUpdate()
  {
    SetActive();
  }

  public void SetActive()
  {
    foreach (var roleInfoBox in roleInfoBoxList)
    {
      roleInfoBox.SetActive(_interfaceList[_interfaceCount][0]);
    }
    _RoleInfoInterface.SetActive(_interfaceList[_interfaceCount][1]);
    _RoleInfoInterface.ModifyCharacter(roleNum);
  }

  public static void UpdateInterface(int interfaceCount)
  {
    _interfaceCount = interfaceCount % _interfaecLimit;
  }

  private void BuildCanvas()
  {
    _canvas = new CanvasGameObjectBuilder();
    _canvas.Build(
      new Dictionary<string, IComponentBuilder>
      {
        {
          "RectTransform",
          new RectTransformComponentBuilder(
            new Dictionary<string, Vector2>
            {
              {"referenceObjectPixels", PixelInfo.referenceScreenPixel},
              {"archorMin", new Vector2(0.5f, 0.5f)},
              {"archorMax", new Vector2(0.5f, 0.5f)},
              {"objectPos", new Vector2(0.5f, 0.5f)},
              {"pivotPos", new Vector2(0.5f, 0.5f)}
            }
          )
        }
      }
    );
  }

  private void BuildBaseImage()
  {
    _baseImage = new ScreenMaskUniversalBuilder(_canvas.Transform, true);
  }

  private void BuildBackgroundImage()
  {
    _backgroundImage = new ImageGameObjectBuilder("BackgroundImage", _baseImage.Transform);
    _backgroundImage.Build(
      new Dictionary<string, IComponentBuilder>
      {
        {
          "RectTransform",
          new RectTransformComponentBuilder(
            new Dictionary<string, Vector2>
            {
              {"referenceObjectPixels", PixelInfo.referenceScreenPixel},
              {"archorMin", new Vector2(0.5f, 0.5f)},
              {"archorMax", new Vector2(0.5f, 0.5f)},
              {"objectPos", new Vector2(0.5f, 0.5f)},
              {"pivotPos", new Vector2(0.5f, 0.5f)}
            }
          )
        },
        {
          "Image",
          new ImageComponentBuilder(
            new Dictionary<string, string>
            {
              {"gameObjectName", "BackgroundImage_02"},
              {"resourcesFolderPath", resourcesFolderPath}
            }
          )
        }
      }
    );
  }

  private void BuildRoleInfoBoxes()
  {
    for (int roleNum = 0; roleNum < 8; roleNum++)
    {
      roleInfoBoxList.Add(new RoleImageBox(roleNum, _backgroundImage.Transform));
    }
  }

  private void BuildRoleInfoInterface()
  {
    _RoleInfoInterface = new RoleInfoInterface(_backgroundImage.Transform, 0);
    _RoleInfoInterface.SetActive(false);
  }
}