//Author : _SourceCode
//CreateTime : 2025-02-17-22:09:00
//Version : 1.0
//UnityVersion : 2021.3.45f1c1

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    #region SingleTon
    private static GameLoop instance;
    private static object locker = new object();
    public static GameLoop Instance{
        get{
            if(instance == null){
                lock(locker){
                    if(instance == null){
                        Debug.Log("GameLoop SingleTon Created !");
                        GameObject go = new GameObject("GameLoop");
                        instance = go.AddComponent<GameLoop>();
                        DontDestroyOnLoad(go);
                    }
                }
            }
            return instance;
        }
    }
    #endregion

    private GameLoop(){}
    private SceneStateControl sceneStateControl = new SceneStateControl();
    public OnlineManager onlineManager;
    [SerializeField]
    private UI_Manager ui_manager;
    private void Awake()
    {
        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);

        onlineManager = gameObject.AddComponent<OnlineManager>();

        ui_manager = UI_Manager.Instance;
        ui_manager.Init();
    }

    private void Start()
    {
        sceneStateControl.SetState(new SceneStartState(sceneStateControl), "StartScene");
    }

    private void Update()
    {
        sceneStateControl.StateUpdate();
    }
}
