using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class RoleImageBox : IInterfaceBuilder
{
  private ImageGameObjectBuilder _roleFrameBuilder;
  private ButtonGameObjectBuilder _roleImageBuilder;
  private TextBoxGameObjectBuilder _nameCardBuilder;
  private TextBoxGameObjectBuilder _playerNameBuilder;
  private int _roleIdx;

  public int RoleIdx => _roleIdx;

  public RoleImageBox(int roleIdx, Transform parentTransform)
  {
    if (roleIdx < 0 || roleIdx >= SettingsInfo.maxPlayerCount)
    {
      Debug.LogWarning($"roleIdx: {roleIdx} is out of range {SettingsInfo.maxPlayerCount}");
      roleIdx = 0;
    }

    _roleIdx = roleIdx;
    BuildRoleFrame(parentTransform, roleIdx);
    BuildRoleImage(roleIdx);
    BuildNameCard(roleIdx);
    BuildPlayerName(roleIdx);
  }

  private void BuildRoleFrame(Transform parentTransform, int roleIdx)
  {
    _roleFrameBuilder = new ImageGameObjectBuilder($"RoleImage\\{(roleIdx + 1).ToString()}Frame", parentTransform);
    _roleFrameBuilder.Build(new Dictionary<string, IComponentBuilder>
    {
      {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
      {
        {"referenceObjectPixels", new Vector2(240, 320)},
        {"archorMin", new Vector2(0.1f + 0.1f * (2 * (roleIdx % 4) + 1), 0.7f - 0.4f * (roleIdx / 4))},
        {"archorMax", new Vector2(0.1f + 0.1f * (2 * (roleIdx % 4) + 1), 0.7f - 0.4f * (roleIdx / 4))},
        {"pivotPos", new Vector2(0.5f, 0.5f)}
      })},
      {"Image", new ImageComponentBuilder(new Dictionary<string, string>
      {
        {"gameObjectName", "RoleFrameImage"},
        {"resourcesFolderPath", RoleSelectScene.resourcesFolderPath},
      })}
    });
  }

  private void BuildRoleImage(int roleIdx)
  {
    float roleRadio = 0.9f;
    _roleImageBuilder = new ButtonGameObjectBuilder($"RoleImage\\{(roleIdx + 1).ToString()}", _roleFrameBuilder.Transform);
    _roleImageBuilder.Build(new Dictionary<string, IComponentBuilder>
    {
      {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
      {
        {"referenceObjectPixels", new Vector2(240, 320) * roleRadio},
        {"archorMin", new Vector2(0.5f, 0.5f)},
        {"archorMax", new Vector2(0.5f, 0.5f)},
        {"pivotPos", new Vector2(0.5f, 0.5f)}
      })},
      {"Image", new ImageComponentBuilder(new Dictionary<string, string>
      {
        {"gameObjectName", $"RoleImage/{(roleIdx + 1).ToString()}"},
        {"resourcesFolderPath", RoleSelectScene.resourcesFolderPath},
      })},
      {"Button", new ButtonComponentBuilder(new Dictionary<string, string>
      {
        {"gameObjectName", $"RoleImage/{(roleIdx + 1).ToString()}"},
        {"resourcesFolderPath", RoleSelectScene.resourcesFolderPath},
      })},
      {"EventTrigger", new EventTriggerComponentBuilder(new Dictionary<string, UnityAction>
      {
        {"onClickAction", OnRoleImageClicked},
        {"onHoverAction", null},
      })}
    });
  }

  private void BuildNameCard(int roleIdx)
  {
    _nameCardBuilder = new TextBoxGameObjectBuilder(new Dictionary<string, string>
    {
      {"Image", "NameCardImage"},
      {"Text", "NameCardText"},
    }, _roleFrameBuilder.Transform);

    _nameCardBuilder.Build(
      new SortedDictionary<string, Dictionary<string, IComponentBuilder>>
      {
        {"Image", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
            {
              {"referenceObjectPixels", new Vector2(180, 60)},
              {"archorMin", new Vector2(0.5f, 1.0f)},
              {"archorMax", new Vector2(0.5f, 1.0f)},
              {"pivotPos", new Vector2(0.5f, 0.5f)}
            })},
            {"Image", new ImageComponentBuilder(new Dictionary<string, string>
            {
              {"gameObjectName", "NameCardImage"},
              {"resourcesFolderPath", RoleSelectScene.resourcesFolderPath},
            })}
          }
        },
        {"Text", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
            {
              {"referenceObjectPixels", new Vector2(180 * 0.9f, 60 * 0.9f)},
              {"archorMin", new Vector2(0.5f, 0.5f)},
              {"archorMax", new Vector2(0.5f, 0.5f)},
              {"pivotPos", new Vector2(0.5f, 0.5f)}
            })},
            {"Text", new TextComponentBuilder(new Dictionary<string, List<float>>
            {
              {"fontSize", new List<float>{30f}},
              {"color", new List<float>{0.0f, 0.0f, 0.0f, 1.0f}},
              {"alignmentOption", TextComponentBuilder.middle},
              {"font", TextComponentBuilder.kai},
            })},
            {"ContentSizeFitter", new ContentSizeFitterComponentBuilder(new Dictionary<string, bool>
            {
              {"horizontalFit", false},
              {"verticalFit", false},
            })}
          }
        }
      }
    );
    _nameCardBuilder.ModifyContent(SettingsInfo.roleNameList[roleIdx]);
  }

  private void BuildPlayerName(int roleIdx)
  {
    _playerNameBuilder = new TextBoxGameObjectBuilder(new Dictionary<string, string>
    {
      {"Text", "PlayerNameText"},
      {"Image", "PlayerNameImage"},
    }, _roleFrameBuilder.Transform);

    _playerNameBuilder.Build(
      new SortedDictionary<string, Dictionary<string, IComponentBuilder>>
      {
        {"Image", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
            {
              {"referenceObjectPixels", new Vector2(180, 60)},
              {"archorMin", new Vector2(0.8f, 0.0f)},
              {"archorMax", new Vector2(0.8f, 0.0f)},
              {"pivotPos", new Vector2(0.5f, 0.5f)}
            })},
            {"Image", new ImageComponentBuilder(new Dictionary<string, string>
            {
              {"gameObjectName", "NameCardImage"},
              {"resourcesFolderPath", RoleSelectScene.resourcesFolderPath},
            })}
          }
        },
        {"Text", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
            {
              {"referenceObjectPixels", new Vector2(180 * 0.9f, 60 * 0.9f)},
              {"archorMin", new Vector2(0.5f, 0.5f)},
              {"archorMax", new Vector2(0.5f, 0.5f)},
              {"pivotPos", new Vector2(0.5f, 0.5f)}
            })},
            {"Text", new TextComponentBuilder(new Dictionary<string, List<float>>
            {
              {"fontSize", new List<float>{30f}},
              {"color", new List<float>{0.0f, 0.0f, 0.0f, 1.0f}},
              {"alignmentOption", TextComponentBuilder.middle},
              {"font", TextComponentBuilder.kai},
            })},
            {"ContentSizeFitter", new ContentSizeFitterComponentBuilder(new Dictionary<string, bool>
            {
              {"horizontalFit", false},
              {"verticalFit", false},
            })}
          }
        }
      }
    );
    _playerNameBuilder.ModifyContent(SettingsInfo.playerNameList[roleIdx]);
  }

  public void SetActive(bool active)
  {
    _roleImageBuilder.SetActive(active);
    _nameCardBuilder.SetActive(active);
    _playerNameBuilder.SetActive(active);
  }

  public void OnRoleImageClicked()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log("OnCharacterClicked");
    }
    RoleSelectScene.UpdateInterface(1);
    RoleSelectScene.roleNum = RoleIdx;
  }
}
