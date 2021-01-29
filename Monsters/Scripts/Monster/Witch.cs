using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : Monster
{
    #region Unity Method
    protected override void OnAwake()
    {
        base.OnAwake();

        InitField(MonsterManager.eMonsterCategory.Witch, 2, 2, 5f, 7f, 1f);

        InitState(this, MonsterIdle.Instance);
    }
    #endregion
}
