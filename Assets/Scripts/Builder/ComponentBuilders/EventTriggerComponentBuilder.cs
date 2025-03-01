using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventTriggerComponentBuilder : ComponentBuilder<UnityAction>
{
  #region Fields
  public readonly static string componentType = "Button";
  public readonly static HashSet<string> arguNameTable = new()
  {
    "onClickAction",
    "onHoverAction",
  };
  public readonly static Dictionary<string, UnityAction> defaultArguTable = new()
  {
    {"onClickAction", null},
    {"onHoverAction", null},
  };
  #endregion


  #region Properities
  public EventTrigger EventTrigger { get; set; }
  public EventTrigger.Entry EntryDown { get; set; }
  public EventTrigger.Entry EntryUp { get; set; }
  #endregion


  #region Methods
  public EventTriggerComponentBuilder()
  {
    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log($"start to create {GetComponentType()}");
    }
  }

  public EventTriggerComponentBuilder(Dictionary<string, UnityAction> arguTable)
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
    return EventTrigger;
  }

  public override HashSet<string> GetArguNameTable()
  {
    return arguNameTable;
  }

  public override Dictionary<string, UnityAction> GetDefaultArguTable()
  {
    return defaultArguTable;
  }

  public override void Modify(Dictionary<string, UnityAction> arguTable)
  {
    float pressTime = 0f;

    EntryDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
    EntryDown.callback.AddListener((data) =>
    {
      pressTime = Time.time;
    });
    EventTrigger.triggers.Add(EntryDown);

    EntryUp = new() { eventID = EventTriggerType.PointerUp };
    EntryUp.callback.AddListener((data) =>
    {
      if (Time.time - pressTime > SettingsInfo.longPressThreshold)
      {
        if (arguTable.TryGetValue("onHoverAction", out var onHoverAction) && onHoverAction != null)
        {
          onHoverAction.Invoke();
        }
      }
      else
      {
        if (arguTable.TryGetValue("onClickAction", out var onClickAction) && onClickAction != null)
        {
          onClickAction.Invoke();
        }
      }
    });
    EventTrigger.triggers.Add(EntryUp);
  }

  public void Modify(EventTrigger eventTrigger)
  {
    EventTrigger = eventTrigger;
  }

  public override void Build(GameObject gameObject)
  {
    if (gameObject.TryGetComponent(out EventTrigger eventTrigger))
    {
      EventTrigger = eventTrigger;
    }
    else
    {
      EventTrigger = gameObject.AddComponent<EventTrigger>();
    }

    Modify(ArguTable);

    if (DebugInfo.PrintDebugInfo)
    {
      Debug.Log($"successfully load {componentType} in {gameObject.name}");
    }
  }
  #endregion
}