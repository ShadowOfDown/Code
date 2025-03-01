//Author : _SourceCode
//CreateTime : 2025-02-24-17:49:03
//Version : 1.0
//UnityVersion : 2022.3.53f1c1


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObject : MonoBehaviour
{
    public virtual void OnLoad() { Debug.Log(gameObject.name + " OnLoad"); }
    public virtual void OnClose() { Debug.Log(gameObject.name + " OnClose"); }

    public virtual void OnUpdate() {  }
    public virtual void OnHide() { Debug.Log(gameObject.name + " OnHide"); }
}
