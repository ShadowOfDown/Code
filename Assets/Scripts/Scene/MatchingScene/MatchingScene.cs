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
  public static string resourcesFolderPath = "Arts/Textures/MatchingScene/";
  ScreenMaskUniversalBuilder _baseImage = null; // TODO: 把这个扔到基类里面去, 就是一个黑色的遮罩
  private ImageGameObjectBuilder _backgroundImage = null;
  private ImageGameObjectBuilder _matchingBackgroundImage = null;
  private Vector2 _referenceMatchingBackgroundPixel = new(1440, 1080);
  private Vector2 _referenceMaskPixel = new(1280, 640);
  private float _searchInputBoxHeight;
  private float _inputboxPosX;
  private float _searchButtonPosX;
  private InputBoxUniversalBuilder _searchInputBox = null;                        // 搜索输入框
  private int _searchInputType = 4;
  private bool _searchInputIsDark = false;
  private float _researchPosY;                                      // 搜索输入框的高度
  private ScrollRectUniversalBuilder _sliderBuilder = null;        // 滚轮
  private List<RoomInfoInterface> _roomInfoList = null;                           // 房间信息的列表, 可以是 userId
  private float _scrollPosY;                                        // 滚轮的纵坐标
  private ButtonUniversalbuilder _searchButton = null;                            // 输入后查找的
  private int _buttonType = 3;
  private float _buttonHeight = 120;
  private bool _buttonIsDark = false;
  private float _buttonPosY;
  private float _buttonPosX;
  private ButtonUniversalbuilder _refreshButton = null;                           // 刷新房间
  private ButtonUniversalbuilder _createButton = null;                            // 加入房间
  private ButtonUniversalbuilder _returnButton = null;                            // 返回房间列表
  private RoomCreateInterface _roomCreateInterface = null;                        // 点击加入房间后弹出的那个框
  private KeyInputInterface _keyInputInterface = null;                            // 输入密码的那个界面
  private PromptBoxUniversalBuilder _promptBox = null;                            // 提示框, 调用之前记得调用 ModifyContent
  private ButtonUniversalbuilder _exitButton = null;                               // 退出按钮
  private TextBoxUniversalBuilder _playNumBox = null;                              // 显示当前房间的玩家
  private TextBoxUniversalBuilder _roomIdBox = null;                               // 显示当前的房间号
  private float _roomIdPosX;
  private bool _roomIdIsDark = true;
  private int _roomIdType = 5;
  private float _playNumPosX;
  private bool _playNumBoxIsDark = true;
  private int _playNumBoxType = 3;

  private string password;
  private Dictionary<string, bool> allIsReady = new Dictionary<string, bool>();
  private bool isReady = false;
  #endregion

  #region Properties
  public int RoomLimit { set; get; } = 20;    // 房间的搜索数量
  public int PlayerLimit { set; get; } = 8;    // 当前玩家的数量
  #endregion

  #region Methods
  public override void OnLoad()
  {
    BuildBackgroundImage();
    BuildMatchingBackgroundImage();
    BuildSearchInputBox();
    BuildSearchButton();
    BuildSlider();
    CreateRoomInfoList();
    BuildRefreshButton();
    BuildCreateButton();
    BuildReturnButton();
    BuildRoomCreateInterface();
    BuildKeyInputInterface();
    BuildPromptBox();
    BuildExitButton();
    BuildPlayNumBox();
    BuildRoomIdBox();
    Init();
  }

  private void BuildBackgroundImage()
  {
    _baseImage = new ScreenMaskUniversalBuilder(transform, true);
    float searchInputAspectRadio = InputBoxUniversalBuilder.GetReferenceAspectRadio(_searchInputType, _searchInputIsDark);
    float searchInputBoxWidth = _referenceMaskPixel.x * searchInputAspectRadio / (searchInputAspectRadio + 1);
    _searchInputBoxHeight = searchInputBoxWidth / searchInputAspectRadio;

    float horizontalGap = (_referenceMatchingBackgroundPixel.x - _referenceMaskPixel.x) / 2;
    _inputboxPosX = (horizontalGap + searchInputBoxWidth / 2) / _referenceMatchingBackgroundPixel.x;
    _searchButtonPosX = (horizontalGap + searchInputBoxWidth + _searchInputBoxHeight / 2) / _referenceMatchingBackgroundPixel.x;

    float buttonWidth = _buttonHeight * ButtonUniversalbuilder.GetReferenceAspectRadio(_buttonType, false);

    float verticalGap = (_referenceMatchingBackgroundPixel.y - _buttonHeight - _referenceMaskPixel.y - _searchInputBoxHeight) / 6;
    _researchPosY = (4 * verticalGap + _buttonHeight + _referenceMaskPixel.y + _searchInputBoxHeight / 2) / _referenceMatchingBackgroundPixel.y;
    _scrollPosY = (3 * verticalGap + _buttonHeight + _referenceMaskPixel.y / 2) / _referenceMatchingBackgroundPixel.y;
    _buttonPosY = (2 * verticalGap + _buttonHeight / 2) / _referenceMatchingBackgroundPixel.y;

    _buttonPosX = (horizontalGap + buttonWidth / 2) / _referenceMatchingBackgroundPixel.x;

    float roomIdWidth = _searchInputBoxHeight * TextBoxUniversalBuilder.GetReferenceAspectRadio(_roomIdType, _roomIdIsDark);
    float playerNumWidth = _searchInputBoxHeight * TextBoxUniversalBuilder.GetReferenceAspectRadio(_playNumBoxType, _playNumBoxIsDark);

    float innerGap = _referenceMaskPixel.x - roomIdWidth - playerNumWidth;
    _roomIdPosX = (horizontalGap + roomIdWidth / 2) / _referenceMatchingBackgroundPixel.x;
    _playNumPosX = (horizontalGap + roomIdWidth + innerGap + playerNumWidth / 2) / _referenceMatchingBackgroundPixel.x;

    _backgroundImage = new ImageGameObjectBuilder("BackgroundImage", _baseImage.Transform);
    _backgroundImage.Build(new Dictionary<string, IComponentBuilder>
    {
      {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
        {
          {"referenceObjectPixels", PixelInfo.referenceScreenPixel},
          {"archorMin", new Vector2(0.5f, 0.5f)},
          {"archorMax", new Vector2(0.5f, 0.5f)},
          {"objectPos", new Vector2(0.5f, 0.5f)},
          {"pivotPos", new Vector2(0.5f, 0.5f)},
        }
      )},
      {"Image", new ImageComponentBuilder(new Dictionary<string, string>
        {
          {"gameObjectName", "BackgroundImage"},
          {"resourcesFolderPath", resourcesFolderPath},
        }
      )}
    });
  }

  private void BuildMatchingBackgroundImage()
  {
    _matchingBackgroundImage = new ImageGameObjectBuilder("MatchingBackground", _backgroundImage.Transform);
    _matchingBackgroundImage.Build(new Dictionary<string, IComponentBuilder>
    {
      {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
        {
          {"referenceObjectPixels", _referenceMatchingBackgroundPixel},
          {"archorMin", new Vector2(0.5f, 0.5f)},
          {"archorMax", new Vector2(0.5f, 0.5f)},
          {"pivotPos", new Vector2(0.5f, 0.5f)},
        }
      )},
      {"Image", new ImageComponentBuilder(new Dictionary<string, string>
        {
          {"gameObjectName", null},
          {"resourcesFolderPath", null},
        }
      )}
    });
  }

  private void BuildSearchInputBox()
  {
    _searchInputBox = new InputBoxUniversalBuilder("InputBox", _matchingBackgroundImage.Transform, new Vector2(_inputboxPosX, _researchPosY), _searchInputType,
      InputBoxUniversalBuilder.GetScaleRadio(_searchInputType, new Vector2(-1, _searchInputBoxHeight), _searchInputIsDark), _searchInputIsDark);
  }

  private void BuildSearchButton()
  {
    _searchButton = new ButtonUniversalbuilder(
      "SearchButton", new Vector2(_searchButtonPosX, _researchPosY), _matchingBackgroundImage.Transform, new Dictionary<string, UnityAction>
      {
        {"onClickAction", OnSearchButtonClicked},
        {"OnHoverAction", null}
      }, ButtonUniversalbuilder.searchButtonIndex, ButtonUniversalbuilder.GetScaleRadio(1, new(-1, _searchInputBoxHeight), true), false, true);
  }

  private void BuildSlider()
  {
    _sliderBuilder = new ScrollRectUniversalBuilder(
      _matchingBackgroundImage.Transform,
      new List<Vector2>
      {
        _referenceMaskPixel,
        new(40, _referenceMaskPixel.y),
        new(0.5f, _scrollPosY),
        new(1.0f, _scrollPosY),
      });
  }

  private void CreateRoomInfoList()
  {
    _roomInfoList = new List<RoomInfoInterface>();
    for (int idx = 0; idx < RoomLimit; idx++)
    {
      RoomInfoInterface temp = new(idx, _sliderBuilder.Transform);
      temp.SetActive(false);
      _roomInfoList.Add(temp);
    }

    _sliderBuilder.SetContent();
  }

  private void BuildRefreshButton()
  {
    Debug.Log("============================================================================" + _buttonPosY);
    _refreshButton = new ButtonUniversalbuilder(
      "RefreshButton", new Vector2(_buttonPosX, _buttonPosY), _matchingBackgroundImage.Transform, new Dictionary<string, UnityAction>
      {
        {"onClickAction", OnRefreshButtonClick},
        {"OnHoverAction", null}
      }, _buttonType, ButtonUniversalbuilder.GetScaleRadio(_buttonType, new(-1, _buttonHeight), false), _buttonIsDark, false
    );
  }

  private void BuildCreateButton()
  {
    _createButton = new ButtonUniversalbuilder(
      "JoinButton", new Vector2(1 - _buttonPosX, _buttonPosY), _matchingBackgroundImage.Transform, new Dictionary<string, UnityAction>
      {
        {"onClickAction", OnCreateButtonClick},
        {"OnHoverAction", null}
      }, _buttonType, ButtonUniversalbuilder.GetScaleRadio(_buttonType, new(-1, _buttonHeight), false), _buttonIsDark, false
    );
  }

  private void BuildReturnButton()
  {
    _returnButton = new ButtonUniversalbuilder(
      "ReturnButton", new Vector2(0.5f, _buttonPosY), _matchingBackgroundImage.Transform, new Dictionary<string, UnityAction>
      {
        {"onClickAction", OnReturnButonClicked},
        {"OnHoverAction", null}
      }, _buttonType, ButtonUniversalbuilder.GetScaleRadio(_buttonType, new(-1, _buttonHeight), false), _buttonIsDark, false
    );
  }

  private void BuildRoomCreateInterface()
  {
    _roomCreateInterface = new RoomCreateInterface(_backgroundImage.Transform);
  }

  private void BuildKeyInputInterface()
  {
    _keyInputInterface = new KeyInputInterface(_backgroundImage.Transform, OnKeyInputComfirmClick);
  }

  private void BuildPromptBox()
  {
    _promptBox = new PromptBoxUniversalBuilder("RoomFullPromptBox", new Vector2(0.5f, 0.5f), _backgroundImage.Transform);
    _promptBox.ModifyButtonContent("Confirm");
    _promptBox.ModifyPromptContent("the room is full");
  }

  private void BuildExitButton()
  {
    float height = 80;
    _exitButton = new ButtonUniversalbuilder(
      "ExitButton", ButtonUniversalbuilder.GetPos(height, _referenceMatchingBackgroundPixel), _matchingBackgroundImage.Transform, new Dictionary<string, UnityAction>
      {
        {"onClickAction", OnExitButtonClick},
        {"OnHoverAction", null}
      }, ButtonUniversalbuilder.returnButtonIndex, ButtonUniversalbuilder.GetScaleRadio(1, new(height, height), true), false, true);
  }

  private void BuildPlayNumBox()
  {
    _playNumBox = new TextBoxUniversalBuilder("PlayNumBox", _matchingBackgroundImage.Transform, new Vector2(_playNumPosX, _researchPosY), _playNumBoxType,
      TextBoxUniversalBuilder.GetScaleRadio(_playNumBoxType, new(-1, _searchInputBoxHeight), _playNumBoxIsDark), _playNumBoxIsDark);
  }

  private void BuildRoomIdBox()
  {
    _roomIdBox = new TextBoxUniversalBuilder("RoomIdBox", _matchingBackgroundImage.Transform, new Vector2(_roomIdPosX, _researchPosY), _roomIdType,
      TextBoxUniversalBuilder.GetScaleRadio(_roomIdType, new(-1, _searchInputBoxHeight), _roomIdIsDark), _roomIdIsDark);
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

    _searchInputBox.InputBoxBuilder.InputFieldBuilder.InputField.InputField.text = null;
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
    _sliderBuilder.SetActive(state);
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
    EventManager.AddListener("OnConnectedServerEvent", OnConnected);
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

    if (_isRoomInfo)
    { // 只有在房间模式才会更新
      RefreshList(_searchInputBox.GetContent());
    }
  }

  // 在密钥输入界面输入密钥后加入房间
  public void OnKeyInputComfirmClick()
  {
    if (password == _keyInputInterface.KeyInputBox.GetContent())
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

  private void OnJoinedRoom(string roomId)
  {
    _isRoomInfo = false;
    _roomCreateInterface.SetActive(false);
    SetActive();
    InRoomUIFresh();
    foreach (Player p in GameLoop.Instance.onlineManager.PlayerListOthers)
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

  private void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable table)
  {
    if (table.TryGetValue("IsReady", out object value))
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
    foreach (KeyValuePair<string, bool> pair in allIsReady)
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
    if (GameLoop.Instance.onlineManager.CurrentRoom == null) { return; }
    int playerCount = GameLoop.Instance.onlineManager.CurrentRoom.PlayerCount;
    GameLoop.Instance.onlineManager.CurrentRoom.CustomProperties.TryGetValue(OnlineManager.RoomNameSearchFilter, out object roomName);
    _playNumBox.ModifyContent(playerCount + " / 8");
    _roomIdBox.ModifyContent((string)roomName);
    for (int i = 0; i < 20; i++)
    {
      _roomInfoList[i].SetActive(i < playerCount);
      _roomInfoList[i].ShiftMode(false);
      if (i < playerCount)
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
        _roomInfoList[i].ModifyRoomContent(roomId, playerCount, (string)filter);
        Debug.Log(rooms[i].CustomProperties.TryGetValue(OnlineManager.RoomPasswordFilter, out object T) + (string)T);
        if (rooms[i].CustomProperties.TryGetValue(OnlineManager.RoomPasswordFilter, out object passwordKey))
        {
          _roomInfoList[i].ModifyRoomContent(roomId, playerCount, (string)filter, (string)passwordKey);
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
      GameLoop.Instance.onlineManager.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable { { "StartGame", true } });
      return;
    }

    isReady = !isReady;
    _returnButton.ModifyContent(isReady ? "已准备" : "准备");
    GameLoop.Instance.onlineManager.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable { { "IsReady", isReady } });
  }

  private void CallKeyInput(string password)
  {
    Debug.Log("CallKeyInput");
    _keyInputInterface.SetActive(true);
    this.password = password;
    EventManager.RemoveListener<string>("CallKeyInput", CallKeyInput);
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
