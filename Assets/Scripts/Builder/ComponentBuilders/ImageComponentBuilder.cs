using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ImageComponentBuilder : ComponentBuilder<string>
{
  # region Fields
  public readonly static string componentType = "Image";
  public readonly static HashSet<string> arguNameTable = new()
  {
    "gameObjectName",
    "resourcesFolderPath",
  };
  public readonly static Dictionary<string, string> defaultArguTable = new()
  {
    {"gameObjectName", "DefaultImage"},
    {"resourcesFolderPath", "DefaultImage/"},
  };
  #endregion


  # region Properities
  public override bool Enable { get { return Image.enabled; } set { Image.enabled = value; } }
  public Image Image { get; set; }
  public Texture2D Texture
  {
    get { return Image.sprite.texture; }
  }
  public Sprite Sprite
  {
    get
    {
      return Image.sprite;
    }
    set
    {
      Image.sprite = value;
    }
  }
  public bool PreserveAspect 
  { 
    get 
    {
      return Image.preserveAspect;
    } 
    set
    {
      Image.preserveAspect = value;
    } 
  }
  #endregion


  # region Methods
  public ImageComponentBuilder()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log($"start to create {GetComponentType()}");
    }
  }
  public ImageComponentBuilder(Dictionary<string, string> arguTable)
  {
    ArguTable = arguTable;
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
    return Image;
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
    if (!arguTable.ContainsKey("gameObjectName") && arguTable.ContainsKey("resourcesFolderPath"))
    {
      Debug.LogWarning("gameObjectName is not set, but resourcesFolderPath is set, please check the arguTable");
      return;
    }
    else if (arguTable["gameObjectName"] != null && arguTable["resourcesFolderPath"] != null)
    {
      Image.sprite = Resources.Load<Sprite>(arguTable["resourcesFolderPath"] + arguTable["gameObjectName"]);
      Image.preserveAspect = true;
    }
    else 
    {
      Image.color = new Color(1, 1, 1, 1.0f / 255.0f);
    }
  }

  public void Modify(Image image)
  {
    Image = image;
  }

  public override void Build(GameObject gameObject)
  {
    if (gameObject.TryGetComponent(out Image image))
    {
      Image = image;
    }
    else
    {
      Image = gameObject.AddComponent<Image>();
    }

    Modify(ArguTable);

    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log($"successfully load {componentType} in {gameObject.name}");
    }
  }

  public void ModifyColor(Color color)
  {
    if (Image == null)
    {
      Debug.LogWarning("Image is null, please build the component first");
      return;
    }
    Image.color = color;
  }

  public void ModifyColor(float r, float g, float b, float a)
  {
    if (Image == null)
    {
      Debug.LogWarning("Image is null, please build the component first");
      return;
    }

    Image.color = new Color(r, g, b, a);
  }
  #endregion
}