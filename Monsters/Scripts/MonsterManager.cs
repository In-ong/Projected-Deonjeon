using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : SingleTonMonoBehaviour<MonsterManager>
{
    #region Enum
    public enum eMonsterCategory
    {
        Skeleton,
        Skeleton_Soldier,
        Witch
    }
    #endregion

    #region Field
    [SerializeField] GameObject[] m_monsterPrefab;

    List<Monster> m_fieldMonsterList = new List<Monster>();
    Dictionary<eMonsterCategory, GameObjectPool<Monster>> m_monsterPoolDic = new Dictionary<eMonsterCategory, GameObjectPool<Monster>>();
    #endregion

    #region Public Method
    public void CreateMonsters(eMonsterCategory category, Vector3 spawn, WayPoint[] ways)
    {
        var mon = m_monsterPoolDic[category].Get();
        mon.SetMonster(spawn, ways);
        m_fieldMonsterList.Add(mon);
    }

    public void RemoveMonsters(Monster monster)
    {
        if (m_fieldMonsterList.Remove(monster))
            m_monsterPoolDic[monster.MonsterCategory].Set(monster);
    }

    public List<Monster> GetFieldMonsterList()
    {
        return m_fieldMonsterList;
    }
    #endregion

    #region Unity Method
    protected override void OnAwake()
    {
        foreach(var mons in m_monsterPrefab)
        {
            eMonsterCategory category = (eMonsterCategory)int.Parse(mons.name.Substring(mons.name.Length - 1));
            GameObjectPool<Monster> pool = new GameObjectPool<Monster>(7, () =>
            {
                var obj = Instantiate<GameObject>(mons);
                obj.transform.SetParent(this.gameObject.transform);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.identity;
                obj.transform.localScale = Vector3.one;
                var mon = obj.GetComponent<Monster>();

                return mon;
            });

            m_monsterPoolDic.Add(category, pool);
        }
    }
    #endregion
}
