using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class RoleIntroduction
{
  #region Properties
  #region SelectBackgroundImage
  private readonly Dictionary<string, string> _selectBackgroundImageArgu = new()
  {
    {"gameObjectName", "SelectBackgroundImage"},
    {"resourcesFolderPath", RoleSelectScene.resourcesFolderPath},
  };
  private readonly Dictionary<string, Vector2> _selectBackgroundRectTransformArgu = new()
  {
    {"referenceObjectPixels", new Vector2(1680, 640)},
    {"archorMin", new Vector2(0.5f, 0.6f)},
    {"archorMax", new Vector2(0.5f, 0.6f)},
    {"objectPos", new Vector2(0.5f, 0.5f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  #endregion


  #region RoleImage
  private readonly Dictionary<string, string> _roleImageArgu = new()
  {
    {"gameObjectName", "RoleImage/RoleImage_0"},
    {"resourcesFolderPath", RoleSelectScene.resourcesFolderPath},
  };
  private readonly Dictionary<string, Vector2> _roleRectTransformArgu = new()
  {
    {"referenceObjectPixels", new Vector2(420, 560)},
    {"archorMin", new Vector2(0.1488f, 0.5f)},
    {"archorMax", new Vector2(0.1488f, 0.5f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  #endregion


  #region NameCard
  private readonly Dictionary<string, string> _nameCardNameArgu = new()
  {
    {"Image", "NameCardImage"},
    {"Text", "NameCardText"},
  };
  private readonly Dictionary<string, Vector2> _nameCardRectTransformArgu = new()
  {
    {"referenceObjectPixels", new Vector2(300, 100)},
    {"archorMin", new Vector2(0.3869f, 0.8593f)},
    {"archorMax", new Vector2(0.3869f, 0.8593f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  private readonly Dictionary<string, Vector2> _nameCardTextRectTransformArgu = new()
  {
    {"referenceObjectPixels", new Vector2(300 * 0.9f, 100 * 0.9f)},
    {"archorMin", new Vector2(0.5f, 0.5f)},
    {"archorMax", new Vector2(0.5f, 0.5f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  private readonly Dictionary<string, string> _nameCardImageArgu = new()
  {
    {"gameObjectName", "NameCardImage"},
    {"resourcesFolderPath", RoleSelectScene.resourcesFolderPath},
  };
  private readonly Dictionary<string, List<float>> _nameCardTextArgu = new()
  {
    {"fontSize", new List<float>{30f}},
    {"color", new List<float>{0.0f, 0.0f, 0.0f, 1.0f}},
    {"alignmentOption", TextComponentBuilder.middle},
    {"font", TextComponentBuilder.kai},
  };
  private readonly Dictionary<string, bool> _nameCardContentsizeFitterArgu = new()
  {
    {"horizontalFit", false},
    {"verticalFit", false},
  };
  #endregion


  #region Introduction
  private readonly Dictionary<string, string> _introductionNameArgu = new()
  {
    {"Image", "IntroductionImage"},
    {"Text", "IntroductionText"},
  };
  private readonly Dictionary<string, Vector2> _introductionRectTransformArgu = new()
  {
    {"referenceObjectPixels", new Vector2(1080, 420)},
    {"archorMin", new Vector2(0.619f, 0.39f)},
    {"archorMax", new Vector2(0.619f, 0.39f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  private readonly Dictionary<string, Vector2> _introductionTextRectTransformArgu = new()
  {
    {"referenceObjectPixels", new Vector2(1080 * 0.9f, 420 * 0.9f)},
    {"archorMin", new Vector2(0.5f, 0.5f)},
    {"archorMax", new Vector2(0.5f, 0.5f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  private readonly Dictionary<string, string> _introductionImageArgu = new()
  {
    {"gameObjectName", "IntroductionImage"},
    {"resourcesFolderPath", RoleSelectScene.resourcesFolderPath},
  };
  private readonly Dictionary<string, List<float>> _introductionTextArgu = new()
  {
    {"fontSize", new List<float>{30f}},
    {"color", new List<float>{0.0f, 0.0f, 0.0f, 1.0f}},
    {"alignmentOption", TextComponentBuilder.middle},
    {"font", TextComponentBuilder.kai},
  };
  private readonly Dictionary<string, bool> _introductionContentsizeFitterArgu = new()
  {
    {"horizontalFit", false},
    {"verticalFit", false},
  };
  #endregion
  #endregion

  #region Fields
  public int RoleIdx { set; get; } = 0;
  public ImageGameObjectBuilder SelectBackgroundImage { set; get; }
  public ImageGameObjectBuilder RoleImageBuilder { get; set; }
  public TextBoxGameObjectBuilder NameCardBuilder { get; set; }
  public TextBoxGameObjectBuilder IntroductionBuilder { get; set; }
<<<<<<< HEAD
  public ButtonBoxGameObjectbuilder ReturnButtonBuilder { get; set; }
=======
  public ButtonUniversalbuilder ReturnButtonBuilder { get; set; }
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
  #endregion


  #region Methods
  public RoleIntroduction(Transform parentTransform, int roleIdx, UnityAction OnReturnButtonClick)
  {
    if (roleIdx < 0 || roleIdx >= SettingsInfo.roleNameList.Count)
    {
      if (DebugInfo.PrintDebugInfo)
      {
        Debug.LogError($"Invalid role index: {roleIdx}");
      }
      roleIdx = 0;
    }
    RoleIdx = roleIdx;
    
    // SeletctBackgroundImage =========================================================================
    SelectBackgroundImage = new ImageGameObjectBuilder("SelectBackground", parentTransform);
    SelectBackgroundImage.Build(
      new Dictionary<string, IComponentBuilder>
      {
        {"RectTransform", new RectTransformComponentBuilder(_selectBackgroundRectTransformArgu)},
        {"Image", new ImageComponentBuilder(_selectBackgroundImageArgu)}
      }
    );

    // RoleImage =====================================================================================
    if (roleIdx != -1) 
    {
      RenderRoleImage();
    }

    // NameCard =======================================================================================
    NameCardBuilder = new TextBoxGameObjectBuilder(_nameCardNameArgu, SelectBackgroundImage.Transform);
    NameCardBuilder.Build(
      new SortedDictionary<string, Dictionary<string, IComponentBuilder>>
      {
        {"Image", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(_nameCardRectTransformArgu)},
            {"Image", new ImageComponentBuilder(_nameCardImageArgu)},
          }
        },
        {"Text", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(_nameCardTextRectTransformArgu)},
            {"Text", new TextComponentBuilder(_nameCardTextArgu)},
            {"ContentSizeFitter", new ContentSizeFitterComponentBuilder(_nameCardContentsizeFitterArgu)},
          }
        }
      }
    );
    NameCardBuilder.ModifyContent(SettingsInfo.roleNameList[roleIdx]);

    // Introduction ===================================================================================
   IntroductionBuilder = new TextBoxGameObjectBuilder(_introductionNameArgu, SelectBackgroundImage.Transform);
    IntroductionBuilder.Build(
      new SortedDictionary<string, Dictionary<string, IComponentBuilder>>
      {
        {"Image", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(_introductionRectTransformArgu)},
            {"Image", new ImageComponentBuilder(_introductionImageArgu)},
          }
        },
        {"Text", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(_introductionTextRectTransformArgu)},
            {"Text", new TextComponentBuilder(_introductionTextArgu)},
            {"ContentSizeFitter", new ContentSizeFitterComponentBuilder(_introductionContentsizeFitterArgu)},
          }
        }
      }
    );
    IntroductionBuilder.ModifyContent(SettingsInfo.roleIntroductionList[roleIdx]);

    // ReturnButton =====================================================================================
<<<<<<< HEAD
    ReturnButtonBuilder = new ButtonBoxGameObjectbuilder(
      "ReturnImage", new Vector2(1.0f, 1.0f), parentTransform, new Dictionary<string, UnityAction>
      {
        {"返回", OnReturnButtonClick},
=======
    ReturnButtonBuilder = new ButtonUniversalbuilder(
      "ReturnImage", new Vector2(1.0f, 1.0f), SelectBackgroundImage.Transform, new Dictionary<string, UnityAction>
      {
        {"onClickAction", OnReturnButtonClick},
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
      }, 1
    );
  }
  
  public void RenderRoleImage()
  {
    if (RoleIdx < 0 || RoleIdx >= SettingsInfo.maxPlayerCount)
    {
      Debug.LogWarning($"RoleIdx: {RoleIdx} is out of range {SettingsInfo.maxPlayerCount}");
      RoleIdx = 0;
    }

    string roleSelectImageName = @"RoleImage" + (RoleIdx + 1).ToString();
    _roleImageArgu["gameObjectName"] += (RoleIdx + 1).ToString();

    RoleImageBuilder = new ImageGameObjectBuilder(roleSelectImageName, SelectBackgroundImage.Transform);
    RoleImageBuilder.Build(
      new Dictionary<string, IComponentBuilder>
      {
        {"RectTransform", new RectTransformComponentBuilder(_roleRectTransformArgu)},
        {"Image", new ImageComponentBuilder(_roleImageArgu)}
      }
    );
  } 

  public void SetActive(bool state)
  {
    SelectBackgroundImage.SetActive(state);
    RoleImageBuilder.SetActive(state);
    NameCardBuilder.SetActive(state);
    IntroductionBuilder.SetActive(state);
  }
  #endregion
}