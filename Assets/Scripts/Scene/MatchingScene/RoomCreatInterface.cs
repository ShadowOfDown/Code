using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using Photon.Realtime;

class RoomCreateInterface : IInterfaceBuilder
{
  const int RoomIDBit = 6;

  #region Fields
  private ScreenMaskUniversalBuilder _screenMask;
  private TextBoxUniversalBuilder _boxBuilder;
  private ButtonUniversalbuilder _exitButton;
  private CheckBoxUniversalBuilder _checkBox;
  private InputBoxUniversalBuilder _roomIdInputBox;
  private ButtonUniversalbuilder _comfirmButton;
  private TextGameObjectBuilder _promptText;
  private ButtonTextGameObjectBuilder _keyInputButton;
  private KeyInputInterface _keyInputInterface;

  // 提取构造函数中的局部变量为私有变量
  private int _boxtype;
  private bool _boxIsDark;
  private float _boxWidth;
  private float _boxHeight;

  private int _roomIdInputFieldType;
  private bool _roomIdinputFieldIsDark;
  private float _roomIdWidth;
  private float _stdHeight_01;
  private float _stdHeight_02;

  private int _comfirmButtonType;
  private bool _comfirmButtonIsDark;
  private float _comfirmButtonWidth;

  private float _referenceGapX_01;
  private float _referenceGapX_02;
  private float _referenceGapY;
  private float _roomIdPosX;
  private float _linePosY_01;
  private float _comfirmBoxButtonPosX;

  private float _checkBoxWidth;
  private float _checkBoxPosX;
  private float _linePosY_02;
  private float _promptTextWidth;
  private float _promptTextPosX;

  private int _keyInputButtonType;
  private bool _keyInputButtonIsDark;
  private float _keyInputButtonWidth;
  private float _KeyInputBoxPosX;
  #endregion

  #region Methods
  public RoomCreateInterface(Transform parentTransform)
  {
    _boxtype = 4;
    _boxIsDark = false;
    _boxWidth = 1680;
    _boxHeight = _boxWidth / TextBoxUniversalBuilder.GetReferenceAspectRadio(_boxtype, _boxIsDark);
    Debug.Log($"========================================================================================={_boxHeight}, {_boxWidth}");

    // 第一行: roomIdInputField, comfirmButton
    _roomIdInputFieldType = 4;
    _roomIdinputFieldIsDark = true;
    _roomIdWidth = 840;
    _stdHeight_01 = _roomIdWidth / InputBoxUniversalBuilder.GetReferenceAspectRadio(_roomIdInputFieldType, _roomIdinputFieldIsDark);
    _stdHeight_02 = _stdHeight_01 * 0.6f;

    _comfirmButtonType = 3;
    _comfirmButtonIsDark = false;
    _comfirmButtonWidth = _stdHeight_01 * ButtonUniversalbuilder.GetReferenceAspectRadio(_comfirmButtonType, _comfirmButtonIsDark);

    _referenceGapX_01 = (_boxWidth - _roomIdWidth - _comfirmButtonWidth) / 3;
    _referenceGapY = (_boxHeight - _stdHeight_01 * 2) / 7;
    _roomIdPosX = (_referenceGapX_01 + _roomIdWidth / 2) / _boxWidth;
    _linePosY_01 = (_referenceGapY * 4 + _stdHeight_01 * 3 / 2) / _boxHeight;
    _comfirmBoxButtonPosX = (_roomIdWidth + _comfirmButtonWidth / 2 + _referenceGapX_01 * 2f) / _boxWidth;

    // 第二行: checkbox, prompttext, keyinputbutton
    
    _checkBoxWidth = _stdHeight_02 * 0.8f;
    _promptTextWidth = _stdHeight_02 * 2;
    _keyInputButtonType = 1;
    _keyInputButtonIsDark = false;
    _keyInputButtonWidth = _stdHeight_02 * InputBoxUniversalBuilder.GetReferenceAspectRadio(_keyInputButtonType, _keyInputButtonIsDark);
    
    _referenceGapX_02 = (_roomIdWidth - _checkBoxWidth - _keyInputButtonWidth - _promptTextWidth) / 2;
    _linePosY_02 = (_referenceGapY * 4 + _stdHeight_02 / 2) / _boxHeight;
    _checkBoxPosX = (_referenceGapX_01 + _checkBoxWidth / 2) / _boxWidth;
    _promptTextPosX = (_referenceGapX_01 + _referenceGapX_02 + _checkBoxWidth + _promptTextWidth / 2) / _boxWidth;
    _KeyInputBoxPosX = (_referenceGapX_01 + _referenceGapX_02 * 2 + _checkBoxWidth + _promptTextWidth + _keyInputButtonWidth / 2) / _boxWidth;

    BuildScreenMask(parentTransform);
    BuildBoxBuilder();
    BuildExitButton();
    BuildRoomIdInputBox();
    BuildComfirmButton();
    BuildPromptText();
    BuildCheckBox();
    BuildKeyInputButton();
    BuildKeyInputInterface();
  }

  private void BuildScreenMask(Transform parentTransform)
  {
    _screenMask = new ScreenMaskUniversalBuilder(parentTransform);
  }

  private void BuildBoxBuilder()
  {
    _boxBuilder = new TextBoxUniversalBuilder("InputBox", _screenMask.Transform, new Vector2(0.5f, 0.5f), _boxtype,
      TextBoxUniversalBuilder.GetScaleRadio(_boxtype, new Vector2(_boxWidth, -1), _boxIsDark), _boxIsDark);
  }

  private void BuildExitButton()
  {
    _exitButton = new ButtonUniversalbuilder(
      "ExitButton", new Vector2(1.0f, 1.0f), _boxBuilder.Transform, new Dictionary<string, UnityAction>
      {
        {"onClickAction", OnExitButtonClick},
        {"OnHoverAction", null}
      }, ButtonUniversalbuilder.returnButtonIndex, ButtonUniversalbuilder.GetScaleRadio(ButtonUniversalbuilder.returnButtonIndex, new(120, 120), true), !_boxIsDark, true);
  }

  private void BuildRoomIdInputBox()
  {
    _roomIdInputBox = new InputBoxUniversalBuilder("RoomIdImputBox", _boxBuilder.Transform, new Vector2(_roomIdPosX, _linePosY_01), _roomIdInputFieldType,
      InputBoxUniversalBuilder.GetScaleRadio(_roomIdInputFieldType, new(_roomIdWidth, _stdHeight_01), _roomIdinputFieldIsDark), _roomIdinputFieldIsDark);
  }

  private void BuildComfirmButton()
  {
    _comfirmButton = new ButtonUniversalbuilder(
      "ComfirmButtom", new Vector2(_comfirmBoxButtonPosX, _linePosY_01), _boxBuilder.Transform, new Dictionary<string, UnityAction>
      {
        {"onClickAction", OnConfirmButtonClick},
        {"OnHoverAction", null}
      }, _comfirmButtonType, ButtonUniversalbuilder.GetScaleRadio(_comfirmButtonType, new(_comfirmButtonWidth, -1), _comfirmButtonIsDark), !_comfirmButtonIsDark, false);
  }

  private void BuildPromptText()
  {
    Vector2 referencePixel = new(_promptTextWidth, _stdHeight_02);
    Vector2 referencePos = new(_promptTextPosX, _linePosY_02);
    _promptText = new TextGameObjectBuilder("PromptText", _boxBuilder.Transform);
    _promptText.Build(new Dictionary<string, IComponentBuilder>
      {
        {"Text", new TextComponentBuilder(TextBoxUniversalBuilder.textArgu_02)},
        {"RectTransform", new RectTransformComponentBuilder(new Dictionary<string, Vector2>
          {
            {"referenceObjectPixels", referencePixel},
            {"archorMin", referencePos},
            {"archorMax", referencePos},
            {"pivotPos", new Vector2(0.5f, 0.5f)},
          }
        )}
      }
    );

    _promptText.ModifyContent("*******");
  }

  private void BuildCheckBox()
  {
    _checkBox = new CheckBoxUniversalBuilder("IsKeyNeed", new Vector2(_checkBoxPosX, _linePosY_02), _boxBuilder.Transform, 
     CheckBoxUniversalBuilder.getScaleRadio(new(_checkBoxWidth, -1)), OnCheckedButtonClick);
  }

  private void BuildKeyInputButton()
  {
    _keyInputButton = new ButtonTextGameObjectBuilder(new Dictionary<string, string>
    {
      {"Button", "keyInputButton"},
      {"Text", "key"},
    }, _boxBuilder.Transform);

    _keyInputButton.Build(new SortedDictionary<string, Dictionary<string, IComponentBuilder>>
      {
        {"Button", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform", new RectTransformComponentBuilder(
              new Dictionary<string, Vector2>
              {
                {"referenceObjectPixels", new Vector2(_keyInputButtonWidth, _stdHeight_02)},
                {"archorMin", new Vector2(_KeyInputBoxPosX, _linePosY_02)},
                {"archorMax", new Vector2(_KeyInputBoxPosX, _linePosY_02)},
                {"pivotPos", new Vector2(0.5f, 0.5f)},
              }
            )},
            {"Image" , new ImageComponentBuilder(
              new Dictionary<string, string>
              {
                {"gameObjectName", InputBoxUniversalBuilder.GetInputFieldImageName(_keyInputButtonType, _keyInputButtonIsDark)},
                {"resourcesFolderPath", InputBoxUniversalBuilder.resourcesFolderPath}
              }
            )},
            {"Button", new ButtonComponentBuilder(
              new Dictionary<string, string>
              {
                {"gameObjectName", null},
                {"resourcesFolderPath", InputBoxUniversalBuilder.resourcesFolderPath}
              }
            )},
            {"EventTrigger", new EventTriggerComponentBuilder(new Dictionary<string, UnityAction>
              {
                {"onClickAction", OnKeyInputButtonClick},
                {"OnHoverAction", null}
              }
            )}
          }
        },
        {"Text", new Dictionary<string, IComponentBuilder>
          {
            {"RectTransform" , new RectTransformComponentBuilder(
              new Dictionary<string, Vector2>
              {
                {"referenceObjectPixels", new Vector2(_keyInputButtonWidth, _stdHeight_02) * 0.9f},
                {"archorMin", new Vector2(0.5f, 0.5f)},
                {"archorMax", new Vector2(0.5f, 0.5f)},
                {"pivotPos", new Vector2(0.5f, 0.5f)},
              }
            )},
            {"Text", new TextComponentBuilder(TextBoxUniversalBuilder.textArgu_02)},
            {"ContentSizeFitter", new ContentSizeFitterComponentBuilder(
              new Dictionary<string, bool>
              {
                {"horizontalFit", false},
                {"verticalFit", false},
              }
            )},
          }
        }
      }
    );
  }

  private void BuildKeyInputInterface()
  {
    _keyInputInterface = new KeyInputInterface(_boxBuilder.Transform, OnKeyInputComfirmClick);
    _keyInputInterface.SetActive(false);
    _keyInputButton.SetActive(false);
  }

  public void SetActive(bool state)
  {
    _screenMask.SetActive(state);
    _boxBuilder.SetActive(state);
    _exitButton.SetActive(state);
    _checkBox.SetActive(state);
    _roomIdInputBox.SetActive(state);
    _comfirmButton.SetActive(state);
    _promptText.SetActive(state);
    _keyInputButton.SetActive(state & _checkBox.Checked);
  }

  // 勾选控制密钥输入是否出现
  private void OnCheckedButtonClick()
  {
    DebugInfo.Print("checked button clicked");
    _keyInputButton.SetActive(!_checkBox.Checked);
  }

  // 点击关闭按钮后退出弹窗
  private void OnExitButtonClick()
  {
    DebugInfo.Print("exit button clicked");
    SetActive(false);
  }

  // 启动密钥后开启密钥输入界面
  private void OnKeyInputButtonClick()
  {
    DebugInfo.Print("key input key clicked");
    _keyInputInterface.SetActive(true);
  }

  // 点击确认后保存密钥
  private void OnConfirmButtonClick()
  {
    DebugInfo.Print("confirm button clicked");
    RoomOptions roomOptions = CreateDefultOption();
    roomOptions.CustomRoomProperties[OnlineManager.RoomNameSearchFilter] = _roomIdInputBox.GetContent();
    string roomId = CreateRoomID();
    roomOptions.CustomRoomProperties[OnlineManager.RoomIDSearchFilter] = roomId;
    if (_checkBox.Checked)
    {
      roomOptions.CustomRoomProperties.Add(OnlineManager.RoomPasswordFilter, _keyInputInterface.GetContent());
    }
    GameLoop.Instance.onlineManager.CreateRoom(roomId, roomOptions);
    EventManager.AddListener<short, string>("OnCreatedRoomFailedEvent", OnCreateRoomFailed);
    EventManager.AddListener<string>("OnJoinRoomEvent", OnJoinRoom);
  }

  private void OnJoinRoom(string s)
  {
    EventManager.RemoveListener<string>("OnJoinRoomEvent", OnJoinRoom);
    EventManager.RemoveListener<short, string>("OnCreatedRoomFailedEvent", OnCreateRoomFailed);
  }

  public void OnCreateRoomFailed(short returnCode, string message)
  {
    EventManager.RemoveListener<short, string>("OnCreatedRoomFailedEvent", OnCreateRoomFailed);
    EventManager.RemoveListener<string>("OnJoinRoomEvent", OnJoinRoom);
    if (returnCode == 32766)
    {
      RoomOptions roomOptions = CreateDefultOption();
      roomOptions.CustomRoomProperties[OnlineManager.RoomNameSearchFilter] = _roomIdInputBox.GetContent();
      string roomId = CreateRoomID();
      roomOptions.CustomRoomProperties[OnlineManager.RoomIDSearchFilter] = roomId;
      if (!_checkBox.Checked)
      {
        roomOptions.CustomRoomProperties.Add(OnlineManager.RoomPasswordFilter, _keyInputInterface.GetContent());
      }
      GameLoop.Instance.onlineManager.CreateRoom(roomId, roomOptions);
      return;
    }
    UI_Manager.Instance.LogWarnning("创建房间失败");
  }

  private string CreateRoomID()
  {
    System.Random random = new System.Random();
    int id = random.Next((int)Mathf.Pow(10, RoomIDBit - 1), (int)Mathf.Pow(10, RoomIDBit) - 1);
    return id.ToString();
  }

  // 输入完成确认后创建房间
  private void OnKeyInputComfirmClick()
  {
    DebugInfo.Print("key input confirmed");
    _keyInputInterface.SetActive(false);
  }

  public RoomOptions CreateDefultOption()
  {
    RoomOptions roomOptions = new RoomOptions
    {
      MaxPlayers = 8,
      PublishUserId = true
    };

    ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable
      {
        { OnlineManager.RoomNameSearchFilter, GameLoop.Instance.onlineManager.PlayerName + "的房间" },
        { OnlineManager.RoomIDSearchFilter, "1" }
      };
    roomOptions.CustomRoomProperties = table;
    roomOptions.CustomRoomPropertiesForLobby = new string[] { OnlineManager.RoomIDSearchFilter, OnlineManager.RoomNameSearchFilter, OnlineManager.RoomPasswordFilter };
    return roomOptions;
  }
  #endregion
}