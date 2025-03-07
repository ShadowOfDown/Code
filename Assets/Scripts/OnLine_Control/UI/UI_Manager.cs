//Author : _SourceCode
//CreateTime : 2025-02-17-19:11:53
//Version : 1.0
//UnityVersion : 2021.3.45f1c1

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    private List<UIObject> uiList;
    public Transform CanvasTf { get; private set; }

    public void Init()
    {
        FindCanvas();
        FindUnityEventSystem();

        uiList = new List<UIObject>();
    }
    public void FindUnityEventSystem()
    {
        EventSystem e = GameObject.Find("EventSystem")?.GetComponent<EventSystem>();
        if(e == null)
        {
            GameObject go = new GameObject("EventSystem");
            go.AddComponent<EventSystem>();
            go.AddComponent<StandaloneInputModule>();
        }
    }
    void FindCanvas()
    {
        CanvasTf = GameObject.Find("Canvas")?.transform;
        if (CanvasTf == null)
        {
            CanvasTf = UIObject.Instantiate(Resources.Load<GameObject>("LoginSystem/UI/Prefabs/Canvas")).transform;
            CanvasTf.gameObject.name = "Canvas";
        }
    }

    private UIObject Find(string name)
    {
        CheckList();
        foreach (UIObject go in uiList)
        {
            if (go.name == name) return go;
        }
        return null;
    }

    private void CheckList()
    {
        List<UIObject> ToRemove = new List<UIObject>();
        foreach(UIObject go in uiList)
        {
            if(go == null)
            {
                Debug.LogWarning("Some GameObject In uiList Had Been Destroyed£¡");
                ToRemove.Add(go);
            }
        }
        foreach(UIObject go in ToRemove)
        {
            uiList.Remove(go);
        }
    }

    private T Find<T>(string name) where T : UIObject
    {
        CheckList();
        foreach(UIObject go in uiList)
        {
            if (go.name == name) return go.GetComponent<T>();
        }
        return null;
    }
    public T ShowUI<T>(string Name) where T : UIObject
    {
        T ui = Find<T>(Name);
        if(ui == null)
        {
            if(CanvasTf == null) { FindCanvas(); }
            GameObject go = GameObject.Instantiate(Resources.Load<GameObject>("LoginSystem/UI/Prefabs/" + Name),CanvasTf) ;
            go.name = Name;
            ui = go.AddComponent<T>();
            ui.OnLoad();
            uiList.Add(ui);
        }
        else
        {
            ui.gameObject.SetActive(true);
        }
        return ui;
    }

    public void CloseUI(string Name)
    {
        UIObject go = Find(Name);
        if(go == null) return;
        go.OnClose();
        uiList.Remove(go);
        GameObject.Destroy(go.gameObject);
    }

    public void HideUI(string Name) { 
        UIObject go = Find(Name);
        if(go == null) return;
        go.OnHide();
        go.gameObject.SetActive(false);
    }

    public void CloseAllUI()
    {
        CheckList();
        foreach (UIObject go in uiList)
        {
            go.OnClose();
            GameObject.Destroy(go.gameObject);
        }
        uiList.Clear();
    }

    public void LogWarnning(string message) {
        ShowUI<MaskUI>("WarningUI").ShowMessage(message);
        IEnumeratorSystem.Instance.startCoroutine(CloseUIForSeconds(3,"WarningUI"));
    }

    IEnumerator CloseUIForSeconds(float seconds,string name)
    {
        UIObject go = Find(name);
        if (go == null) yield break;
        yield return new WaitForSeconds(seconds);
        uiList.Remove(go);
        UIObject.Destroy(go.gameObject);
    }

    public void UIobjectUpdate()
    {
        CheckList();
        foreach(UIObject go in uiList)
        {
            if (!go.gameObject.activeInHierarchy)
            {
                continue;
            }
            go.OnUpdate();
        }
    }
}
