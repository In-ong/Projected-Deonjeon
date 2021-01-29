using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Monster
{
    #region Unity Method
    protected override void OnAwake()
    {
        base.OnAwake();

        m_attackArea = GetComponentInChildren<AttackArea>();
        m_attackArea.SetType(AttackArea.eAttackType.Monster);

        m_animCurve.SetType(MoveAnimCurve.eMoveType.NavMeshAgent);

        InitField(MonsterManager.eMonsterCategory.Skeleton, 3, 1, 1.9f, 5f, 0.5f);
        InitState(this, MonsterIdle.Instance);
    }
    #endregion
}
