using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TextComponentBuilder : ComponentBuilder<List<float>>
{
  #region Fields
  public override bool Enable { get { return Text.enabled; } set { Text.enabled = value; } }
  public readonly static string componentType = "Image";
  public readonly static HashSet<string> arguNameTable = new()
  {
    "fontSize",
    "color",
    "alignmentOption",
    "font",
  };
  public readonly static Dictionary<string, List<float>> defaultArguTable = new()
  {
    {"fontSize", new List<float>{14f}},
    {"color", new List<float>{1.0f, 1.0f, 1.0f, 1.0f}},
    {"alignmentOption", new List<float>{0f}},
    {"font", new List<float> {0f}},
  };
  public readonly static string kaiFont = @"Fonts\kai";
  public readonly static string songFont = @"Fonts\Song";
  public readonly static List<float> kai = new() { 0.0f };
  public readonly static List<float> song = new() { 1.0f };
  public readonly static string defaultContent = "hello world!";
  public readonly static List<float> left = new() { 0.0f };
  public readonly static List<float> middle = new() { 1.0f };
  #endregion


  #region Properities
  public TextMeshProUGUI Text { get; set; }
  public Vector2 ReferencePixel { get; set; }
  public string Content { get; set; }
  # endregion


  #region Methods
  public TextComponentBuilder()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log($"start to create {GetComponentType()}");
    }
  }
  public TextComponentBuilder(Dictionary<string, List<float>> arguTable, string content = null)
  {
    ArguTable = arguTable ?? defaultArguTable;
    Content = content;
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
    return Text;
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
    Text.text = Content;
    Text.fontSize = arguTable["fontSize"] == null || arguTable["fontSize"].Count == 0 ?
      defaultArguTable["fontSize"][0] : arguTable["fontSize"][0];
    Text.color = GetColor(arguTable["color"]);
    Text.alignment = GetAlignmentOption(arguTable["alignmentOption"]);

    Text.enableWordWrapping = true;
    Text.overflowMode = TextOverflowModes.Overflow;
    Text.font = GetFont(arguTable["font"]);
  }

  public void Modify(TextMeshProUGUI text)
  {
    Text = text;
  }

  public void ModifyContent(string content)
  {
    Content = Text.text = content;
  }

  public override void Build(GameObject gameObject)
  {
    if (gameObject.TryGetComponent(out TextMeshProUGUI text))
    {
      Text = text;
    }
    else
    {
      Text = gameObject.AddComponent<TextMeshProUGUI>();
    }

    Modify(ArguTable);

    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log($"successfully load {componentType} in {gameObject.name}");
    }
  }

  public static TMP_FontAsset GetFont(List<float> list)
  {
    if (list == null || list.Count == 0)
    {
      Debug.LogError($"invalid font num with {(list == null ? -1 : list.Count)} argu");
    }

    return Resources.Load<TMP_FontAsset>(list[0] == kai[0] ? songFont : kaiFont);
  }

  public static float GetFontSize(List<float> list)
  {
    if (list == null || list.Count == 0)
    {
      Debug.LogWarning("invalid fontsize");
      list = defaultArguTable["fontSize"];
    }

    return list[0];
  }

  public static Color GetColor(List<float> list)
  {
    if (list == null || list.Count < 3)
    {
      Debug.LogError($"invalid rgbd with {(list == null ? -1 : list.Count)} argu");
      list = defaultArguTable["Color"];
    }

    return new Color(list[0], list[1], list[2], list.Count == 3 ? 1.0f : list[3]);
  }

  public static TextAlignmentOptions GetAlignmentOption(List<float> list)
  {
    if (list == null || list.Count == 0)
    {
      Debug.LogError($"invalid alignment num with {(list == null ? -1 : list.Count)} argu");
      list = defaultArguTable["alignmentOption"];
    }

    return list[0] == left[0] ? TextAlignmentOptions.Left : TextAlignmentOptions.Center;
  }
  # endregion
}