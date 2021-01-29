using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    #region Eunm
    public enum eAttackType
    {
        Player,
        Monster
    }
    #endregion

    #region Field
    //[SerializeField] Monster m_monster;
    [SerializeField] Player m_player;
    [SerializeField] Monster m_monster;
    eAttackType m_type;
    #endregion

    #region Public Method
    public void SetType(eAttackType type)
    {
        m_type = type;
    }
    #endregion

    #region Unity Method
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster") && m_type == eAttackType.Player)
        {
            var mon = other.gameObject.GetComponent<Monster>();

            if (!mon.IsHit)
                mon.ChangeState(MonsterHit.Instance);
        }

        if (other.CompareTag("Player") && m_type == eAttackType.Monster)
        {
            var player = other.gameObject.GetComponent<Player>();

            if (!player.IsCrash && !player.Death)
            {
                player.IsCrash = true;

                player.TargetTransform = m_monster.transform;

                player.SetDamage(m_monster.Atk, m_monster.transform.position);
                player.ChangeState(PlayerHit.Instance);
            }
            //else if (player.IsDefence && !player.IsCrash)
            //{
            //    player.IsCrash = true;

            //    player.ActiveDefence(m_monster.transform.position);
            //}
        }
    }

    private void Start()
    {
        if (m_type == eAttackType.Monster)
            m_monster = GetComponentInParent<Monster>();
        else  if (m_type == eAttackType.Player)
            m_player = GetComponentInParent<Player>();
        
        
        this.gameObject.SetActive(false);
    }
    #endregion
}
