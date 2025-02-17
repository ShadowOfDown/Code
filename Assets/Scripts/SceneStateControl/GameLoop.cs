using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameLoop : MonoBehaviour
{
    [SerializeField]
    private SceneStateControl sceneStateControl = new SceneStateControl();
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
        
    }

    private void Start()
    {
        sceneStateControl.SetState(new SceneStartState(sceneStateControl),"StartScene");
    }

    private void Update()
    {
        sceneStateControl.StateUpdate();
    }
}
