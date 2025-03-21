using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class SceneStateControl
{
    [SerializeField]
    private ISceneState currentState = null;
    [SerializeField]
    private bool stateBegin =false;
    [SerializeField]
    private bool isLoadingScene = false;
    public float loadSceneProgress = 0f;

    public void SetState(ISceneState state,string SceneName)
    {
        Debug.Log("SetState : " + state.ToString());
        stateBegin =false;
        if (!isLoadingScene)
        {
            IEnumeratorSystem.Instance.startCoroutine(LoadScene(SceneName), "LoadScene : " + SceneName);
        }

        if (currentState != null)
        {
            currentState.StateEnd();
        }

        currentState = state;
    }
    IEnumerator LoadScene(string SceneName)
    {
        if (SceneName == null || SceneName.Length == 0) { yield break; }
        if (isLoadingScene) { yield break; }
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneName);
        isLoadingScene = true;
        yield return new WaitUntil(() => { loadSceneProgress = asyncOperation.progress; return asyncOperation.isDone; });
        loadSceneProgress = 0;
        isLoadingScene = false ;
    }



    public void StateUpdate()
    {
        if(currentState == null||isLoadingScene) { return; }
        if (!stateBegin) {
            currentState.StateBegin();
            stateBegin = true;
        }
        currentState.StateUpdate();
    }
}
