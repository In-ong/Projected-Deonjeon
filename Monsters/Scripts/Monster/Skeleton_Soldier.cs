using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Soldier : Monster
{
    #region Unity Method
    protected override void OnAwake()
    {
        base.OnAwake();

        InitField(MonsterManager.eMonsterCategory.Skeleton_Soldier, 3, 2, 2.2f, 5f, 0.5f);
        InitState(this, MonsterIdle.Instance);
    }
    #endregion
}
