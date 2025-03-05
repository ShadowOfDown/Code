//Author : _SourceCode
//CreateTime : 2025-02-14-23:47:23
//Version : 1.0
//UnityVersion : 2021.3.45f1c1

using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class SceneStartState : ISceneState
{
    public SceneStartState (SceneStateControl control):base(control){
        this.StateName = "SceneStartState";
    }


    public override void StateBegin()
    {
        UI_Manager.Instance.Init();
        UI_Manager.Instance.ShowUI<LoginUI>("LoginUI");
        EventManager.AddListener<Player, ExitGames.Client.Photon.Hashtable>("OnPlayerPropertiesUpdateEvent", SwitchScene);
    }
    public override void StateEnd()
    {
        EventManager.RemoveListener<Player, ExitGames.Client.Photon.Hashtable>("OnPlayerPropertiesUpdateEvent", SwitchScene);
    }
    public override void StateUpdate()
    {

    }

    private void SwitchScene(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps != null && changedProps.ContainsKey("StartGame"))
        {
            UI_Manager.Instance.CloseAllUI();
            my_control.SetState(new RoleSelectScene(my_control), "RoleSelectScene");
        }
    }
}
