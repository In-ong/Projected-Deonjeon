using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimController : AnimController
{
    #region Enum
    public enum eAnimState
    {
        IDLE,
        WALK,
        LEFTMOVE,
        ATTACK,
        ATTACK_STING,
        HIT,
        DIE
    }
    #endregion

    #region Field
    eAnimState m_state;
    #endregion

    #region Public Method
    public eAnimState GetAnimState()
    {
        return m_state;
    }

    public void Play(eAnimState state, bool isBlend = true)
    {
        m_state = state;
        base.Play(m_state.ToString(), isBlend);
    }
    #endregion
}
