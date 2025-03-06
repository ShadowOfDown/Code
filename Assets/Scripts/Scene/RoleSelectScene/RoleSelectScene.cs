using UnityEngine;
using System.Collections.Generic;
<<<<<<< HEAD
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
=======
using UnityEditor.Search;
using UnityEngine.Events;

public class RoleSelectScene : MonoBehaviour
{
  #region Fields
  public static readonly string resourcesFolderPath = "Arts/Textures/RoleSelectScene/";
  private bool _showIntroduction = false;
  SelectBoxUniversalBuilder _confirmSelectBox;
  PromptBoxUniversalBuilder _promptSelectBox;
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
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
<<<<<<< HEAD
  private readonly List<RoleInfoBox> roleInfoBoxList = new();
=======
  private readonly List<RoleInfoInterface> roleInfoBoxList = new();
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
  private RoleIntroduction _roleDetailInterface = null;
  #endregion

  #region Properties
  #endregion


  #region Methods
<<<<<<< HEAD
  public RoleSelectScene(SceneStateControl control) : base(control)
  {
    this.StateName = "RoleSelectScene";
  }

  public override void StateBegin()
=======

  public void Start()
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
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
<<<<<<< HEAD
      roleInfoBoxList.Add(new RoleInfoBox(roleNum, _backgroundImage.Transform, OnRoleImageClicked));
=======
      roleInfoBoxList.Add(new RoleInfoInterface(roleNum, _backgroundImage.Transform, OnRoleImageClicked));
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
    }

    // SelectInterface ================================================================================
    _roleDetailInterface = new RoleIntroduction(_backgroundImage.Transform, 0, OnReturnButtonClicked);
  
    // SelectionConfirmBox ============================================================================
<<<<<<< HEAD
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
=======
    _confirmSelectBox= new SelectBoxUniversalBuilder(
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
  public void Update()
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
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
<<<<<<< HEAD
    _promptSelectBox.SetActive(_showIntroduction & _showPromptBox);
  }

  public override void StateEnd()
  {

=======
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
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
<<<<<<< HEAD
    _showPromptBox = true;
  }

  public void OnPromptButtonClicked()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log("OnPromptButtonClicked");
    }
    _showPromptBox = false;
=======
    _promptSelectBox.SetActive(true);
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
  }
  #endregion
}