// 选择房间的界面
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Realtime;

public class MatchingScene : UIObject
{
  #region Fields
  private bool _isRoomInfo = true;    // 这是展示谁的界面
  public static readonly string resourcesFolderPath = "Arts/Textures/MatchingScene/";
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
  private ImageGameObjectBuilder _matchingBackgroundImage = null;
  private readonly Dictionary<string, string> _backgroundImageArgu = new()
  {
    {"gameObjectName", "BackgroundImage"},
    {"resourcesFolderPath", resourcesFolderPath},
  };
  private InputBoxUniversalBuilder _searchInputBox = null;                        // 搜索输入框
  private const float _researchPosY = 0.95f;                                      // 搜索输入框的高度
  private ScrollRectGameObjectBuilder _scrollRectGameObejctBuilder = null;        // 滚轮
  private VerticalLayoutGroupGameObjectBuilder _roomIdVerticalLayout = null;      // 数值排列, 未必是 roomId, 也可能是 userId
  private List<RoomInfoInterface> _roomInfoList = null;                           // 房间信息的列表, 可以是 userId
  private const float _scrollPosY = 0.55f;                                        // 滚轮的纵坐标
  private ButtonUniversalbuilder _searchButton = null;                            // 输入后查找的
  private ButtonUniversalbuilder _refreshButton = null;                           // 刷新房间
  private ButtonUniversalbuilder _createButton = null;                            // 加入房间
  private ButtonUniversalbuilder _returnButton = null;                            // 返回房间列表
  private RoomCreateInterface _roomCreateInterface = null;                        // 点击加入房间后弹出的那个框
  private KeyInputInterface _keyInputInterface = null;                            // 输入密码的那个界面
  private PromptBoxUniversalBuilder _promptBox = null;                            // 提示框, 调用之前记得调用 ModifyContent
  public ButtonUniversalbuilder _exitButton = null;                               // 退出按钮
  public TextBoxUniversalBuilder _playNumBox = null;                              // 显示当前房间的玩家
  public TextBoxUniversalBuilder _roomIdBox = null;                               // 显示当前的房间号

    private string password;
    private Dictionary<string,bool> allIsReady = new Dictionary<string,bool>();
    private bool isReady = false;
  #endregion


  #region Properties
  public int RoomLimit { set; get; } = 20;    // 房间的搜索数量
  public int PlayerLimit { set; get; } = 8;  // 当前玩家的数量
  #endregion


  #region Methods
  public override void OnLoad()
  {
        _backgroundImage = new ImageGameObjectBuilder("BackgroundImage", this.transform);
        _backgroundImage.Build(
      new Dictionary<string, IComponentBuilder>
      {
        {"RectTransform", new RectTransformComponentBuilder(_fullScreenRectTransformArgu)},
        {"Image", new ImageComponentBuilder(_backgroundImageArgu)}
      }
    );

    // 那个大方框
    _matchingBackgroundImage = new ImageGameObjectBuilder("MatchingBackground", _backgroundImage.Transform);
    _matchingBackgroundImage.Build(
      new Dictionary<string, IComponentBuilder>
      {
        {"RectTransform", new RectTransformComponentBuilder(
          new Dictionary<string, Vector2>{
            {"referenceObjectPixels", new Vector2(1280, 960)},
            {"archorMin", new Vector2(0.5f, 0.5f)},
            {"archorMax", new Vector2(0.5f, 0.5f)},
            {"pivotPos", new Vector2(0.5f, 0.5f)},
          }
        )},
        {"Image", new ImageComponentBuilder(
          new Dictionary<string, string>{
            {"gameObjectName", "MatchingBackgroundImage"},
            {"resourcesFolderPath", resourcesFolderPath},
          }
        )},
      }
    );

    // 搜索 room Id 的那个文本输入框
    _searchInputBox = new InputBoxUniversalBuilder("InputBox", _matchingBackgroundImage.Transform, new Vector2(0.5f, _researchPosY));

    // 搜索按钮
    _searchButton = new ButtonUniversalbuilder(
    "SearchButton", new Vector2(1.0f, 0.5f), _searchInputBox.Transform, new Dictionary<string, UnityAction>
      {
        {"onClickAction", OnSearchButtonClicked},
        {"OnHoverAction", null}
      }, -1, 80.0f / 100.0f
    );

    // 滚动条的框架
    _scrollRectGameObejctBuilder = new ScrollRectGameObjectBuilder(_matchingBackgroundImage.Transform);
    _scrollRectGameObejctBuilder.Build(
      new SortedDictionary<string, Dictionary<string, IComponentBuilder>>
      {
        {"MaskImage", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
              {
                {"referenceObjectPixels", new Vector2(1080, 640)},
                {"archorMin", new Vector2(0.5f, _scrollPosY)},
                {"archorMax", new Vector2(0.5f, _scrollPosY)},
                {"pivotPos", new Vector2(0.5f, 0.5f)},
              }
            )},
            {"Image", new ImageComponentBuilder(new Dictionary<string, string>
              {
                {"gameObjectName", "MaskImage"},
                {"resourcesFolderPath", resourcesFolderPath}
              }
            )},
          }
        },
        {"ScrollBar", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
              {
                {"referenceObjectPixels", new Vector2(40, 640)},
                {"archorMin", new Vector2(1.0f, _scrollPosY)},
                {"archorMax", new Vector2(1.0f, _scrollPosY)},
                {"pivotPos", new Vector2(0.5f, 0.5f)},
              })
            },
          }
        },
        {"SlidingArea", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
              {
                {"referenceObjectPixels", new Vector2(40, 640)},
                {"archorMin", new Vector2(0.5f, 0.5f)},
                {"archorMax", new Vector2(0.5f, 0.5f)},
                {"pivotPos", new Vector2(0.5f, 0.5f)},
              })
            }
          }
        },
        {"HandleImage", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
              {
                {"referenceObjectPixels", new Vector2(-1, -1)},
                {"archorMin", new Vector2(0.5f, 0.5f)},
                {"archorMax", new Vector2(0.5f, 0.5f)},
                {"pivotPos", new Vector2(0.5f, 0.5f)},
              }
            )},
          }
        }
      }
    );

    // 竖直排列
    _roomIdVerticalLayout = new VerticalLayoutGroupGameObjectBuilder("VerticalLayoutGroup", _scrollRectGameObejctBuilder.Transform);
    _roomIdVerticalLayout.Build(new Dictionary<string, IComponentBuilder>
    {
      {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
        {
          {"referenceObjectPixels", new Vector2(1080, -1)},
          {"archorMin", new Vector2(0.5f, _scrollPosY)},
          {"archorMax", new Vector2(0.5f, _scrollPosY)},
          {"pivotPos", new Vector2(0.5f, 0.5f)},
        }
      )},
    });

    _roomInfoList = new List<RoomInfoInterface>();
    List<ISingleGameObjectBuilder> _roomInfoBoxList = new();
    for (int idx = 0; idx < RoomLimit; idx++)
    {
      RoomInfoInterface temp = new(idx, _roomIdVerticalLayout.Transform);
      temp.SetActive(false);
      _roomInfoList.Add(temp);
      _roomInfoBoxList.Add(temp.ImageGameObjectBuilder);
    }

    _roomIdVerticalLayout.Spacing = 10.0f;
    _scrollRectGameObejctBuilder.SetContent(_roomIdVerticalLayout.RectTransform, new ColorBlock()
    {
      normalColor = new Color(1.0f, 1.0f, 1.0f, 0.5f),
      highlightedColor = new Color(1.0f, 1.0f, 1.0f, 0.65f),
      pressedColor = new Color(1.0f, 1.0f, 1.0f, 0.8f),
      selectedColor = new Color(1.0f, 1.0f, 1.0f, 0.5f),
      disabledColor = new Color(1.0f, 1.0f, 1.0f, 0.3f),
      colorMultiplier = 1.0f,
      fadeDuration = 0.1f,
    }, new Color(1.0f, 1.0f, 1.0f, 0.5f));
    _scrollRectGameObejctBuilder.InitPos = 1.0f;

    // 几个不同的按钮 
    _refreshButton = new ButtonUniversalbuilder(
    "RefreshButton", new Vector2(0.2f, 0.1f), _matchingBackgroundImage.Transform, new Dictionary<string, UnityAction>
      {
        {"onClickAction", OnRefreshButtonClick},
        {"OnHoverAction", null}
      }, 3, 1.5f
    );

    _createButton = new ButtonUniversalbuilder(
    "JoinButton", new Vector2(0.8f, 0.1f), _matchingBackgroundImage.Transform, new Dictionary<string, UnityAction>
      {
        {"onClickAction", OnCreateButtonClick},
        {"OnHoverAction", null}
      }, 3, 1.5f
    );

    _searchButton = new ButtonUniversalbuilder(
    "SearchButton", new Vector2(1.0f, 0.5f), _searchInputBox.Transform, new Dictionary<string, UnityAction>
      {
        {"onClickAction", OnSearchButtonClicked},
        {"OnHoverAction", null}
      }, -1, 80f / 100f
    );

    _returnButton = new ButtonUniversalbuilder(
    "ReturnButton", new Vector2(0.5f, 0.1f), _matchingBackgroundImage.Transform, new Dictionary<string, UnityAction>
      {
        {"onClickAction", OnReturnButonClicked},
        {"OnHoverAction", null}
      }, 3, 1.5f
    );

    // 那个启动房间的界面
    _roomCreateInterface = new RoomCreateInterface(_backgroundImage.Transform);

    // 输入密钥的界面
    _keyInputInterface = new KeyInputInterface(_backgroundImage.Transform, OnKeyInputComfirmClick);

    // 满员后的提示界面
    _promptBox = new PromptBoxUniversalBuilder("RoomFullPromptBox", new Vector2(0.5f, 0.5f), _backgroundImage.Transform);
    _promptBox.ModifyButtonContent("Confirm");
    _promptBox.ModifyPromptContent("the room is full");

    // 退出按钮
    _exitButton = new ButtonUniversalbuilder(
    "ExitButton", new Vector2(1.0f, 1.0f), _matchingBackgroundImage.Transform, new Dictionary<string, UnityAction>
      {
        {"onClickAction", OnExitButtonClick},
        {"OnHoverAction", null}
      }, -1, 80.0f / 100.0f
    );

    // 人数
    _playNumBox = new TextBoxUniversalBuilder("PlayNumBox", _matchingBackgroundImage.Transform, new Vector2(0.796875f, _researchPosY), 3, 
      80f / TextBoxUniversalBuilder.GetReferencePixel(3).y);

    // 房间号
    _roomIdBox = new TextBoxUniversalBuilder("RoomIdBox", _matchingBackgroundImage.Transform, new Vector2(0.328125f, _researchPosY), 5,
      80f / TextBoxUniversalBuilder.GetReferencePixel(5).y);

        Init();
    }
    private void CallKeyInput(string password)
    {
        Debug.Log("CallKeyInput");
        _keyInputInterface.SetActive(true);
        this.password = password;
        EventManager.RemoveListener<string>("CallKeyInput", CallKeyInput);
    }

    private void Init()
    {
        EventManager.AddListener<string>("OnLeftRoomEvent", OnLeftRoom);
        EventManager.AddListener<List<RoomInfo>>("OnRoomListUpdateEvent", FreshRoomObjects);
        EventManager.AddListener<string>("OnJoinedRoomEvent", OnJoinedRoom);
        EventManager.AddListener<Player>("OnPlayerLeftRoomEvent", OnPlayerLeftRoom);
        EventManager.AddListener<Player>("OnPlayerEnteredRoomEvent", OnPlayerEnterRoom);
        EventManager.AddListener<string>("CallKeyInput", CallKeyInput);
        EventManager.AddListener<Player, ExitGames.Client.Photon.Hashtable>("OnPlayerPropertiesUpdateEvent", OnPlayerPropertiesUpdate);

        SetActive();

        _searchInputBox.InputBoxBuilder.InputFieldBuilder.InputField .InputField.text = null;
        OnRefreshButtonClick();

        if (GameLoop.Instance.onlineManager.IsClient)
        {
            isReady = true;
            _returnButton.ModifyContent("开始游戏");
            _returnButton.SetActive(false);
            FreshStartButton();
            return;
        }
        _returnButton.ModifyContent(isReady ? "已准备" : "准备");
        GameLoop.Instance.onlineManager.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable { { "IsReady", isReady } });


    }


  public void SetActive(bool state = true)
  {
        // 控制界面的一些 UI:
        _backgroundImage.SetActive(state);
        _matchingBackgroundImage.SetActive(state);
        _scrollRectGameObejctBuilder.SetActive(state);
        _roomIdVerticalLayout.SetActive(state);
        _exitButton.SetActive(state);

        _searchInputBox.SetActive(_isRoomInfo & state);
        _searchButton.SetActive(_isRoomInfo & state);
        _refreshButton.SetActive(_isRoomInfo & state);
        _createButton.SetActive(_isRoomInfo & state);

        _returnButton.SetActive(!_isRoomInfo & state);
        _roomIdBox.SetActive(!_isRoomInfo & state);
        _playNumBox.SetActive(!_isRoomInfo & state);
    }

  /* 按钮的部分: */

  // 点击关闭按钮后退出弹窗
  public void OnExitButtonClick()
  {
    DebugInfo.Print("exit button clicked");
        // TODO: 切换回开始界面

        if (!_isRoomInfo)
        {
            _isRoomInfo = true;
            GameLoop.Instance.onlineManager.LeaveRoom();
            SetActive();
        }
        else
        {
            UI_Manager.Instance.ShowUI<StartScene>("LoginUI");
            UI_Manager.Instance.CloseUI(this.gameObject.name);
        }
  }

  // 点击刷新房间后更新
  public void OnRefreshButtonClick()
  {
    DebugInfo.Print("refresh button clicked");
    RefreshList(_searchInputBox.GetContent());
  }
    private void OnLeftRoom(string s)
    {
        EventManager.AddListener("OnConnectedServerEvent",OnConnected);
    }

    private void OnConnected()
    {
        RefreshList();
        EventManager.RemoveListener("OnConnectedServerEvent", OnConnected);
    }



    // 点击创建房间后弹出创建房间的内容
    public void OnCreateButtonClick()
  {
    DebugInfo.Print("join button clicked");
    _roomCreateInterface.SetActive(true);
  }

  // 点击搜索按钮后刷新
  public void OnSearchButtonClicked()
  {
    DebugInfo.Print("search button clicked");

    if (_isRoomInfo == true)
    { // 只有在房间模式才会更新
      RefreshList(_searchInputBox.GetContent());
    }
  }

    // 在密钥输入界面输入密钥后加入房间
    public void OnKeyInputComfirmClick()
    {
        if(password == _keyInputInterface.KeyInputBox.GetContent())
        {
            EventManager.BroadCast("KeyRight");
            _keyInputInterface.SetActive(false);
            return;
        }
        UI_Manager.Instance.LogWarnning("密码错误！");
    }

    // roomInfoList 的切换:
    public void RefreshList(string searchRoomId)
    {
        GameLoop.Instance.onlineManager.RefreshRoomList(searchRoomId);
    }
    public void RefreshList()
    {
        GameLoop.Instance.onlineManager.RefreshRoomList();
    }

    private void OnJoinedRoom(string roomId) { 
        _isRoomInfo = false ;
        _roomCreateInterface.SetActive(false);
        SetActive();
        InRoomUIFresh();
        foreach(Player p in GameLoop.Instance.onlineManager.PlayerListOthers)
        {
            if (p.IsMasterClient)
            {
                allIsReady.Add(p.UserId, true);
                continue;
            }
            p.CustomProperties.TryGetValue("IsReady", out object value);
            allIsReady.Add(p.UserId, (bool)value);
        }
        if (!GameLoop.Instance.onlineManager.IsClient)
        {
            allIsReady.Add(GameLoop.Instance.onlineManager.LocalPlayer.UserId, GameLoop.Instance.onlineManager.IsClient);
        }
    }

    private void OnPlayerPropertiesUpdate(Player targetPlayer,ExitGames.Client.Photon.Hashtable table)
    {
        if(table.TryGetValue("IsReady", out object value))
        {
            allIsReady[targetPlayer.UserId] = (bool)value;
            Debug.Log((bool)value);
            if (GameLoop.Instance.onlineManager.IsClient)
            {
                InRoomUIFresh();
            }
        }
    }

    private bool AllReady()
    {
        foreach(KeyValuePair<string,bool> pair in allIsReady)
        {
            if (!pair.Value)
                return false;
        }
        return true;
    }
    private void OnPlayerEnterRoom(Player newPlayer)
    {
        allIsReady.Add(newPlayer.UserId, false);
        InRoomUIFresh();

    }

    private void OnPlayerLeftRoom(Player thePlayer)
    {
        allIsReady.Remove(thePlayer.UserId);
        InRoomUIFresh();

    }
    private void InRoomUIFresh()
    {
        if(GameLoop.Instance.onlineManager.CurrentRoom == null) { return; }
        int playerCount = GameLoop.Instance.onlineManager.CurrentRoom.PlayerCount;
        GameLoop.Instance.onlineManager.CurrentRoom.CustomProperties.TryGetValue(OnlineManager.RoomNameSearchFilter, out object roomName);
        _playNumBox.ModifyContent(playerCount + " / 8");
        _roomIdBox.ModifyContent((string)roomName);
        for (int i = 0; i < 20; i++)
        {
            _roomInfoList[i].SetActive(i < playerCount);
            _roomInfoList[i].ShiftMode(false);
            if(i<playerCount)
            {
                if (i > 0) { _roomInfoList[i].ModifyTextContent(GameLoop.Instance.onlineManager.PlayerListOthers[i - 1].NickName); }
                else { _roomInfoList[0].ModifyTextContent(GameLoop.Instance.onlineManager.LocalPlayer.NickName); }
            }
        }
        FreshStartButton();
    }

    private void FreshStartButton()
    {
        if (GameLoop.Instance.onlineManager.IsClient)
        {
            isReady = true;
            allIsReady[GameLoop.Instance.onlineManager.LocalPlayer.UserId] = true;
            _returnButton.SetActive(AllReady());
            return;
        }
        
    }
    private void FreshRoomObjects(List<RoomInfo> rooms)
    {
        int roomCount = rooms.Count;
        for (int i = 0; i < RoomLimit; i++)
        {
            _roomInfoList[i].SetActive(i < roomCount);
            _roomInfoList[i].ShiftMode(true);
            if (i < roomCount)
            {
                rooms[i].CustomProperties.TryGetValue(OnlineManager.RoomNameSearchFilter, out object filter);
                string roomId = rooms[i].Name;
                int playerCount = rooms[i].PlayerCount;
                _roomInfoList[i].ModifyRoomContent(roomId, playerCount,(string)filter);
                Debug.Log(rooms[i].CustomProperties.TryGetValue(OnlineManager.RoomPasswordFilter, out object T)+ (string)T);
                if (rooms[i].CustomProperties.TryGetValue(OnlineManager.RoomPasswordFilter,out object passwordKey))
                {
                    _roomInfoList[i].ModifyRoomContent(roomId, playerCount, (string)filter,(string)passwordKey);
                }
            }
        }
    }

  // 在当前房间进行退出
  public void OnReturnButonClicked()
  {
    DebugInfo.Print("return button clicked");
        if (GameLoop.Instance.onlineManager.IsClient)
        {
            GameLoop.Instance.onlineManager.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable { { "StartGame", true } } );
            return;
        }

        isReady = !isReady;
        _returnButton.ModifyContent(isReady ? "已准备" : "准备");
        GameLoop.Instance.onlineManager.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable { { "IsReady", isReady } });

    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<List<RoomInfo>>("OnRoomListUpdateEvent", FreshRoomObjects);
        EventManager.RemoveListener<Player>("OnPlayerLeftRoomEvent", OnPlayerLeftRoom);
        EventManager.RemoveListener<Player>("OnPlayerEnteredRoomEvent", OnPlayerEnterRoom);
        EventManager.RemoveListener<Player, ExitGames.Client.Photon.Hashtable>("OnPlayerPropertiesUpdateEvent", OnPlayerPropertiesUpdate);
        EventManager.RemoveListener<string>("OnJoinedRoomEvent", OnJoinedRoom);
    }
    #endregion
}