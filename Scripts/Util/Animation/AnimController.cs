using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    public delegate void FinishFunc();

    #region Field
    Animator m_anim;
    FinishFunc m_finishDel;
    string m_prevAnimName = string.Empty;
    #endregion

    #region Public Method
    public void Play(string animName, bool isBlend = true) //Trigger Name, Blend 여부
    {
        if(!string.IsNullOrEmpty(m_prevAnimName)) //이전에 입력된 Trigger가 있을 경우 리셋
        {
            m_anim.ResetTrigger(m_prevAnimName);
            m_prevAnimName = string.Empty;
        }
        if(isBlend) //Blend하면서 Trigger
        {
            m_anim.SetTrigger(animName);
        }
        else //Blend를 쓰지 않고 재생
        {
            m_anim.Play(animName, 0, 0f); //첫 레이어(다른 레이어에서 재생할 시 레이어 번호 입력), 재생은 바로 되도록 0초로
        }
        m_prevAnimName = animName;
    }

    public void Stop() //애니메이션을 일시정지
    {
        m_anim.speed = 0f;
    }
    public void Resume() //애니메이션을 다시 시작
    {
        m_anim.speed = 1f;
    }

    //이 함수는 업데이트에서 계속 확인해주어야 제대로 실행이 가능.
    public void CurrentAnimEnd(string animName, FinishFunc finishDel, float endTime = 0f) //애니메이션이 끝났을 때, 특정 이벤트를 실행하는 함수
    {
        //현재 진행 중인 애니메이션을 확인. animName과 같은 이름이면 다음 코드를 실행토록한다.
        if (m_anim.GetCurrentAnimatorStateInfo(0).IsName(animName))
        {
            //endTime: 이벤트 함수가 나오는 타이밍이 맞지 않을 때 조절하기 위한 장치
            if (m_anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= m_anim.GetCurrentAnimatorStateInfo(0).length - endTime)
                finishDel();
        }
    }

    public float CurrentAnimPlayTime()
    {
        return m_anim.GetCurrentAnimatorStateInfo(0).length;
    }
    #endregion

    #region Unity Method
    // Start is called before the first frame update
    void Start()
    {
        m_anim = GetComponent<Animator>();
    }
    #endregion
}
