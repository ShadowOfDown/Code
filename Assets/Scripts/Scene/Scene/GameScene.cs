//Author : _SourceCode
//CreateTime : 2025-03-18-15:57:12
//Version : 1.0
//UnityVersion : 2022.3.53f1c1

using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static GameSceneStart;

public abstract class StateOfGameScene
{
    protected GameScene gameScene;
    protected GameState gameState;
    public StateOfGameScene(GameScene gameScene, GameState gameState)
    {
        this.gameScene = gameScene;
        this.gameState = gameState;
        Debug.Log("Switch To GameState : " + gameState);
    }

    public abstract void StateBegin();
    public abstract void StateEnd();
    public abstract void StateUpdate();
}

public class GameSceneStart : StateOfGameScene
{
    public GameSceneStart(GameScene gameScene, GameState gameState) : base(gameScene, gameState)
    {
        
    }

    public override void StateBegin()
    {
        UI_Manager.Instance.Init();
    }

    public override void StateEnd()
    {
        gameScene.SwitchGameState();
    }

    public override void StateUpdate()
    {
        
    }
}


public enum GameState
{
    SceneStart,
    RoleSelect,
    DramaPerform,
    Decrypt,
    End,
}

public class RoleDialogueContainer : ScriptableObject
{
    public List<DialogueStruct> dialogues = new List<DialogueStruct>();
}

[Serializable]
public struct RoleInfo
{
    public string name;
    public short faction;
    public AllRoles ID;
    public RoleDialogueContainer dialogues;
}

public enum AllRoles
{
    Xia,
    Lin,
    Zhang,
    Xu,
    Guan,
    Fang,
    Jiang,
    Ning
}
public class GameScene : ISceneState
{
    private GameState currentGameState = GameState.SceneStart;
    private StateOfGameScene currentState;
    private Dictionary<GameState,StateOfGameScene> gameStates = new Dictionary<GameState,StateOfGameScene>();
    public Dictionary<string , RoleInfo> selectedRoles = new Dictionary<string , RoleInfo>();
    public Dictionary<AllRoles,RoleInfo> AllRoleInfo = new Dictionary<AllRoles,RoleInfo>();
    public AllRoles myRole;
    public GameScene(SceneStateControl control) : base(control)
    {
        this.StateName = "GameScene";
    }

    public override void StateBegin() {
        InportData();
        Init();
    }
    public override void StateEnd() { 
        RemoveAllListenners();
    }
    public override void StateUpdate() { 
        currentState?.StateUpdate();
    }

    public void SwitchGameState(GameState state)
    {
        if (currentState != null)
        {
            currentState.StateEnd();
        }
        currentGameState = state;
        currentState = gameStates[currentGameState];
        currentState.StateBegin();
    }
    public void SwitchGameState()
    {
        currentGameState++;
        if (currentState != null) { 
            currentState.StateEnd();
        }
        currentState = gameStates[currentGameState];
        currentState.StateBegin();
    }

    public void SwitchScene()
    {

    }

    private void InportData()
    {
        //AllRolesInfo 
        List<RoleInfo> roleInfos = ((AllRolesBulider)(Resources.Load("ProjectData/RoleInfo/AllRoleInfo/AllRolesInfo"))).roleInfos;
        foreach (RoleInfo roleInfo in roleInfos) { 
            AllRoleInfo.Add(roleInfo.ID, roleInfo);
        }


    }

    private void Init()
    {
        EventManager.AddListener<Player, ExitGames.Client.Photon.Hashtable>("OnPlayerPropertiesUpdateEvent", OnPlayerSelectRoles);

        gameStates.Add(GameState.SceneStart, new GameSceneStart(this, GameState.SceneStart));

        SwitchGameState(GameState.SceneStart);
    }

    private void RemoveAllListenners()
    {
        EventManager.RemoveListener<Player, ExitGames.Client.Photon.Hashtable>("OnPlayerPropertiesUpdateEvent", OnPlayerSelectRoles);
    }

    private void OnPlayerSelectRoles(Player targetPlayer,ExitGames.Client.Photon.Hashtable table)
    {
        if(!table.TryGetValue<AllRoles>("SelectRole", out AllRoles role)) { return; }
        if (selectedRoles.ContainsValue(AllRoleInfo[role]))
        {
            EventManager.BroadCast<Player>("RoleHasSelectedEvent",targetPlayer);
            return;
        }
        selectedRoles.Add(targetPlayer.UserId, AllRoleInfo[role]);
        if(targetPlayer.UserId == GameLoop.Instance.onlineManager.LocalPlayer.UserId)
        {
            myRole = role;
        }
        EventManager.BroadCast<Player>("SucessfullySelectRoleEvent", targetPlayer);
        if(selectedRoles.Count == AllRoleInfo.Count)
        {
            SwitchGameState();
        }
    }
}
