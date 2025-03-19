//Author : _SourceCode
//CreateTime : 2025-03-18-15:57:12
//Version : 1.0
//UnityVersion : 2022.3.53f1c1

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    SceneStart,
    RoleSelect,
    Decrypt,
    End,
}

public struct RoleInfo
{
    public string name;
    public short faction;
    public AllRoles ID;
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
    public Dictionary<string , RoleInfo> roles = new Dictionary<string , RoleInfo>();
    public AllRoles myRole;
    public GameScene(SceneStateControl control) : base(control)
    {
        this.StateName = "GameScene";
    }

    public override void StateBegin() { 
        UI_Manager.Instance.Init();
        UI_Manager.Instance.ShowUI<RoleSelectScene>("RoleSelectUI");
    }
    public override void StateEnd() { 

    }
    public override void StateUpdate() { 

    }

    public void SwitchGameState(GameState state)
    {
        currentGameState = state;
    }
    public void SwitchGameState()
    {
        currentGameState++;
    }
    private void CheckGameState()
    {
        if(currentGameState == GameState.End)
        {
            SwitchScene();
        }
    }

    public void SwitchScene()
    {

    }

    private void OnPlayerSelectRole(string playerID , RoleInfo roleInfo)
    {
        if(roles.ContainsValue(roleInfo))
        roles.Add(playerID, roleInfo);
    }
}
