// 勾选框
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public class CheckBoxUniversalBuilder : UniversalGameObjectBuilder
{
  #region Fields
  public static readonly Vector2 referencePixel = new(100, 100);
  private ButtonGameObjectBuilder _checkBoxButton = null;
  private UnityAction _checkedAction = null;
  private Sprite _checkSprite = null;
  private Sprite _normalSprite = null;
  #endregion


  #region Properties
  public override string Type { get; }
  public override string GameObjectName { get; }
  public override Transform Transform { get; }
  public override RectTransform RectTransform { get; }
  public bool Checked { get; set; } = false;
  #endregion


  #region Methods
  public CheckBoxUniversalBuilder(string checkName, Vector2 imagePos, Transform parentTransform, float scaleRadio = 1.0f, UnityAction checkedAction = null)
  {
    _checkSprite = Resources.Load<Sprite>(resourcesFolderPath + "checkSpriteImage");
    _checkedAction = checkedAction;

    _checkBoxButton = new ButtonGameObjectBuilder(checkName, parentTransform);
    _checkBoxButton.Build(new Dictionary<string, IComponentBuilder>
    {
      {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
        {
          {"referenceObjectPixels", referencePixel * scaleRadio},
          {"archorMin", imagePos},
          {"archorMax", imagePos},
          {"pivotPos", new Vector2(0.5f, 0.5f)},
        }
      )},
      {"Image", new ImageComponentBuilder(new Dictionary<string, string>
        {
          {"gameObjectName", "CheckBoxImage"},
          {"resourcesFolderPath", resourcesFolderPath},
        })
      },
      {"Button", new ButtonComponentBuilder(new Dictionary<string, string>
        {
          {"gameObjectName", null},
          {"resourcesFolderPath", null},
        })
      },
      {"EventTrigger", new EventTriggerComponentBuilder(new Dictionary<string, UnityAction>
        {
          {"onClickAction", OnClicked},
          {"onHoverAction", null},
        }
      )}
    });

    _normalSprite = _checkBoxButton.Sprite;
  }

  public override void SetActive(bool state)
  {
    _checkBoxButton.SetActive(state);
  }

  private void OnClicked()
  {
    DebugInfo.Print("check box clicked from " + Checked);
    _checkedAction?.Invoke();

    if (Checked == true)
    {
      Checked = false;
      _checkBoxButton.Sprite = _normalSprite;
    }
    else
    {
      Checked = true;
      _checkBoxButton.Sprite = _checkSprite;
    }
  }
  #endregion
}