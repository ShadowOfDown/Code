//Author : _SourceCode
//CreateTime : 2025-02-18-18:07:53
//Version : 1.0
//UnityVersion : 2021.3.45f1c1


using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
    public string OwnerID
    {
        get; internal set;
    }
    public bool isReady = false;
    public Button ReadyButton;
    public Text PlayerName;
    public Text WhetherReady;
    public Image PlayerImage;

    public void Init(Player player)
    {
        GetComs();
        this.OwnerID = player.UserId;
        PlayerName.text = player.IsMasterClient ? player.NickName + "\n(房主)" : player.NickName;
        PlayerImage.sprite = player.IsMasterClient ? Resources.Load<Sprite>("UI/Pictures/Owner") : Resources.Load<Sprite>("UI/Pictures/User");

        if (player.UserId == PhotonNetwork.LocalPlayer.UserId)
        {
            ReadyButton.onClick.AddListener(OnReadyButtonClicked);
        }
        else
        {
            ReadyButton.gameObject.GetComponent<Image>().color = Color.black;
            ReadyButton.enabled = false;
        }
    }
    private void GetComs()
    {
        ReadyButton = transform.Find("Button").GetComponent<Button>();
        PlayerName = transform.Find("Name").GetComponent<Text>();
        WhetherReady = ReadyButton.transform.Find("Text").GetComponent<Text>();
        PlayerImage = transform.Find("Image").GetComponent<Image>();
        WhetherReady.text = "准备";
    }
    private void OnReadyButtonClicked()
    {
        isReady = !isReady;
        ChangeUI();
        ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable();
        table.Add("isReady", isReady);
        PhotonNetwork.LocalPlayer.SetCustomProperties(table);
    }

    public void ChangeUI()
    {
        WhetherReady.text = isReady ? "已准备" : "准备";
    }
}
