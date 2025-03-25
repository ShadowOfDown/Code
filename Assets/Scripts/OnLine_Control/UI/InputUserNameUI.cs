//Author : _SourceCode
//CreateTime : 2025-02-17-20:34:36
//Version : 1.0
//UnityVersion : 2021.3.45f1c1


using Photon.Pun.Demo.PunBasics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputUserNameUI : UIObject
{
    public InputField inputField;
    public event Action OnNameChanged;
    public override void OnLoad()
    {
        transform.Find("bg/title/closeBtn").GetComponent<Button>().onClick.AddListener(OnCloseBtnClicked);
        transform.Find("bg/okBtn").GetComponent<Button>().onClick.AddListener(OnSetBtnClicked);
        inputField = transform.Find("bg/InputField").GetComponent<InputField>();
        
        if (GameLoop.Instance.onlineManager.PlayerName == null || GameLoop.Instance.onlineManager.PlayerName.Length == 0){
            inputField.text = "User_" + UnityEngine.Random.Range(1, 99999);
        }
        else{
            Debug.Log(GameLoop.Instance.onlineManager.PlayerName + "   Length  : "+ GameLoop.Instance.onlineManager.PlayerName.Length);
            inputField.text = GameLoop.Instance.onlineManager.PlayerName;
        }
    }

    public void OnCloseBtnClicked()
    {
        if (GameLoop.Instance.onlineManager.PlayerName == null || GameLoop.Instance.onlineManager.PlayerName.Length == 0){
            if (inputField.text == null || inputField.text.Length <= 2)
            {
                GameLoop.Instance.onlineManager.SetName("User_" + UnityEngine.Random.Range(1, 99999));
            }
            else {
                GameLoop.Instance.onlineManager.SetName(inputField.text);
            }
            OnNameChanged?.Invoke();
        }
        UI_Manager.Instance.CloseUI("InputUserNameUI");
    }
    public void OnSetBtnClicked()
    {
        if (inputField.text.Length < 2)
        {
            UI_Manager.Instance.LogWarnning("输入字符不得少于2");
            return;
        }
        GameLoop.Instance.onlineManager.SetName(inputField.text);
        OnNameChanged?.Invoke();
        UI_Manager.Instance.CloseUI("InputUserNameUI");
    }
}
