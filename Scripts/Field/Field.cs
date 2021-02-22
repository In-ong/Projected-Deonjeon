using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    #region Enum
    public enum eStage
    {
        stage0,
        stage1
    }
    #endregion

    #region SerializeField
    [SerializeField] Player m_player;
    #endregion

    #region Field
    int m_wayNum; //WayPoint의 순서 설정을 위한 int
    int m_spawnNum; //WayPoint 다중 배열의 크기를 설정하기 위한 int
    bool m_onPlayer;

    Door[] m_door;
    Transform m_startPos;
    eStage m_stageNum;
    GameObject[] m_stages;
    SpawnPoint[][] m_spawnPoints;
    WayPoint[][] m_wayPoints;

    List<StartBattle> m_startList = new List<StartBattle>();
    Dictionary<int, GameObject> m_stageDic = new Dictionary<int, GameObject>();
    #endregion

    #region Property
    public bool OnPlayer { get { return m_onPlayer; } set { m_onPlayer = value; } }
    #endregion

    #region Public Method
    public void BattleStart()
    {
        if (!OnPlayer)
        {
            m_startList[(int)m_stageNum].gameObject.SetActive(false);
            //m_door[(int)m_stageNum].CloseDoor();
            for (int i = 0; i < m_spawnPoints[(int)m_stageNum].Length; i++)
            {
                MonsterManager.Instance.CreateMonsters(MonsterManager.eMonsterCategory.Skeleton, m_spawnPoints[(int)m_stageNum][i].gameObject.transform.position, m_wayPoints[m_wayNum++]);
            }
        }
    }

    public void BattleEnd()
    {
        //m_door[(int)m_stageNum].OpenDoor();
        OnPlayer = false;
        m_stageNum++;
    }
    #endregion

    #region Method
    void InitField()
    {
        m_stageNum = eStage.stage0;
        //m_player.transform.position = m_startPos.position;
        //for (int i = 0; i < m_stageDic.Count; i++)
        //    m_door[i].OpenDoor();
    }
    #endregion

    #region Unity Method
    // Start is called before the first frame update
    void Start()
    {
        //m_door = GetComponentsInChildren<Door>();
        //m_startPos = Util.FindChildObject(gameObject, "StartPosition").transform;

        m_stages = GameObject.FindGameObjectsWithTag("Stage");
        foreach(var stage in m_stages)
        {
            m_stageDic.Add(int.Parse(stage.name.Substring(6)), stage);
        }

        m_spawnPoints = new SpawnPoint[m_stageDic.Count][];
        for (int i = 0; i < m_stageDic.Count; i++)
        {
            m_spawnPoints[i] = m_stageDic[i].GetComponentsInChildren<SpawnPoint>();
            m_startList.Add(m_stageDic[i].GetComponentInChildren<StartBattle>());
            for(int j = 0; j < m_spawnPoints[i].Length; j++) //WayPoint 다중 배열의 크기를 정할 int를 설정하기 위한 반복문
            {
                m_spawnNum++;
            }
        }
        m_wayPoints = new WayPoint[m_spawnNum][];
        for(int i = 0; i < m_stageDic.Count; i++)
        {
            for (int j = 0; j < m_spawnPoints[i].Length; j++)
            {
                m_wayPoints[m_wayNum++] = m_spawnPoints[i][j].GetComponentsInChildren<WayPoint>();
            }
        }
        m_wayNum = 0; //몬스터 생성에서 쓰기 위해 다시 0으로 돌려 놓음

        //InitField();
    }

    // Update is called once per frame
    void Update()
    {
        //if (OnPlayer && (MonsterManager.Instance.MonsterList.Count == 0))
        //    BattleEnd();
    }
    #endregion
}
