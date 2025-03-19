using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class RoleSelectScene : UIObject
{
  #region Fields
  public static readonly string resourcesFolderPath = "Arts/Textures/RoleSelectScene/";
  private bool _showIntroduction = false;
  SelectBoxUniversalBuilder _confirmSelectBox;
  PromptBoxUniversalBuilder _promptSelectBox;
  private readonly Dictionary<string, Vector2> _fullScreenRectTransformArgu = new()
  {
    {"referenceObjectPixels", PixelInfo.referenceScreenPixel},
    {"archorMin", new Vector2(0.5f, 0.5f)},
    {"archorMax", new Vector2(0.5f, 0.5f)},
    {"objectPos", new Vector2(0.5f, 0.5f)},
    {"pivotPos", new Vector2(0.5f, 0.5f)},
  };
  private CanvasGameObjectBuilder _canvas = null;
  private ImageGameObjectBuilder _backgroundImage = null;
  private readonly Dictionary<string, string> _backgroundImageArgu = new()
  {
    {"gameObjectName", "BackgroundImage"},
    {"resourcesFolderPath", resourcesFolderPath},
  };
  private readonly List<RoleInfoInterface> roleInfoBoxList = new();
  private RoleIntroduction _roleDetailInterface = null;

    #endregion

    #region Properties
    #endregion


    #region Methods

    public override void OnLoad()
    {
        _backgroundImage = new ImageGameObjectBuilder("BackgroundImage", this.transform);
        _backgroundImage.Build(
          new Dictionary<string, IComponentBuilder>
          {
        {"RectTransform", new RectTransformComponentBuilder(_fullScreenRectTransformArgu)},
        {"Image", new ImageComponentBuilder(_backgroundImageArgu)}
          }
        );

        // roleInfoBox ====================================================================================
        for (int roleNum = 0; roleNum < 8; roleNum++)
        {
            roleInfoBoxList.Add(new RoleInfoInterface(roleNum, _backgroundImage.Transform, OnRoleImageClicked));
        }

        // SelectInterface ================================================================================
        _roleDetailInterface = new RoleIntroduction(_backgroundImage.Transform, 0, OnReturnButtonClicked);

        // SelectionConfirmBox ============================================================================
        _confirmSelectBox = new SelectBoxUniversalBuilder(
          "PromptBox_01", new Vector2(0.5f, 0.18f), _backgroundImage.Transform, new SortedDictionary<string, UnityAction>
          {
        {"comfirm", OnConfirmButtonClicked},
        {"wait", OnReturnButtonClicked}
          });
        _confirmSelectBox.ModifyContent("whether to choose?");

        // PromptSelectBox ================================================================================
        _promptSelectBox = new PromptBoxUniversalBuilder(
          "PromptBox_02", new Vector2(0.5f, 0.5f), _backgroundImage.Transform);
        _promptSelectBox.ModifyContent("has been chosen");
        _promptSelectBox.SetActive(false);
    }
    public override void OnUpdate()
    {
        SetActive();
    }



    public void SetActive()
    {
        // 展示角色
        foreach (var roleInfoBox in roleInfoBoxList)
        {
            roleInfoBox.SetActive(!_showIntroduction);
        }
        // 人物信息
        _roleDetailInterface.SetActive(_showIntroduction);
        // 提示框
        _confirmSelectBox.SetActive(_showIntroduction);
    }

    // Buttons ==========================================================================================
    public void OnRoleImageClicked()
    {
        if (DebugInfo.PrintDebugInfo)
        {
            _showIntroduction = true;
            Debug.Log("OnCharacterClicked");
        }
    }

    public void OnReturnButtonClicked()
    {
        if (DebugInfo.PrintDebugInfo)
        {
            _showIntroduction = false;
            Debug.Log("OnReturnButtonClicked");
        }
    }

    public void OnConfirmButtonClicked()
    {
        if (DebugInfo.PrintDebugInfo)
        {
            Debug.Log("OnConfirmButtonClicked");
        }
        _promptSelectBox.SetActive(true);
    }
    #endregion
}