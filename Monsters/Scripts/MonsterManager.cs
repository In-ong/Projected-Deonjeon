using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : SingleTonMonoBehaviour<MonsterManager>
{
    #region Enum
    public enum eMonsterCategory
    {
        Skeleton,
        Skeleton_Soldier,
        Witch
    }
    #endregion

    #region Unity Method
    protected override void OnAwake()
    {
        
    }
    #endregion
}
