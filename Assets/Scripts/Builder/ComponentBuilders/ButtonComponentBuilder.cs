using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonComponentBuilder : ComponentBuilder<string>
{
  #region Fields
  public readonly static string componentType = "Button";
  public readonly static HashSet<string> arguNameTable = new()
  {
    "gameObjectName",
    "resourcesFolderPath",
  };
  public readonly static Dictionary<string, string> defaultArguTable = new()
  {
    {"gameObjectName", null},
    {"resourcesFolderPath", null},
  };
  #endregion


  #region Properities
  public override bool Enable { get { return Button.enabled; } set { Button.enabled = value; } }
  public Button Button { get; set; }
  public Sprite PressedSprite { get; set; }
  #endregion


  #region Methods
  public ButtonComponentBuilder()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log($"start to create {GetComponentType()}");
    }
  }
  public ButtonComponentBuilder(Dictionary<string, string> arguTable)
  {
    ArguTable = arguTable ?? defaultArguTable;
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log($"start to create {GetComponentType()}");
    }
  }

  public override string GetComponentType()
  {
    return componentType;
  }

  public override Component GetComponent()
  {
    return Button;
  }

  public override HashSet<string> GetArguNameTable()
  {
    return arguNameTable;
  }

  public override Dictionary<string, string> GetDefaultArguTable()
  {
    return defaultArguTable;
  }

  public override void Modify(Dictionary<string, string> arguTable)
  {
    if (arguTable.ContainsKey("gameObjectName") == false)
    {
      Button.transition = Selectable.Transition.None;
    }
    else if (arguTable["gameObjectName"] != null)
    {
      PressedSprite = Resources.Load<Sprite>(arguTable["resourcesFolderPath"] + arguTable["gameObjectName"]);

      Button.transition = Selectable.Transition.SpriteSwap;
      Button.spriteState = new SpriteState
      {
        highlightedSprite = null,
        pressedSprite = PressedSprite,
        disabledSprite = null
      };
    }
    else
    {
      PressedSprite = null;
      Button.colors = new ColorBlock()
      {
        normalColor = Color.white,
        highlightedColor = Color.gray,
        pressedColor = Color.white,
        disabledColor = Color.gray,
        selectedColor = Color.white,
        colorMultiplier = 1.0f,
        fadeDuration = 0.1f,
      };

    }
  }

  public void Modify(Button button)
  {
    Button = button;
  }

  public override void Build(GameObject gameObject)
  {
    if (gameObject.TryGetComponent(out Button button))
    {
      Button = button;
    }
    else
    {
      Button = gameObject.AddComponent<Button>();
    }

    Modify(ArguTable);

    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log($"successfully load {componentType} in {gameObject.name}");
    }
  }
  #endregion
}