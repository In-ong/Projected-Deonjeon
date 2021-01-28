using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : AnimController
{
    #region Enum
    public enum eAnimState
    {
        NONE = -1,
        IDLE,
        WALK,
        RUN,
        ATTACK,
        DEFENCE,
        HIT,
        DEATH,
        SHOOT,
        SHOOT2,
        MAX
    }
    #endregion

    #region Field
    eAnimState m_state;
    #endregion

    #region Public Method
    public eAnimState GetAnimState() //현재 애니메이션 상태를 전달
    {
        return m_state;
    }
    public void Play(eAnimState state, bool isBlend = true)
    {
        m_state = state;
        base.Play(state.ToString(), isBlend); 
        //이러한 형태로 쓰기 위해서는 애니메이션과 Trigger의 이름이 동일해야 함
    }
    #endregion
}
