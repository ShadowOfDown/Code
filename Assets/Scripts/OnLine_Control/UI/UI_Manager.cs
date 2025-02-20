//Author : _SourceCode
//CreateTime : 2025-02-17-19:11:53
//Version : 1.0
//UnityVersion : 2021.3.45f1c1

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class UI_Manager
{
    #region SingleTon
    private static UI_Manager instance;
    private static object locker = new object();
    public static UI_Manager Instance{
        get{
            if(instance == null){
                lock(locker){
                    if(instance == null){
                        Debug.Log("UI_Manager SingleTon Created !");
                        instance = new UI_Manager();
                    }
                }
            }
            return instance;
        }
    }
    #endregion

    private UI_Manager(){}
    [SerializeField]
    private List<GameObject> uiList;
    public Transform CanvasTf { get; private set; }

    public void Init()
    {
        CanvasTf = GameObject.Find("canvas")?.transform;
        if(CanvasTf == null)
        {
            CanvasTf = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas")).transform;
        }
        uiList = new List<GameObject>();
        ShowUI<LoginUI>("LoginUI");
    }

    private GameObject Find(string name)
    {
        foreach (GameObject go in uiList)
        {
            if (go.name == name) return go;
        }
        return null;
    }

    private T Find<T>(string name) where T : Component
    {
        foreach(GameObject go in uiList)
        {
            if (go.name == name) return go.GetComponent<T>();
        }
        return null;
    }
    public T ShowUI<T>(string Name) where T : Component
    {
        T ui = Find<T>(Name);
        if(ui == null)
        {
            GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + Name),CanvasTf) ;
            go.name = Name;
            uiList.Add(go);
            ui = go.AddComponent<T>();
        }
        else
        {
            ui.gameObject.SetActive(true);
        }
        return ui;
    }

    public void CloseUI(string Name)
    {
        GameObject go = Find(Name);
        if(go == null) return;
        uiList.Remove(go);
        GameObject.Destroy(go);
    }

    public void HideUI(string Name) { 
        GameObject go = Find(Name);
        if(go == null) return;
        go.SetActive(false);
    }

    public void CloseAllUI()
    {
        foreach (GameObject go in uiList)
        {
            GameObject.Destroy(go);
        }
        uiList.Clear();
    }

    public void LogWarnning(string message) { 
        ShowUI<MaskUI>("MaskUI").ShowMessage(message);
        IEnumeratorSystem.Instance.startCoroutine(CloseUIForSeconds(3,"MaskUI"));
    }

    IEnumerator CloseUIForSeconds(float seconds,string name)
    {
        GameObject go = Find(name);
        if (go == null) yield break;
        yield return new WaitForSeconds(seconds);
        uiList.Remove(go);
        GameObject.Destroy(go);
    }


}
