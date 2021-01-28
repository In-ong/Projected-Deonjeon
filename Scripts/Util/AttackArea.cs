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
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("On");

        if(collision.collider.CompareTag("Monster"))
        {
            var mon = collision.collider.gameObject.GetComponent<Monster>();

            if (!mon.IsHit)
                mon.ChangeState(MonsterHit.Instance);
        }
        else if(collision.collider.CompareTag("Player"))
        {
            Debug.Log("Hit");

            var player = collision.collider.gameObject.GetComponent<Player>();

            //if (!player.IsDefence && !player.IsCrash)
            //{
            //    player.IsCrash = true;

            //    player.SetDamage(m_monster.GhostAtk, m_monster.transform.position);
            //    player.ChangeState(PlayerHit.Instance);
            //}
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
        if (m_type == eAttackType.Player)
            m_player = GetComponentInParent<Player>();
        
        
        this.gameObject.SetActive(false);
    }
    #endregion
}
