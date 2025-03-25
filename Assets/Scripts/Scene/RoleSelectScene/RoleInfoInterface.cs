// 人物介绍框
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

public class RoleInfoInterface : IInterfaceBuilder
{
  private int _roleNum;
  private ImageGameObjectBuilder _selectBackgroundImage;
  private ImageGameObjectBuilder _roleFrameBuilder;
  private ImageGameObjectBuilder _roleImageBuilder;
  private TextBoxGameObjectBuilder _nameCardBuilder;
  private TextBoxGameObjectBuilder _introductionBuilder;
  private SelectBoxUniversalBuilder _confirmSelectBuilder;
  private PromptBoxUniversalBuilder _promptBoxBuilder;

  public int RoleIdx { get; set; } = 0;

  public RoleInfoInterface(Transform parentTransform, int roleIdx)
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

    BuildSelectBackgroundImage(parentTransform);
    BuildRoleImage();
    BuildNameCard();
    BuildIntroduction();
    BuildConfirmSelectBox(parentTransform);
    BuildPromptBox(parentTransform);
  }

  private void BuildSelectBackgroundImage(Transform parentTransform)
  {
    _selectBackgroundImage = new ImageGameObjectBuilder("SelectBackground", parentTransform);
    _selectBackgroundImage.Build(new Dictionary<string, IComponentBuilder>
    {
      {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
      {
        {"referenceObjectPixels", new Vector2(1680, 640)},
        {"archorMin", new Vector2(0.5f, 0.6f)},
        {"archorMax", new Vector2(0.5f, 0.6f)},
        {"objectPos", new Vector2(0.5f, 0.5f)},
        {"pivotPos", new Vector2(0.5f, 0.5f)},
      })},
      {"Image", new ImageComponentBuilder(new Dictionary<string, string>
      {
        {"gameObjectName", "SelectBackgroundImage"},
        {"resourcesFolderPath", RoleSelectScene.resourcesFolderPath},
      })},
    });
  }

  private void BuildRoleImage()
  {
    if (RoleIdx < 0 || RoleIdx >= SettingsInfo.maxPlayerCount)
    {
      Debug.LogWarning($"roleIdx: {RoleIdx} is out of range {SettingsInfo.maxPlayerCount}");
      RoleIdx = 0;
    }

    string roleSelectImageName = $"RoleImage\\RoleImage_{RoleIdx + 1}";
    Dictionary<string, string> characterImageArgu = new()
    {
      {"gameObjectName", roleSelectImageName},
      {"resourcesFolderPath", RoleSelectScene.resourcesFolderPath},
    };

    _roleFrameBuilder = new ImageGameObjectBuilder(roleSelectImageName + "Frame", _selectBackgroundImage.Transform);
    _roleFrameBuilder.Build(new Dictionary<string, IComponentBuilder>
    {
      {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
      {
        {"referenceObjectPixels", new Vector2(420, 560)},
        {"archorMin", new Vector2(0.1488f, 0.5f)},
        {"archorMax", new Vector2(0.1488f, 0.5f)},
        {"pivotPos", new Vector2(0.5f, 0.5f)},
      })},
      {"Image", new ImageComponentBuilder(new Dictionary<string, string>
      {
        {"gameObjectName", "RoleFrameImage"},
        {"resourcesFolderPath", RoleSelectScene.resourcesFolderPath},
      })},
    });

    float roleRatio = 0.9f;
    _roleImageBuilder = new ImageGameObjectBuilder(roleSelectImageName + "Image", _roleFrameBuilder.Transform);
    _roleImageBuilder.Build(new Dictionary<string, IComponentBuilder>
    {
      {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
      {
        {"referenceObjectPixels", new Vector2(420, 560) * roleRatio},
        {"archorMin", new Vector2(0.5f, 0.5f)},
        {"archorMax", new Vector2(0.5f, 0.5f)},
        {"pivotPos", new Vector2(0.5f, 0.5f)},
      })},
      {"Image", new ImageComponentBuilder(characterImageArgu)},
    });
  }

  private void BuildNameCard()
  {
    _nameCardBuilder = new TextBoxGameObjectBuilder(new Dictionary<string, string>
    {
      {"Image", "NameCardImage"},
      {"Text", "NameCardText"},
    }, _selectBackgroundImage.Transform);
    _nameCardBuilder.Build(new SortedDictionary<string, Dictionary<string, IComponentBuilder>>
    {
      {"Image", new Dictionary<string, IComponentBuilder>
      {
        {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
        {
          {"referenceObjectPixels", new Vector2(300, 100)},
          {"archorMin", new Vector2(0.3869f, 0.8593f)},
          {"archorMax", new Vector2(0.3869f, 0.8593f)},
          {"pivotPos", new Vector2(0.5f, 0.5f)},
        })},
        {"Image", new ImageComponentBuilder(new Dictionary<string, string>
        {
          {"gameObjectName", "NameCardImage"},
          {"resourcesFolderPath", RoleSelectScene.resourcesFolderPath},
        })},
      }},
      {"Text", new Dictionary<string, IComponentBuilder>
      {
        {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
        {
          {"referenceObjectPixels", new Vector2(300 * 0.9f, 100 * 0.9f)},
          {"archorMin", new Vector2(0.5f, 0.5f)},
          {"archorMax", new Vector2(0.5f, 0.5f)},
          {"pivotPos", new Vector2(0.5f, 0.5f)},
        })},
        {"Text", new TextComponentBuilder(TextBoxUniversalBuilder.textArgu_02)},
        {"ContentSizeFitter", new ContentSizeFitterComponentBuilder(new Dictionary<string, bool>
        {
          {"horizontalFit", false},
          {"verticalFit", false},
        })},
      }},
    });
    _nameCardBuilder.ModifyContent(SettingsInfo.roleNameList[RoleIdx]);
  }

  private void BuildIntroduction()
  {
    _introductionBuilder = new TextBoxGameObjectBuilder(new Dictionary<string, string>
    {
      {"Image", "IntroductionImage"},
      {"Text", "IntroductionText"},
    }, _selectBackgroundImage.Transform);
    _introductionBuilder.Build(new SortedDictionary<string, Dictionary<string, IComponentBuilder>>
    {
      {"Image", new Dictionary<string, IComponentBuilder>
      {
        {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
        {
          {"referenceObjectPixels", new Vector2(1080, 420)},
          {"archorMin", new Vector2(0.619f, 0.39f)},
          {"archorMax", new Vector2(0.619f, 0.39f)},
          {"pivotPos", new Vector2(0.5f, 0.5f)},
        })},
        {"Image", new ImageComponentBuilder(new Dictionary<string, string>
        {
          {"gameObjectName", "IntroductionImage"},
          {"resourcesFolderPath", RoleSelectScene.resourcesFolderPath},
        })},
      }},
      {"Text", new Dictionary<string, IComponentBuilder>
      {
        {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
        {
          {"referenceObjectPixels", new Vector2(1080 * 0.9f, 420 * 0.9f)},
          {"archorMin", new Vector2(0.5f, 0.5f)},
          {"archorMax", new Vector2(0.5f, 0.5f)},
          {"pivotPos", new Vector2(0.5f, 0.5f)},
        })},
        {"Text", new TextComponentBuilder(TextBoxUniversalBuilder.textArgu_02)},
        {"ContentSizeFitter", new ContentSizeFitterComponentBuilder(new Dictionary<string, bool>
        {
          {"horizontalFit", false},
          {"verticalFit", false},
        })},
      }},
    });
    _introductionBuilder.ImageBuilder.ModifyColor(88f / 255f, 44f / 255f, 11f / 255f, 1.0f);
    _introductionBuilder.ModifyContent(SettingsInfo.roleIntroductionList[RoleIdx]);
  }

  private void BuildConfirmSelectBox(Transform parentTransform)
  {
    _confirmSelectBuilder = new SelectBoxUniversalBuilder(
      "PromptBox_01", new Vector2(0.5f, 0.18f), parentTransform, new SortedDictionary<string, UnityAction>
      {
        {"confirm", OnConfirmButtonClicked},
        {"wait", OnReturnButtonClicked}
      });
    _confirmSelectBuilder.ModifyContent("是否选择？");
  }

  private void BuildPromptBox(Transform parentTransform)
  {
    _promptBoxBuilder = new PromptBoxUniversalBuilder(
      "PromptBox_02", new Vector2(0.5f, 0.5f), parentTransform);
    _promptBoxBuilder.ModifyContent("已被选择");
    _promptBoxBuilder.SetActive(false);
  }

  public void ModifyCharacter(int roleIdx)
  {
    if (_roleNum == roleIdx)
    {
      return;
    }

    if (roleIdx < 1 || roleIdx > SettingsInfo.maxPlayerCount)
    {
      Debug.LogWarning($"roleIdx: {roleIdx} is out of range {SettingsInfo.maxPlayerCount}");
      roleIdx = 1;
    }

    string roleImageName = $"RoleImage/{(roleIdx + 1).ToString()}";
    _roleImageBuilder.Image.Sprite = Resources.Load<Sprite>(RoleSelectScene.resourcesFolderPath + roleImageName);
  }

  public void SetActive(bool state)
  {
    _selectBackgroundImage.SetActive(state);
    _roleFrameBuilder.SetActive(state);
    _roleImageBuilder.SetActive(state);
    _nameCardBuilder.SetActive(state);
    _introductionBuilder.SetActive(state);
    _confirmSelectBuilder.SetActive(state);
    _promptBoxBuilder.SetActive(state);
  }

  // Buttons ==========================================================================================
  public void OnConfirmButtonClicked()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log("确认按钮被点击");
    }
    _promptBoxBuilder.IsActive = true;
  }

  public void OnReturnButtonClicked()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log("返回按钮被点击");
    }
    RoleSelectScene.UpdateInterface(0);
  }
}