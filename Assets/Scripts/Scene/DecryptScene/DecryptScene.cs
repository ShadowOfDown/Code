//Author : _SourceCode
//CreateTime : 2025-03-05-15:18:04
//Version : 1.0
//UnityVersion : 2022.3.53f1c1

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecryptScene : ISceneState
{
    private short flag =0;  
    /// <summary>
    /// 
    /// </summary>
    /// <param name="control"></param>
    /// <param name="flag">0为上海方，非0为南京方</param>
    public DecryptScene(SceneStateControl control,short flag) : base(control)
    {
        this.StateName = "DecryptScene";
        this.flag = flag;
    }

    public override void StateBegin() {
        if (flag == 0) {

            return;
        }

    }
    public override void StateEnd() { 

    }
    public override void StateUpdate() { 

    }
}
