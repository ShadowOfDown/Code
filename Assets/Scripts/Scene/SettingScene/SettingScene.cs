using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using Unity.VisualScripting;

public class SettingScene : UIObject
{
  private ImageGameObjectBuilder _backgroundImage;
  private ImageGameObjectBuilder _settingBackground;
  private VolumnInterface _volumnInterface; // 到时候应该会做成一个 List 来控制
  private ButtonUniversalbuilder _exitButton;
  private ButtonUniversalbuilder _volumnButton;
  private ButtonUniversalbuilder _imageButton;

  public static readonly string resourcesFolderPath = "Arts/Textures/SettingScene/";

  public override void OnLoad()
  {
    BuildBackgroundImage();
    BuildSettingBackground();
    BuildVolumnInterface();
    BuildExitButton();
    BuildVolumnButton();
    BuildImageButton();
  }

  public override void OnUpdate()
  {
  }

  public void SetActive(bool state)
  {
    // 因为还没有写其他的 interface(界面), 所以就没有做
  }

  private void BuildBackgroundImage()
  {
    _backgroundImage = new ImageGameObjectBuilder("BackgroundImage", this.transform);
    _backgroundImage.Build(
      new Dictionary<string, IComponentBuilder>
      {
        {"RectTransform", new RectTransformComponentBuilder(
          new Dictionary<string, Vector2>
          {
            {"referenceObjectPixels", PixelInfo.referenceScreenPixel},
            {"archorMin", new Vector2(0.5f, 0.5f)},
            {"archorMax", new Vector2(0.5f, 0.5f)},
            {"objectPos", new Vector2(0.5f, 0.5f)},
            {"pivotPos", new Vector2(0.5f, 0.5f)},
          }
        )},
        {"Image", new ImageComponentBuilder(
          new Dictionary<string, string>
          {
            {"gameObjectName", "BackgroundImage"},
            {"resourcesFolderPath", resourcesFolderPath},
          })
        }
      }
    );
  }

  private void BuildSettingBackground()
  {
    _settingBackground = new ImageGameObjectBuilder("SettingBackground", _backgroundImage.Transform);
    _settingBackground.Build(
      new Dictionary<string, IComponentBuilder>
      {
        {"RectTransform", new RectTransformComponentBuilder(
          new Dictionary<string, Vector2>
          {
            {"referenceObjectPixels", new Vector2(1860, 840)},
            {"archorMin", new Vector2(0.5f, 0.45f)},
            {"archorMax", new Vector2(0.5f, 0.45f)},
            {"pivotPos", new Vector2(0.5f, 0.5f)},
          }
        )},
        {"Image", new ImageComponentBuilder(
          new Dictionary<string, string>
          {
            {"gameObjectName", "SettingBackgroundImage"},
            {"resourcesFolderPath", resourcesFolderPath},
          })
        }
      }
    );
  }

  private void BuildVolumnInterface()
  {
    _volumnInterface = new VolumnInterface(_settingBackground.Transform);
  }

  private void BuildExitButton()
  {
    _exitButton = new ButtonUniversalbuilder("StartButton", new Vector2(0.9f, 0.9f), _backgroundImage.Transform, new Dictionary<string, UnityAction>
    {
      {"onClickAction", OnExitButtonClick},
    }, -1, 1);
  }

  private void BuildVolumnButton()
  {
    _volumnButton = new ButtonUniversalbuilder("StartButton", new Vector2(0.6f, 0.9f), _backgroundImage.Transform, new Dictionary<string, UnityAction>
    {
      {"onClickAction", OnVolumnButtonClick},
    }, -1, 1);
  }

  private void BuildImageButton()
  {
    _imageButton = new ButtonUniversalbuilder("StartButton", new Vector2(0.4f, 0.9f), _backgroundImage.Transform, new Dictionary<string, UnityAction>
    {
      {"onClickAction", OnImageButtonClick},
    }, -1, 1);
  }

  public void OnExitButtonClick()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log("exit button clicked");
    }
    UI_Manager.Instance.ShowUI<StartScene>("LoginUI");
    UI_Manager.Instance.CloseUI(this.gameObject.name);
  }

  public void OnVolumnButtonClick()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log("Volumn button clicked");
    }
  }

  public void OnImageButtonClick()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log("Image button clicked");
    }
  }
}
