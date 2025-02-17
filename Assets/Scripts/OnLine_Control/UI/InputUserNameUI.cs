//Author : _SourceCode
//CreateTime : 2025-02-17-20:34:36
//Version : 1.0
//UnityVersion : 2021.3.45f1c1


using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputUserNameUI : MonoBehaviour
{
    public InputField inputField;
    public void Awake()
    {
        transform.Find("bg/title/closeBtn").GetComponent<Button>().onClick.AddListener(OnCloseBtnClicked);
        transform.Find("bg/okBtn").GetComponent<Button>().onClick.AddListener(OnSetBtnClicked);
        inputField = transform.Find("bg/InputField").GetComponent<InputField>();
        if (OnLine_Manager.Instance.PlayerName == null || OnLine_Manager.Instance.PlayerName.Length == 0)
        {
            inputField.text = "User" + Random.Range(1, 99999);
        }
        inputField.text = OnLine_Manager.Instance.PlayerName;
    }

    public void OnCloseBtnClicked()
    {
        if (inputField.text == null && (OnLine_Manager.Instance.PlayerName == null || OnLine_Manager.Instance.PlayerName.Length == 0))
        {
            OnLine_Manager.Instance.SetName("User" + Random.Range(1, 99999));
        }
        UI_Manager.Instance.CloseUI("InputUserNameUI");
    }
    public void OnSetBtnClicked()
    {
        if (inputField.text.Length < 2)
        {
            UI_Manager.Instance.ShowUI<MaskUI>("MaskUI").ShowMessage("输入字符不得少于2");
            return;
        }
        OnLine_Manager.Instance.SetName(inputField.text);
        UI_Manager.Instance.CloseUI("InputUserNameUI");
    }
}
