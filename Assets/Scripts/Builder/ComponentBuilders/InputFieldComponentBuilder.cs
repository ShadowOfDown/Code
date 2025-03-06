using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InputFieldComponentBuilder : ComponentBuilder<List<float>>
{
  #region Fields
  public readonly static string componentType = "InputField";
  public readonly static HashSet<string> arguNameTable = new()
  {
    "pressedColor"
  };
  public readonly static Dictionary<string, List<float>> defaultArguTable = new()
  {
    {"pressedColor", new List<float>{0.5f, 0.5f, 0.5f, 1f}},
  };
  public readonly static string kaiFont = @"Fonts\kai";
  public readonly static string songFont = @"Fonts\Song";
  public readonly static List<float> kai = new() { 0.0f };
  public readonly static List<float> song = new() { 1.0f };
  public readonly static string defaultPlaceholderText = "Enter text";
  #endregion

  #region Properties
  public override bool Enable { get { return InputField.enabled; } set { InputField.enabled = value; } }
  public TMP_InputField InputField { get; set; }
  public string Content { get; set; }
  #endregion

  #region Methods
  public InputFieldComponentBuilder()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log($"Start to create {GetComponentType()}");
    }
  }

  public InputFieldComponentBuilder(Dictionary<string, List<float>> arguTable, string placeholderText = null)
  {
    ArguTable = arguTable ?? defaultArguTable;
    Content = placeholderText ?? defaultPlaceholderText;
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log($"Start to create {GetComponentType()}");
    }
  }

  public override string GetComponentType()
  {
    return componentType;
  }

  public override Component GetComponent()
  {
    return InputField;
  }

  public override HashSet<string> GetArguNameTable()
  {
    return arguNameTable;
  }

  public override Dictionary<string, List<float>> GetDefaultArguTable()
  {
    return defaultArguTable;
  }

  public override void Modify(Dictionary<string, List<float>> arguTable)
  {
    ArguTable = arguTable;
    InputField.text = Content ?? "input";
    InputField.transition = Selectable.Transition.ColorTint;
    Color pressedColor = arguTable.ContainsKey("pressedColor") & arguTable["pressedColor"] != null ? 
      TextComponentBuilder.GetColor(arguTable["pressedColor"]) : Color.gray; 
    InputField.colors = new ColorBlock()
    {
      normalColor = Color.white,
      highlightedColor = pressedColor,
      pressedColor = Color.white,
      disabledColor = pressedColor,
      selectedColor = Color.white,
      colorMultiplier = 1.0f,
      fadeDuration = 0.1f,
    };
  }

  public void Modify(TMP_InputField inputField)
  {
    InputField = inputField;
  }

  public void SetTransitionSprite(string imagePath)
  {
    Sprite pressedSprite = Resources.Load<Sprite>(imagePath);
    if (pressedSprite == null)
    {
      Debug.LogWarning("fail to load image" + imagePath);
      return;
    }

    InputField.transition = Selectable.Transition.SpriteSwap;
    InputField.spriteState = new SpriteState
    {
      highlightedSprite = null,
        pressedSprite = pressedSprite,
        disabledSprite = null
    };
  }

  public override void Build(GameObject gameObject)
  {
    if (gameObject.TryGetComponent(out TMP_InputField inputField))
    {
      InputField = inputField;
    }
    else
    {
      InputField = gameObject.AddComponent<TMP_InputField>();
    }

    Modify(ArguTable);

    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log($"Successfully load {componentType} in {gameObject.name}");
    }
  }

  public void ModifyText(RectTransform textViewPort, TextMeshProUGUI textComponent, TextMeshProUGUI placeholder)
  {
    InputField.textViewport = textViewPort;
    InputField.textComponent = textComponent;
    InputField.placeholder = placeholder;
    InputField.pointSize = 100;
  }

  // 返回文本输入的内容
  public string GetContent()
  {
    DebugInfo.Print("content: " + InputField.text);
    return InputField.text;
  }
  #endregion
}