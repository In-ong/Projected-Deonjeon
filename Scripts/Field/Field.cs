using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Field : SingleTonMonoBehaviour<Field>
{
    #region Enum
    public enum eStage
    {
        stage0,
        stage1,
        stage2
    }
    #endregion

    #region SerializeField
    [SerializeField] Player m_player;
    [SerializeField] GameObject m_startZone;
    #endregion

    #region Field
    int m_wayNum; //WayPoint의 순서 설정을 위한 int
    int m_spawnNum; //WayPoint 다중 배열의 크기를 설정하기 위한 int
    bool m_onPlayer;

    Door[] m_door;
    Transform m_startPos;
    eStage m_stageNum;
    GameObject[] m_stages;
    GameObject[] m_pathes;
    MoveAnimCurve[] m_pathMoveCurves;
    SpawnPoint[][] m_spawnPoints;
    WayPoint[][] m_wayPoints;
    NavMeshSurface[] m_navMeshSurface;

    List<StartBattle> m_startList = new List<StartBattle>();
    //Dictionary<int, GameObject> m_stageDic = new Dictionary<int, GameObject>();
    #endregion

    #region Property
    public bool OnPlayer { get { return m_onPlayer; } set { m_onPlayer = value; } }
    #endregion

    #region Public Method
    public void BattleStart()
    {
        if (!OnPlayer)
        {
            for (int i = 0; i < m_navMeshSurface.Length; i++)
            {
                if (m_navMeshSurface[i].agentTypeID != 0)
                    m_navMeshSurface[i].enabled = true;
            }

            m_startList[(int)m_stageNum].gameObject.SetActive(false);
            if(m_stageNum == 0)
            {
                var zoneMove = m_startZone.GetComponent<MoveAnimCurve>();
                zoneMove.SetType(MoveAnimCurve.eMoveType.None);
                zoneMove.SetMove(m_startZone.transform.position, new Vector3(m_startZone.transform.position.x, m_startZone.transform.position.y, m_startZone.transform.position.z + 10f), 1f, () =>
                {

                    zoneMove.SetMove(m_startZone.transform.position, new Vector3(m_startZone.transform.position.x, m_startZone.transform.position.y - 30f, m_startZone.transform.position.z), 1f, () =>
                    {
                        for (int i = 0; i < m_navMeshSurface.Length; i++)
                        {
                            //실시간으로 bake된 정보를 지우고 새롭게 build하려면 사용되는 모델의 read/write를 적용시킴으로 바꿔놓아야 한다.
                            m_navMeshSurface[i].RemoveData();
                            m_navMeshSurface[i].BuildNavMesh();
                        }
                    });
                });
            }
            else
            {
                m_pathMoveCurves[(int)m_stageNum - 1].SetMove(m_pathes[(int)m_stageNum - 1].transform.position, new Vector3(m_pathes[(int)m_stageNum - 1].transform.position.x, m_pathes[(int)m_stageNum - 1].transform.position.y - 20f, m_pathes[(int)m_stageNum - 1].transform.position.z), 1f, () =>
                {
                    for (int i = 0; i < m_navMeshSurface.Length; i++)
                    {
                        //실시간으로 bake된 정보를 지우고 새롭게 build하려면 사용되는 모델의 read/write를 적용시킴으로 바꿔놓아야 한다.
                        m_navMeshSurface[i].RemoveData();
                        m_navMeshSurface[i].BuildNavMesh();
                    }
                });
            }
            //m_door[(int)m_stageNum].CloseDoor();
            for (int i = 0; i < m_spawnPoints[(int)m_stageNum].Length; i++)
            {
                MonsterManager.Instance.CreateMonsters((MonsterManager.eMonsterCategory)Random.Range(0, 1), m_spawnPoints[(int)m_stageNum][i].gameObject.transform.position, m_wayPoints[m_wayNum++]);
            }
        }
    }

    public void BattleEnd()
    {
        ItemManager.Instance.CreateTreasureBox(ItemManager.eCategory.Euipment, m_spawnPoints[(int)m_stageNum][0].transform.position);

        m_pathMoveCurves[(int)m_stageNum].SetMove(m_pathes[(int)m_stageNum].transform.position, new Vector3(m_pathes[(int)m_stageNum].transform.position.x, m_pathes[(int)m_stageNum].transform.position.y + 20f, m_pathes[(int)m_stageNum].transform.position.z), 1f, () =>
        {
            OnPlayer = false;
            m_stageNum++;

            for (int i = 0; i < m_navMeshSurface.Length; i++)
            {
                //실시간으로 bake된 정보를 지우고 새롭게 build하려면 사용되는 모델의 read/write를 적용시킴으로 바꿔놓아야 한다.
                m_navMeshSurface[i].RemoveData();
                m_navMeshSurface[i].BuildNavMesh();
            }
        });
        //m_door[(int)m_stageNum].OpenDoor();

        for (int i = 0; i < m_navMeshSurface.Length; i++)
        {
            if (m_navMeshSurface[i].agentTypeID != 0)
                m_navMeshSurface[i].enabled = false;
        }
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
    protected override void OnStart()
    {
        //m_door = GetComponentsInChildren<Door>();
        //m_startPos = Util.FindChildObject(gameObject, "StartPosition").transform;
        m_navMeshSurface = GetComponents<NavMeshSurface>();

        m_stages = GameObject.FindGameObjectsWithTag("Stage");
        m_pathes = GameObject.FindGameObjectsWithTag("Path");
        m_pathMoveCurves = new MoveAnimCurve[m_pathes.Length];
        for(int i = 0; i < m_pathes.Length; i++)
        {
            m_pathMoveCurves[i] = m_pathes[i].GetComponent<MoveAnimCurve>();
            m_pathMoveCurves[i].SetType(MoveAnimCurve.eMoveType.None);
        }
        //foreach (var stage in m_stages)
        //{
        //    m_stageDic.Add(int.Parse(stage.name.Substring(6)), stage);
        //}

        m_spawnPoints = new SpawnPoint[m_stages.Length][];
        for (int i = 0; i < m_stages.Length; i++)
        {
            m_spawnPoints[i] = m_stages[i].GetComponentsInChildren<SpawnPoint>();
            m_startList.Add(m_stages[i].GetComponentInChildren<StartBattle>());
            for (int j = 0; j < m_spawnPoints[i].Length; j++) //WayPoint 다중 배열의 크기를 정할 int를 설정하기 위한 반복문
            {
                m_spawnNum++;
            }
        }
        m_wayPoints = new WayPoint[m_spawnNum][];
        for (int i = 0; i < m_stages.Length; i++)
        {
            for (int j = 0; j < m_spawnPoints[i].Length; j++)
            {
                m_wayPoints[m_wayNum++] = m_spawnPoints[i][j].GetComponentsInChildren<WayPoint>();
            }
        }
        m_wayNum = 0; //몬스터 생성에서 쓰기 위해 다시 0으로 돌려 놓음

        for(int i = 0; i < m_navMeshSurface.Length; i++)
        {
            if (m_navMeshSurface[i].agentTypeID != 0)
                m_navMeshSurface[i].enabled = false;
        }

        //InitField();
    }
    #endregion
}
