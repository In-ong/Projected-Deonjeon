using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : Monster
{
    #region Field
    [SerializeField] GameObject m_dummy_weapon;
    [SerializeField] GameObject m_fireBall;
    #endregion

    #region AnimEvent
    public void Anim_AttackStart()
    {
        m_dummy_weapon.SetActive(true);
        FXManager.Instance.CreateFX(FXManager.eFxCategory.FireBall, m_dummy_weapon.gameObject, this.gameObject);
    }

    public override void AnimEvent_Attack()
    {
        FXManager.Instance.FxEffect(m_player.gameObject);
    }

    public override void AnimEvent_AttackFinish()
    {
        m_dummy_weapon.SetActive(false);

        base.AnimEvent_AttackFinish();
    }
    #endregion

    #region Unity Method
    protected override void OnAwake()
    {
        base.OnAwake();

        m_dummy_weapon.SetActive(false);

        InitField(MonsterManager.eMonsterCategory.Witch, 2, 2, 5f, 7f, 1f);

        InitState(this, MonsterIdle.Instance);
    }
    #endregion
}
