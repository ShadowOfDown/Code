using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;

public class SliderGameObjectBuilder : GroupGameObjectBuilder
{
  #region Fields
  public override GameObject GameObject { get { return SliderAreaBuiler.GameObject; }}
  public override string GameObjectName { get { return SliderAreaBuiler.GameObjectName; } }
  public override Transform ParentTransform { get; }
  public override Transform Transform { get { return BackgroundBuilder.Transform; } }
  public override RectTransform RectTransform { get { return BackgroundBuilder.RectTransform; } }
  public readonly static string groupType = "Slider";
  public readonly static HashSet<string> gameObjectTypeTable = new()
  {
    "SliderArea",
    "BackgroundImage",
    "FillImage",
    "FillArea",
    "HandleImage",
    "HandleArea",
  };
  #endregion


  #region Properties
  public RectTransformGameObjectBuilder SliderAreaBuiler { get; set; } = null;
  public ImageGameObjectBuilder BackgroundBuilder { get; set; } = null;
  public RectTransformGameObjectBuilder SliderBuilder { get; set; } = null;
  public RectTransformGameObjectBuilder FillBuilder { get; set; } = null;
  public ImageGameObjectBuilder FillImageBuilder { get; set; } = null;
  public RectTransformGameObjectBuilder HandleBuilder { get; set; } = null;
  public ImageGameObjectBuilder HandleImageBuilder { get; set; } = null;
  public Slider Slider { get; set; } = null;
  public float Value 
  { 
    get { return Slider.value; }
    set { Slider.value = value; }
  }
  #endregion


  #region Methods
  public SliderGameObjectBuilder(string SliderName, Transform parentTransform, bool isHanleNeed = false)
  {
    ParentTransform = parentTransform;

    GameObjectTable = new SortedDictionary<string, ISingleGameObjectBuilder>();
    
    SliderAreaBuiler = new RectTransformGameObjectBuilder(SliderName, parentTransform);
    GameObjectTable.Add("SliderArea", SliderAreaBuiler);

    // 初始化 BackgroundImage 并添加到 GameObjectTable
    BackgroundBuilder = new ImageGameObjectBuilder("Background", SliderAreaBuiler.Transform);
    GameObjectTable.Add("BackgroundImage", BackgroundBuilder);

    // 初始化 FillArea 和 FillImage 并添加到 GameObjectTable
    FillBuilder = new RectTransformGameObjectBuilder("FillArea", SliderAreaBuiler.Transform);
    GameObjectTable.Add("FillArea", FillBuilder);

    FillImageBuilder = new ImageGameObjectBuilder("Fill", FillBuilder.Transform);
    GameObjectTable.Add("FillImage", FillImageBuilder);

    if (isHanleNeed)
    {      
      // 初始化 HandleArea 和 HandleImage 并添加到 GameObjectTable
      HandleBuilder = new RectTransformGameObjectBuilder("HandleArea", SliderAreaBuiler.Transform);
      GameObjectTable.Add("HandleArea", HandleBuilder);

      HandleImageBuilder = new ImageGameObjectBuilder("Handle", HandleBuilder.Transform);
      GameObjectTable.Add("HandleImage", HandleImageBuilder);
    }
  }

  public override string GetGroupType()
  {
    return groupType;
  }

  public override HashSet<string> GetGameObjectTypeTable()
  {
    return gameObjectTypeTable;
  }

  public override void Build(SortedDictionary<string, Dictionary<string, IComponentBuilder>> gameObjectComponentTable)
  {
    base.Build(gameObjectComponentTable);
    SetSlider();
  }

  public void SetSlider()
  {
    if (SliderAreaBuiler != null)
    {
      Slider = SliderAreaBuiler.GameObject.AddComponent<Slider>();
      if (HandleBuilder != null)
      {
        Slider.targetGraphic = HandleImageBuilder.Image.Image;
        Slider.colors = new ColorBlock()
        {
          normalColor = Color.white,
          highlightedColor = Color.gray,
          pressedColor = Color.gray,
          disabledColor = Color.black,
          selectedColor = Color.white,
          colorMultiplier = 1.0f,
          fadeDuration = 0.1f,
        };
        Slider.handleRect = GameObjectTable["HandleImage"].RectTransform;
      }
      else
      {
        Slider.transition = Selectable.Transition.None;
      }
      Slider.fillRect = GameObjectTable["FillImage"].RectTransform;
      FillImageBuilder.PreserveAspect = false;

      FillBuilder.RectTransform.offsetMax = FillBuilder.RectTransform.offsetMin =
        FillImageBuilder.RectTransform.offsetMax = FillImageBuilder.RectTransform.offsetMin = Vector2.zero;
    }
    else 
    {
      Debug.LogWarning($"the background builder {BackgroundBuilder.GameObjectName} is null");
    }
  }
  #endregion
}