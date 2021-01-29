using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : FSM<Monster>
{
    #region Field
    [SerializeField] protected MonsterManager.eMonsterCategory m_monCategory;

    [SerializeField] protected int m_hp;
    [SerializeField] protected int m_atk;
    [SerializeField] protected float m_attackSight;
    [SerializeField] protected float m_detectedSight;
    [SerializeField] protected float m_delayTime;
    [SerializeField] protected float m_time;

    [SerializeField] protected bool m_delay;
    [SerializeField] protected bool m_isHit;

    protected Vector3 m_searchDir;
    protected Vector3 m_moveDir;
    protected Vector3 m_hitDir;

    [SerializeField] protected AttackArea m_attackArea;
    [SerializeField] Player m_player;
    NavMeshAgent m_navMash;
    MonsterAnimController m_animController;
    protected MoveAnimCurve m_animCurve;
    #endregion

    #region Property
    public MonsterManager.eMonsterCategory MonsterCategory { get { return m_monCategory; } }

    public int Hp { get { return m_hp; } set { m_hp = value; } }
    public int Atk { get { return m_atk; } set { m_atk = value; } }
    public float AttackSight { get { return m_attackSight; } }
    public float DetectedSight { get { return m_detectedSight; } }
    public float DelayTime { get { return m_delayTime; } }
    public float MonsterTime { get { return m_time; } set { m_time = value; } }

    public bool Delay { get { return m_delay; } set { m_delay = value; } }
    public bool IsHit { get { return m_isHit; } set { m_isHit = value; } }

    public Vector3 SearchDir { get { return m_searchDir; } set { m_searchDir = value; } }
    public Vector3 MoveDir { get { return m_moveDir; } set { m_moveDir = value; } }
    public Vector3 HitDir { get { return m_hitDir; } set { m_hitDir = value; } }

    public AttackArea AttackArea { get { return m_attackArea; } }
    public Player Player { get { return m_player; } }
    public NavMeshAgent NavMashAgent { get { return m_navMash; } }
    public MonsterAnimController AnimController { get { return m_animController; } }
    public MoveAnimCurve MoveAnimCurve { get { return m_animCurve; } }
    #endregion

    #region AnimEvent
    public virtual void AnimEvent_Attack()
    {
        m_attackArea.gameObject.SetActive(true);
    }

    public virtual void AnimEvent_AttackFinish()
    {
        m_attackArea.gameObject.SetActive(false);
        ChangeState(MonsterIdle.Instance);
    }
    #endregion

    #region Method
    protected void InitField(MonsterManager.eMonsterCategory category, int hp, int atk, float attackSight, float detectedSight, float delayTime)
    {
        m_monCategory = category;
        m_hp = hp;
        m_atk = atk;
        m_attackSight = attackSight;
        m_detectedSight = detectedSight;
        m_delayTime = delayTime;
    }
    #endregion

    #region Unity Method
    // Start is called before the first frame update
    void Awake()
    {
        OnAwake();
    }

    protected virtual void OnAwake()
    {
        m_navMash = GetComponent<NavMeshAgent>();
        m_animCurve = GetComponent<MoveAnimCurve>();
        m_animController = GetComponent<MonsterAnimController>();
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        FSMUpdate();
    }
    #endregion
}
