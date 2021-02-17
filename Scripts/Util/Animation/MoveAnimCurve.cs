using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//특정 상황에서의 애니메이션 움직임을 만들기 위한 클래스
public class MoveAnimCurve : MonoBehaviour
{
    public delegate void OnFinish();

    #region Enum
    public enum eMoveType
    {
        None,
        CharacterController,
        NavMeshAgent
    }
    #endregion

    #region SerializeField
    [SerializeField] AnimationCurve m_animCurve;
    [SerializeField] eMoveType m_type;
    #endregion

    #region Field
    float m_duration; //지속 시간
    [SerializeField] float m_time; //전체 시간

    [SerializeField] CharacterController m_character;
    [SerializeField] NavMeshAgent m_navMesh;
    OnFinish m_finishDel; //끝났을 때 불러올 함수
    Vector3 m_from; //시작 위치
    Vector3 m_to; //도착 위치
    #endregion

    #region Public Method
    public void SetMove(Vector3 from, Vector3 to, float duration, OnFinish finishDel)
    {
        m_from = from;
        m_to = to;
        m_duration = duration;
        m_finishDel = finishDel;
        if(m_type == eMoveType.NavMeshAgent)
        {
            m_navMesh.isStopped = true;
            m_navMesh.ResetPath();
        }
        StopAllCoroutines();
        if(this.gameObject.activeInHierarchy)
            StartCoroutine("Coroutine_MoveProcess");
    }

    public void SetType(eMoveType type)
    {
        m_type = type;
    }

    public void InitTime()
    {
        m_time = 0f;
    }

    public eMoveType GetMoveType()
    {
        return m_type;
    }
    #endregion

    #region Coroutine
    IEnumerator Coroutine_MoveProcess()
    {
        while(true)
        {
            m_time += Time.deltaTime / m_duration;
            var dest = Vector3.Lerp(m_from, m_to, m_animCurve.Evaluate(m_time));
            if (m_type == eMoveType.CharacterController)
                m_character.Move(dest - transform.position);
            else if(m_type == eMoveType.None)
                transform.position = dest;
            else if (m_type == eMoveType.NavMeshAgent)
            {
                //m_navMesh.SetDestination(dest);
                transform.position = dest;
            }

            if (m_time >= 1f)
            {
                m_time = 0f;
                if(m_finishDel != null)
                {
                    m_finishDel();
                }
                yield break;
            }
            yield return null;
        }
    }
    #endregion

    #region Unity Method
    // Start is called before the first frame update
    void Start()
    {
        if (m_type == eMoveType.CharacterController)
            m_character = GetComponent<CharacterController>();
        else if (m_type == eMoveType.NavMeshAgent)
            m_navMesh = GetComponent<NavMeshAgent>();
    }
    #endregion
}
