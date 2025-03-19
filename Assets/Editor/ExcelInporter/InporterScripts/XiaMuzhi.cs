//Author : _SourceCode
//CreateTime : 2025-03-20-03:06:28
//Version : 1.0
//UnityVersion : 2022.3.53f1c1

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class XiaMuzhiBuilder
{
    #region Fields
    public int Id;
    public string Text;
    public int BackoundJPGId;
    public int VerticalPainting;
    public int FirstBox;
    public int SecondBox;
    public int ThirdBox;
    public int TextClass;
    #endregion
}
//AssetPath is the path that the ScriptableObject to export,ExcelName is the name of excel data you want to transform
[ExcelAsset(AssetPath = "Resources/DialogueData",ExcelName = "XiaMuzhi")]
public class XiaMuzhi : ScriptableObject
{
    #region Fields
    //Add a List here ,the List name MUST be the same as the sheet name of excel data��such as : public List<XiaMuzhi> sheet1
    public List<XiaMuzhiBuilder> dialogueStruct = new List<XiaMuzhiBuilder>();
    #endregion
}
