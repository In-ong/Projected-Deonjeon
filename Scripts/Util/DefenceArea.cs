using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceArea : MonoBehaviour
{
    #region Field
    //Monster m_monster;
    Player m_player;
    #endregion

    #region Unity Method
    private void OnTriggerEnter(Collider other)
    {
        /*
        if(other.CompareTag("Monster"))
        {
            m_monster = other.gameObject.GetComponent<Monster>();

            m_player.ActiveDefence(other.gameObject.transform.position);

            m_monster.IsHit = true;
            m_monster.ChangeState(MonsterIdle.Instance);
        }*/
    }
    // Start is called before the first frame update
    void Awake()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    #endregion
}
