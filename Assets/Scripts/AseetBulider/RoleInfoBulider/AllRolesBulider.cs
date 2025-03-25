//Author : _SourceCode
//CreateTime : 2025-03-20-05:47:59
//Version : 1.0
//UnityVersion : 2022.3.53f1c1


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllRolesInfo" , menuName = "ProjectData")]
public class AllRolesBulider : ScriptableObject
{
    public List<RoleInfo> roleInfos = new List<RoleInfo>();
}
