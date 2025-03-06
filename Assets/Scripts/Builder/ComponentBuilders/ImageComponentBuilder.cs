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
<<<<<<< HEAD
=======
  public override bool Enable { get { return Image.enabled; } set { Image.enabled = value; } }
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
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
<<<<<<< HEAD
=======
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
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
  #endregion


  # region Methods
  public ImageComponentBuilder()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log($"start to create {GetComponentType()} from {defaultArguTable["resourcesFolderPath"] + defaultArguTable["gameObjectName"]}");
    }
  }
  public ImageComponentBuilder(Dictionary<string, string> arguTable)
  {
    ArguTable = arguTable;
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log($"start to create {GetComponentType()} from {ArguTable["resourcesFolderPath"] + ArguTable["gameObjectName"]}");
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
<<<<<<< HEAD
    Image.sprite = Resources.Load<Sprite>(arguTable["resourcesFolderPath"] + arguTable["gameObjectName"]);
    Image.preserveAspect = true;
=======
    if (arguTable["gameObjectName"] != null & arguTable["resourcesFolderPath"] != null)
    {
      Image.sprite = Resources.Load<Sprite>(arguTable["resourcesFolderPath"] + arguTable["gameObjectName"]);
      Image.preserveAspect = true;
    }
>>>>>>> 740b70b2d81a3bdd40b39a7c690b3d7a0aaddff3
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
    Image.color = color;
  }
  #endregion
}