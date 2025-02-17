//Author : _SourceCode
//CreateTime : 2025-02-17-20:01:11
//Version : 1.0
//UnityVersion : 2021.3.45f1c1


using Photon.Chat.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskUI : MonoBehaviour
{
    private Text text;
    public void ShowMessage(string message)
    {
        if(text == null)
        {
            text = transform.Find("msg/bg/Text").GetComponent<Text>();
        }
        text.text = message;
    }
}
