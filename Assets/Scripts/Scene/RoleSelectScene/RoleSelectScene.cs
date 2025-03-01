using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class RoleSelectScene : ISceneState
{
  #region Fields
  private string my_state_name = "RoleSelectScene";
  public static readonly string resourcesFolderPath = "Arts/Textures/RoleSelectScene/";
  private bool _showIntroduction = false;
  private bool _showPromptBox = false;
  SelectBoxGameObjectBuilder _confirmSelectBox;
  SelectBoxGameObjectBuilder _promptSelectBox;
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
  private readonly List<RoleInfoBox> roleInfoBoxList = new();
  private RoleIntroduction _roleDetailInterface = null;
  #endregion

  #region Properties
  #endregion


  #region Methods
  public RoleSelectScene(SceneStateControl control) : base(control)
  {
    this.StateName = "RoleSelectScene";
  }

  public override void StateBegin()
  {
    // Canvas =========================================================================================
    _canvas = new CanvasGameObjectBuilder();
    _canvas.Build(
      new Dictionary<string, IComponentBuilder>
      {
        {"RectTransform", new RectTransformComponentBuilder(_fullScreenRectTransformArgu)},
      }
    );

    // Background =====================================================================================
    _backgroundImage = new ImageGameObjectBuilder("BackgroundImage", _canvas.Transform);
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
      roleInfoBoxList.Add(new RoleInfoBox(roleNum, _backgroundImage.Transform, OnRoleImageClicked));
    }

    // SelectInterface ================================================================================
    _roleDetailInterface = new RoleIntroduction(_backgroundImage.Transform, 0, OnReturnButtonClicked);
  
    // SelectionConfirmBox ============================================================================
    _confirmSelectBox= new SelectBoxGameObjectBuilder(
      "PromptBox_01", new Vector2(0.5f, 0.18f), _backgroundImage.Transform, new SortedDictionary<string, UnityAction>
      {
        {"确定", OnConfirmButtonClicked},
        {"再想想", OnReturnButtonClicked}
      });
    _confirmSelectBox.ModifyContent("是否选择?");

    // PromptSelectBox ================================================================================
    _promptSelectBox = new SelectBoxGameObjectBuilder(
      "PromptBox_02", new Vector2(0.5f, 0.5f), _backgroundImage.Transform, new SortedDictionary<string, UnityAction>
      {
        {"确定", OnPromptButtonClicked},
      });
    _promptSelectBox.ModifyContent("该角色已经被选择");
  }
  public override void StateUpdate()
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
    _promptSelectBox.SetActive(_showIntroduction & _showPromptBox);
  }

  public override void StateEnd()
  {

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
    _showPromptBox = true;
  }

  public void OnPromptButtonClicked()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log("OnPromptButtonClicked");
    }
    _showPromptBox = false;
  }
  #endregion
}