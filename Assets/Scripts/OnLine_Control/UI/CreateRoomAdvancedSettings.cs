//Author : _SourceCode
//CreateTime : 2025-03-04-20:49:40
//Version : 1.0
//UnityVersion : 2022.3.53f1c1


using Photon.Realtime;
using UnityEngine.UI;

public class CreateRoomAdvancedSettings : UIObject
{
    private RoomOptions roomOptions;

    private CreateRoomUI createRoomUI = null;

    private InputField passwordInputField;
    private InputField roomIDInputField;

    private Toggle needPasswordToggle;
    private Toggle needRoomIDToggle;
    private Toggle isVisibleTongle;

    private bool needPassword = false;
    private bool needRoomID = false;
    private bool isVisible = true;
    public override void OnLoad()
    {
        transform.Find("bg/title/closeBtn").GetComponent<Button>().onClick.AddListener(OnCloseBtnClicked);
        transform.Find("bg/okBtn").GetComponent<Button>().onClick.AddListener(OnOKBtnClicked);
        passwordInputField = transform.Find("bg/InputField/passwordInputField").GetComponent<InputField>();
        roomIDInputField = transform.Find("bg/InputField/roomIDInputField").GetComponent<InputField>();
        needPasswordToggle = transform.Find("bg/Toggle/UsePassword").GetComponent<Toggle>();
        needRoomIDToggle = transform.Find("bg/Toggle/UserRoomID").GetComponent<Toggle>();
        isVisibleTongle = transform.Find("bg/Toggle/IsVisible").GetComponent<Toggle>();

        needPasswordToggle.onValueChanged.AddListener(OnNeedPasswordToggleValueChange);
        needRoomIDToggle.onValueChanged.AddListener(OnNeedRoomIDToggleValueChange);
        isVisibleTongle.onValueChanged.AddListener(OnIsVisiableToggleValueChange);
    }

    private void OnNeedPasswordToggleValueChange(bool value)
    {
        passwordInputField.gameObject.SetActive(value);
        needPasswordToggle.transform.Find("Background/Checkmark").gameObject.SetActive(value);
        needPassword = value;
    }
    private void OnNeedRoomIDToggleValueChange(bool value)
    {
        roomIDInputField.gameObject.SetActive(value);
        needRoomIDToggle.transform.Find("Background/Checkmark").gameObject.SetActive(value);
        needRoomID = value;
    }
    private void OnIsVisiableToggleValueChange(bool value)
    {
        isVisible = value;  
    }
    public void Init(CreateRoomUI createRoomUI)
    {
        this.createRoomUI = createRoomUI;
        roomOptions = createRoomUI.CreateDefultOption();
        UI_Manager.Instance.HideUI(createRoomUI.gameObject.name);
    }
    public void OnCloseBtnClicked()
    {
        UI_Manager.Instance.CloseUI(this.name);
    }

    public override void OnClose()
    {
        base.OnClose();
        UI_Manager.Instance.ShowUI<CreateRoomUI>(createRoomUI.gameObject.name);
    }
    public void OnOKBtnClicked()
    {
        if (needPassword && passwordInputField.text!=null && passwordInputField.text.Length >0) {
            roomOptions.CustomRoomProperties.Add("password", passwordInputField.text);
        }
        if (needRoomID && roomIDInputField.text != null && roomIDInputField.text.Length > 0) { 
            createRoomUI.selfCreateRoomID = true;
            createRoomUI.roomID = roomIDInputField.text;
        }
        roomOptions.IsVisible = isVisible;
        UI_Manager.Instance.CloseUI(this.name);

        createRoomUI.roomOptions = roomOptions;
    }
}
