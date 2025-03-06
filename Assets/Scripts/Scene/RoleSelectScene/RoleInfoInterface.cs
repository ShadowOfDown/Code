// 人物信息介绍界面
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class RoleInfoInterface
{
  #region Field
  #region RoleImageSelector
  private readonly Dictionary<string, string> _normalRoleImageArgu = new()
  {
    {"gameObjectName", "RoleImage/RoleImage_0"},
    {"resourcesFolderPath", RoleSelectScene.resourcesFolderPath},
  };
  private readonly Dictionary<string, string> _pressedRoleImageArgu = new()
  {
    {"gameObjectName", null},
    {"resourcesFolderPath", RoleSelectScene.resourcesFolderPath},
  };
  private readonly Dictionary<string, Vector2> _roleRectTransformArgu = new()
  {
    {"referenceObjectPixels", new Vector2(240, 320)},
    {"archorMin", new Vector2(-1, -1)},
    {"archorMax", new Vector2(-1, -1)},
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
    {"referenceObjectPixels", new Vector2(180, 60)},
    {"archorMin", new Vector2(0.5f, 1.0f)},
    {"archorMax", new Vector2(0.5f, 1.0f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  private readonly Dictionary<string, Vector2> _nameCardTextRectTransformArgu = new()
  {
    {"referenceObjectPixels", new Vector2(180 * 0.9f, 60 * 0.9f)},
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


  #region PlayerName
  private readonly Dictionary<string, string> _playerNameNameArgu = new()
  {
    {"Text", "PlayerNameText"},
    {"Image", "PlayerNameImage"},
  };
  private const string NameCardImageName = @"NameCardImage";
  private readonly List<string> _playerNameList = new()
  {
    @"name_01",
    @"name_02",
    @"name_03",
    @"name_04",
    @"name_05",
    @"name_06",
    @"name_07",
    @"name_08",
  };
  private readonly Dictionary<string, Vector2> _playerImageRectTransformArgu = new()
  {
    {"referenceObjectPixels", new Vector2(180, 60)},
    {"archorMin", new Vector2(0.8f, 0.0f)},
    {"archorMax", new Vector2(0.8f, 0.0f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  #endregion
  #endregion

  #region Property
  public int RoleIdx { get; }
  public ButtonGameObjectBuilder RoleImageBuilder { get; set; }
  public TextBoxGameObjectBuilder NameCardBuilder { get; set; }
  public TextBoxGameObjectBuilder PlayerNameBuilder { get; set; }
  #endregion


  #region Method
  public RoleInfoInterface(int roleIdx, Transform parentTransform, UnityAction OnRoleImageClicked)
  {
    if (roleIdx < 0 || roleIdx >= SettingsInfo.maxPlayerCount)
    {
      Debug.LogWarning($"roleIdx: {roleIdx} is out of range {SettingsInfo.maxPlayerCount}");
      roleIdx = 0;
    }

    RoleIdx = roleIdx;
    string roleSelectImageName = @"RoleImage" + (roleIdx + 1).ToString();
    _normalRoleImageArgu["gameObjectName"] += (roleIdx + 1).ToString();
    
    _roleRectTransformArgu["archorMin"] = _roleRectTransformArgu["archorMax"] =
      new Vector2(0.1f + 0.1f * (2 * (roleIdx % 4) + 1), 0.7f - 0.4f * (roleIdx / 4));

    // roleImage ======================================================================================
    RoleImageBuilder = new ButtonGameObjectBuilder(roleSelectImageName, parentTransform);
    RoleImageBuilder.Build(new Dictionary<string, IComponentBuilder>
    {
      {"RectTransform", new RectTransformComponentBuilder(_roleRectTransformArgu)},
      {"Image", new ImageComponentBuilder(_normalRoleImageArgu)},
      {"Button", new ButtonComponentBuilder(_pressedRoleImageArgu)},
      {"EventTrigger", new EventTriggerComponentBuilder(new Dictionary<string, UnityAction>
        {
          {"onClickAction", OnRoleImageClicked},
          {"onHoverAction", null},
        }
      )}
    });

    Debug.Log("1");

    // nameCard =======================================================================================
    NameCardBuilder = new TextBoxGameObjectBuilder(_nameCardNameArgu, RoleImageBuilder.Transform);
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

    // playerName =====================================================================================
    PlayerNameBuilder = new TextBoxGameObjectBuilder(_playerNameNameArgu, RoleImageBuilder.Transform);
    PlayerNameBuilder.Build(
      new SortedDictionary<string, Dictionary<string, IComponentBuilder>>
      {
        {"Image", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(_playerImageRectTransformArgu)},
            {"Image", new ImageComponentBuilder(_nameCardImageArgu)},
          }
        },
        {
          "Text", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(_nameCardTextRectTransformArgu)},
            {"Text", new TextComponentBuilder(_nameCardTextArgu)},
            {"ContentSizeFitter", new ContentSizeFitterComponentBuilder(_nameCardContentsizeFitterArgu)},
          }
        }
      }
    );
    PlayerNameBuilder.ModifyContent(SettingsInfo.playerNameList[roleIdx]);
  }

  public void SetActive(bool active)
  {
    RoleImageBuilder.SetActive(active);
    NameCardBuilder.SetActive(active);
    PlayerNameBuilder.SetActive(active);
  }
  #endregion
}