// 这是展示 roomId 或者 userId 的那个小方框
// _isRoomShow 控制展示的是谁 ?
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.UI;

public class RoomInfoInterface : IInterfaceBuilder
{
  #region Fields
  private TextBoxGameObjectBuilder _roomInfoBackgroundBuilder;  // 这是整个的方框, 也可以包含用户背景
  private TextBoxGameObjectBuilder _roomIdTextBuilder;          // 这是展示 roomId
  private TextBoxGameObjectBuilder _playNumTextBuilder;         // 展示目前多少玩家
  private ButtonUniversalbuilder _joinButton;                   // 加入某一个房间的按钮
  #endregion

  #region Properties
  public bool NeedKey { get; set; } = false;
  public bool IsRoomInfo { get; set; } = true;          // 控制展示谁
  public string GameObjectName { get; private set; }    // 名称
  public int RoomIdx { get; set; }                      // 房间的排列号
  // 返回主体
  public ImageGameObjectBuilder ImageGameObjectBuilder { get { return _roomInfoBackgroundBuilder.ImageBuilder;}}
  #endregion


  #region Methods
  // 构造函数
  public RoomInfoInterface(int roomIdx, Transform parentTransform)
  {
    RoomIdx = roomIdx;
    GameObjectName = "RoomInfoBox_" + roomIdx.ToString();
    
    // 背景文本框
    _roomInfoBackgroundBuilder = new TextBoxGameObjectBuilder(new Dictionary<string, string>{
      {"Text", "RoomInfoText"},
      {"Image", GameObjectName}
    },  parentTransform);
    _roomInfoBackgroundBuilder.Build(new SortedDictionary<string, Dictionary<string, IComponentBuilder>>
    {
      {"Image", new Dictionary<string, IComponentBuilder>
        {
          {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
            {
              {"referenceObjectPixels", new Vector2(920, 80)},
              {"archorMin", new Vector2(0.5f, 0.5f)},
              {"archorMax", new Vector2(0.5f, 0.5f)},
              {"pivotPos", new Vector2(0.5f, 0.5f)},
            }
          )},
          {"Image", new ImageComponentBuilder(new Dictionary<string, string>
            {
              {"gameObjectName", "RoomInfoBoxImage"},
              {"resourcesFolderPath", MatchingScene.resourcesFolderPath},
            }  
          )}
        }
      },
      {"Text", new Dictionary<string, IComponentBuilder>
        {
          {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
            {
              {"referenceObjectPixels", new Vector2(920, 80) * 0.9f},
              {"archorMin", new Vector2(0.5f, 0.5f)},
              {"archorMax", new Vector2(0.5f, 0.5f)},
              {"pivotPos", new Vector2(0.5f, 0.5f)},
            }
          )}, 
          {"Text", new TextComponentBuilder(TextBoxUniversalBuilder.textArgu_02)}
        }
      }
    });
    _roomInfoBackgroundBuilder.ModifyContent(GameObjectName);

    // 防止竖直排列导致文本框变形 
    LayoutElement layoutElement = _roomInfoBackgroundBuilder.GameObject.AddComponent<LayoutElement>();
    layoutElement.preferredHeight = _roomInfoBackgroundBuilder.RectTransform.rect.height;
    layoutElement.preferredWidth = _roomInfoBackgroundBuilder.RectTransform.rect.width;
    layoutElement.flexibleHeight = 0;

    // 加载 roomId 
    _roomIdTextBuilder = new TextBoxGameObjectBuilder(new Dictionary<string, string>{
      {"Image", "RoomIdxBackground"},
      {"Text", "RoomIdx"},
    }, _roomInfoBackgroundBuilder.Transform);
    _roomIdTextBuilder.Build(new SortedDictionary<string, Dictionary<string, IComponentBuilder>>{
      {"Image", new Dictionary<string, IComponentBuilder>
        {
          {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
            {
              {"referenceObjectPixels", new Vector2(540, 60)},
              {"archorMin", new Vector2(0.3152f, 0.5f)},
              {"archorMax", new Vector2(0.3152f, 0.5f)},
              {"pivotPos", new Vector2(0.5f, 0.5f)},
            }
          )},
          {"Image", new ImageComponentBuilder(new Dictionary<string, string>
            {
              {"gameObjectName", "RoomIdxBoxImage"},
              {"resourcesFolderPath", MatchingScene.resourcesFolderPath}
            })
          }
        }
      },
      {"Text", new Dictionary<string, IComponentBuilder>
        {
          {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
            {
              {"referenceObjectPixels", new Vector2(540, 60) * 0.9f},
              {"archorMin", new Vector2(0.5f, 0.5f)},
              {"archorMax", new Vector2(0.5f, 0.5f)},
              {"pivotPos", new Vector2(0.5f, 0.5f)},
            }
          )}, 
          {"Text", new TextComponentBuilder(TextBoxUniversalBuilder.textArgu_02)}
        }
      }
    });

    // 加载当前玩家数目
    _playNumTextBuilder = new TextBoxGameObjectBuilder(new Dictionary<string, string>{
      {"Image", "Background"},
      {"Text", "RoomIdx"},
    }, _roomInfoBackgroundBuilder.Transform);
    _playNumTextBuilder.Build(new SortedDictionary<string, Dictionary<string, IComponentBuilder>>{
      {"Image", new Dictionary<string, IComponentBuilder>
        {
          {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
            {
              {"referenceObjectPixels", new Vector2(120, 60)},
              {"archorMin", new Vector2(0.6957f, 0.5f)},
              {"archorMax", new Vector2(0.6957f, 0.5f)},
              {"pivotPos", new Vector2(0.5f, 0.5f)},
            }
          )},
          {"Image", new ImageComponentBuilder(new Dictionary<string, string>
            {
              {"gameObjectName", "PlayerNumBoxImage"},
              {"resourcesFolderPath", MatchingScene.resourcesFolderPath}
            })
          }
        }
      },
      {"Text", new Dictionary<string, IComponentBuilder>
        {
          {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
            {
              {"referenceObjectPixels", new Vector2(120, 60) * 0.9f},
              {"archorMin", new Vector2(0.5f, 0.5f)},
              {"archorMax", new Vector2(0.5f, 0.5f)},
              {"pivotPos", new Vector2(0.5f, 0.5f)},
            }
          )}, 
          {"Text", new TextComponentBuilder(TextBoxUniversalBuilder.textArgu_02)}
        }
      }
    });

    // 加载加入按钮
    _joinButton = new ButtonUniversalbuilder(
    "JoinButton", new Vector2(0.88f, 0.5f), _roomInfoBackgroundBuilder.Transform, new Dictionary<string, UnityAction>
      {
        {"onClickAction", OnJoinButtonClick},
        {"OnHoverAction", null}
      }, 3, 180 / ButtonUniversalbuilder.GetReferencePixel(3).x
    );
  } 

  public void SetActive(bool state)
  {
    _roomInfoBackgroundBuilder.SetActive(state);
    _roomIdTextBuilder.SetActive(state);
    _playNumTextBuilder.SetActive(state);
    _joinButton.SetActive(state);

    _roomIdTextBuilder.TextBuilder.SetActive(state);
  }

  // 切换模式, 显示谁?
  public void ShiftMode(bool isRoomInfo)
  {
  
    IsRoomInfo = isRoomInfo;
    _roomIdTextBuilder.SetActive(IsRoomInfo);
    _playNumTextBuilder.SetActive(IsRoomInfo);
    _joinButton.SetActive(isRoomInfo);

    // 总有一个展示 
    _roomInfoBackgroundBuilder.TextBuilder.TextBuilder.Text.enabled = !IsRoomInfo; 
  }

  // 点击加入按钮后的逻辑
  public void OnJoinButtonClick()
  {
    DebugInfo.Print(@$"button {RoomIdx} clicked");
    // TODO: 
    if (IsRoomFull() == false)
    {
      NeedKey = IsKeyNeeded();

      if (NeedKey == false)
      {
        // 进行状态切换
        IsRoomInfo = false;
        DebugInfo.Print("shift to" + IsRoomInfo.ToString());
      }
    }
  }

  public bool IsKeyNeeded()
  {
    // TODO: 房间是否需要密钥?
    return false;
  }

  public bool IsRoomFull()
  {
    // TODO: 房间是否满员?
    return false;
  }


  public void ModifyRoomContent(string roomId, int playerCount)
  {
    _roomIdTextBuilder.ModifyContent(roomId);
    _playNumTextBuilder.ModifyContent(playerCount + "/8");
  }

  public void ModifyTextContent(string userId)
  {
    _roomInfoBackgroundBuilder.TextBuilder.ModifyContent(userId);
  }
  #endregion
}