using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : Monster
{
    #region Unity Method
    protected override void OnAwake()
    {
        base.OnAwake();

        InitField(MonsterManager.eMonsterCategory.Witch, 2, 2, 3.6f, 5f, 1f);

        ChangeState(MonsterIdle.Instance);
    }
    #endregion
}
