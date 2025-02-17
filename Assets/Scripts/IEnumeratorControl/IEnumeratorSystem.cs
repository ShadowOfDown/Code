//Author : _SourceCode
//CreateTime : 2025-02-14-23:52:17
//Version : 1.0
//UnityVersion : 2021.3.45f1c1

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IEnumeratorSystem : MonoBehaviour
{
    #region µ¥Àý
    private static IEnumeratorSystem instance;
    private static object locker = new object();
    public static IEnumeratorSystem Instance
    {
        get
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        GameObject go = new GameObject("IEnumeratorSystem");
                        instance = go.AddComponent<IEnumeratorSystem>();
                        DontDestroyOnLoad(go);
                    }
                }
            }
            return instance;
        }
    }
    #endregion

    public List<IEnumerator> allCoroutines = new List<IEnumerator>();
    public List<string> allCoroutinesName = new List<string>();
    private Dictionary<string, Coroutine> dictionary = new Dictionary<string, Coroutine>();
    public void startCoroutine(IEnumerator enumerator)
    {
        allCoroutines.Add(enumerator);
        string name = enumerator.GetType().Name;
        string temp = name;
        int index = 0;
        while (allCoroutinesName.Contains(name))
        {
            index++;
            name = temp+"_"+index;  
        }
        allCoroutinesName.Add(name);
        Coroutine c = StartCoroutine(outIEnumerator(enumerator, name)) as Coroutine;
        dictionary.Add(name, c);
    }
    public void startCoroutine(IEnumerator enumerator,string name)
    {
        allCoroutines.Add(enumerator);
        int index = 0;
        while (allCoroutinesName.Contains(name))
        {
            index++;
            name = name + "_" + index;
        }
        allCoroutinesName.Add(name);
        dictionary.Add(name, StartCoroutine(outIEnumerator(enumerator, name)));
    }

    IEnumerator outIEnumerator(IEnumerator enumerator,string name)
    {
        yield return enumerator;
        endCallBack(enumerator, name);
    }
    void endCallBack(IEnumerator enumerator,string name)
    {
        allCoroutines.Remove(enumerator);
        allCoroutinesName.Remove(name);
        dictionary.Remove(name);
    }

    public void stopCoroutine(string name) {
        if (!allCoroutinesName.Contains(name)) { Debug.Log("Ienumerator Not Contain"); return; }
        StopCoroutine(dictionary[name]);
        allCoroutines.RemoveAt(allCoroutinesName.FindIndex(xxx => xxx == name));
        allCoroutinesName.Remove(name);
        dictionary.Remove(name) ;
    }
    public void stopCoroutine(IEnumerator enumerator) {
        if (!allCoroutines.Contains(enumerator)) { Debug.Log("Ienumerator Not Contain");return; }
        string name = allCoroutinesName[allCoroutines.FindIndex(xxx => xxx == enumerator)];
        StopCoroutine(dictionary[name]);
        allCoroutines.Remove(enumerator);
        allCoroutinesName.Remove(name);
        dictionary.Remove(name);
    }
}
