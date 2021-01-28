using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : FSM<Player>
{
    #region SerializeField
    [SerializeField] GameObject m_defenceArea;
    [SerializeField] AttackArea m_attackArea;
    #endregion

    #region Field
    bool m_isCrash;
    bool m_isRun;
    bool m_isAttack;
    bool m_isDefence;
    bool m_isBoxOpen;
    bool m_clickMonster;
    bool m_getItem;

    [SerializeField]int m_level;
    [SerializeField] int m_curHp;
    int m_curAtk;
    [SerializeField] int m_hp = 4;
    [SerializeField] int m_atk = 1;

    float m_swordAttackSight = 2f;
    float m_bowAttackSight = 4f;
    float m_gravity = 10f;
    float m_verticalVecter;
    float m_walkSpeed = 3f;
    float m_runRate = 2f;

    GameObject m_shield;
    GameObject m_sword;
    GameObject m_bow;
    GameObject m_arrow;
    GameObject m_acceptant_sword;
    GameObject m_acceptant_bow;

    RaycastHit m_hit;
    Transform m_targetTransform;
    Treasure_Box m_treaureBox;
    //Monster m_monster;
    NavMeshAgent m_navMesh;
    ItemController m_itemController;
    Rigidbody m_rigid;
    MoveAnimCurve m_animCurve;
    PlayerAnimController m_animController;

    Vector3 m_targetPos;
    Vector3 m_dir;
    #endregion

    #region Property
    public bool IsCrash { get { return m_isCrash; } set { m_isCrash = value; } }
    public bool IsRun { get { return m_isRun; } set { m_isRun = value; } }
    public bool IsAttack { get { return m_isAttack; } set { m_isAttack = value; } }
    public bool IsDefence { get { return m_isDefence; } set { m_isDefence = value; } }
    public bool IsBoxOpen { get { return m_isBoxOpen; } set { m_isBoxOpen = value; } }
    public bool ClickMonster { get { return m_clickMonster; } set { m_clickMonster = value; } }
    public bool GetItem { get { return m_getItem; } set { m_getItem = value; } }

    public  int Level { get { return m_level; } }
    public int CurrentHp { get { return m_curHp; } set { m_curHp = value; } }
    public int CurrentAtk { get { return m_curAtk; } set { m_curAtk = value; } }
    public int Hp { get { return m_hp; } set { m_hp = value; } }
    public int Atk { get { return m_atk; } set { m_atk = value; } }

    public float SwordAttackSight { get { return m_swordAttackSight; } }
    public float BowAttackStght { get { return m_bowAttackSight; } }
    public float Gravity { get { return m_gravity; } }
    public float VerticalVecter { get { return m_verticalVecter; } set { m_verticalVecter = value; } }
    public float WalkSpeed { get { return m_walkSpeed; } }
    public float RunRate { get { return m_runRate; } }

    public GameObject Shield { get { return m_shield; } }
    public GameObject Sword { get { return m_sword; } }
    public GameObject Bow { get { return m_bow; } }
    public GameObject Acceptant_Sword { get { return m_acceptant_sword; } }
    public GameObject Acceptant_Bow { get { return m_acceptant_bow; } }
    public GameObject Arrow { get { return m_arrow; } }

    public Transform TargetTransform { get { return m_targetTransform; } set { m_targetTransform = value; } }
    public Treasure_Box Treasure_Box { get { return m_treaureBox; } set { m_treaureBox = value; } }
    public NavMeshAgent NavMesh { get { return m_navMesh; } }
    public Rigidbody RigidBody { get { return m_rigid; } }
    //public Monster Monster { get { return m_monster; } set { m_monster = value; } }
    public AttackArea AttackArea { get { return m_attackArea; } }
    public ItemController ItemController { get { return m_itemController; } set { m_itemController = value; } }

    public Vector3 TargetPos { get { return m_targetPos; } set { m_targetPos = value; } }
    public Vector3 Direction { get { return m_dir; } set { m_dir = value; } }
    #endregion

    #region AnimEvent
    public void AnimEvent_Attack()
    {
        if(m_sword.activeSelf)
            m_attackArea.gameObject.SetActive(true);
    }

    public void AnimEvent_AttackFinish()
    {
        ChangeState(PlayerIdle.Instance);
    }
    #endregion

    #region Public Method
    public GameObject DefenceArea()
    {
        return m_defenceArea;
    }

    public PlayerAnimController GetAnimController()
    {
        return m_animController;
    }

    public MoveAnimCurve GetAnimCurve()
    {
        return m_animCurve;
    }

    public void ActiveDefence(Vector3 target) //방어 상태에서 공격 받을 때, 뒤로 밀려나는 함수
    {
        var dir = new Vector3(target.x, 0f, target.z);
        var roDir = target - transform.position;
        Quaternion targetDir = Quaternion.LookRotation(roDir.normalized);

        transform.rotation = targetDir;
        m_animCurve.SetMove(transform.position, transform.position + (dir.normalized * -1f * m_walkSpeed), 0.8f, null);

        m_isCrash = false;
    }

    public void SetDamage(int atk, Vector3 target) //데미지 받는 함수
    {
        m_curHp -= atk;
        UI_InGame.Instance.Damage(atk);

        if (target != Vector3.zero)
        {
            var dir = target - transform.position;
            Quaternion targetDir = Quaternion.LookRotation(dir.normalized);

            transform.rotation = targetDir;
        }
    }
    #endregion

    #region Unity Method
    #region CharcterController 충돌
    /*
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Monster") && !m_isCrash) //충돌 판정이 한 번만 이루어지도록 IsCrash가 꺼져있을 때만 작동하도록 설정
        {
            var mon = hit.gameObject.GetComponent<Monster>();
            if(mon.Type == MonsterManager.eMonsterType.Tuttle)
            {
                m_isCrash = true;
                mon.IsHit = false;
                SetDamage(mon.TuttleAtk, mon.transform.position);
                ChangeState(PlayerHit.Instance);
                mon.ChangeState(MonsterIdle.Instance);
            }
            //else if(mon.Type == MonsterManager.eMonsterType.BlackGhost)
            //{
            //    SetDamage(mon.GhostAtk, mon.transform.position);
            //}
        }
    }*/
    #endregion
    private void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("Monster") && !m_isCrash) //충돌 판정이 한 번만 이루어지도록 IsCrash가 꺼져있을 때만 작동하도록 설정
        {
            var mon = other.gameObject.GetComponent<Monster>();
            if (mon.Type == MonsterManager.eMonsterType.Tuttle)
            {
                m_isCrash = true;
                mon.IsHit = false;
                SetDamage(mon.TuttleAtk, mon.transform.position);
                ChangeState(PlayerHit.Instance);
                mon.ChangeState(MonsterIdle.Instance);
            }
        }*/
    }
    // Start is called before the first frame update
    void Awake()
    {
        m_animController = GetComponent<PlayerAnimController>();
        m_navMesh = GetComponent<NavMeshAgent>();
        m_animCurve = GetComponent<MoveAnimCurve>();
        m_rigid = GetComponent<Rigidbody>();
        m_animCurve.SetType(MoveAnimCurve.eMoveType.NavMeshAgent);

        m_attackArea = GetComponentInChildren<AttackArea>();
        m_defenceArea = Util.FindChildObject(this.gameObject, "DefenceArea");
        m_shield = Util.FindChildObject(this.gameObject, "Dummy_Shield");
        m_sword = Util.FindChildObject(this.gameObject, "Dummy_Sword");
        m_bow = Util.FindChildObject(this.gameObject, "Dummy_Bow");
        m_acceptant_sword = Util.FindChildObject(this.gameObject, "Dummy_Acceptant_Sword");
        m_acceptant_bow = Util.FindChildObject(this.gameObject, "Dummy_Acceptant_Bow");
        //m_arrow = Util.FindChildObject(this.gameObject, "Dummy_Arrow");

        m_shield.SetActive(false);
        m_sword.SetActive(false);
        m_bow.SetActive(false);
        //m_arrow.SetActive(false);
        m_acceptant_sword.SetActive(false);
        m_acceptant_bow.SetActive(false);

        var attackType = m_attackArea.GetComponent<AttackArea>();
        attackType.SetType(AttackArea.eAttackType.Player);
        m_defenceArea.SetActive(false);

        m_curHp = m_hp;
        m_curAtk = m_atk;
        m_level = 1;

        InitState(this, PlayerIdle.Instance);
    }

    // Update is called once per frame
    void Update()
    {
        FSMUpdate();

        #region UI 사용 시 커멘드 입력
        /*
        if (!ItemManager.Instance.Inventory.gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                ItemManager.Instance.CreateTreasureBox(ItemManager.eCategory.Consume, transform.position + new Vector3(2f, 0f, 2f));
            }
            if (Input.GetKeyDown(KeyCode.C))
                ItemManager.Instance.ChangeWeapon(this);
            if (Input.GetKeyDown(KeyCode.L))
                ItemManager.Instance.CreateConsumeItem(transform.position + new Vector3(2f, 0f, 2f));
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!ItemManager.Instance.Inventory.gameObject.activeSelf)
                ItemManager.Instance.Inventory.OpenInventory();
            else
                ItemManager.Instance.Inventory.ExitInvetory();
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            if (ItemManager.Instance.OnInventory())
                ItemManager.Instance.Inventory.InGameSlotOn(KeyCode.A, KeyCode.S);
            else
                UI_InGame.Instance.UsingItem(KeyCode.A, this);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (ItemManager.Instance.OnInventory())
                ItemManager.Instance.Inventory.InGameSlotOn(KeyCode.S, KeyCode.A);
            else
                UI_InGame.Instance.UsingItem(KeyCode.S, this);
        }
        */
        #endregion

        if (!ItemManager.Instance.Inventory.gameObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                ItemManager.Instance.CreateTreasureBox(ItemManager.eCategory.Euipment, transform.position + new Vector3(2f, 0f, 2f));
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (!ItemManager.Instance.Inventory.gameObject.activeSelf)
                    ItemManager.Instance.Inventory.OpenInventory();
                else
                    ItemManager.Instance.Inventory.ExitInvetory();
            }
        }
    }
    #endregion
}
