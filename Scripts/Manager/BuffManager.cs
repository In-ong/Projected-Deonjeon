using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : SingleTonMonoBehaviour<BuffManager>
{
    #region Enum
    public enum eBuffType
    {
        None = -1,
        PowerUp,
        Hilling
    }
    #endregion

    #region Field
    Dictionary<eBuffType, Buff> m_buffDic = new Dictionary<eBuffType, Buff>();
    Player m_player;
    #endregion

    public class Buff
    {
        public float m_lifeTime;
        public eBuffType m_type;
    }

    #region Public Method
    public void ActiveBuff(float duration, eBuffType type)
    {
        if (!m_buffDic.ContainsKey(type))
        {
            m_buffDic.Add(type, new Buff { m_lifeTime = duration, m_type = type });
            switch (type)
            {
                case eBuffType.PowerUp:
                    m_player.Atk += 2;
                    break;
            }
        }
        else
        {
            switch(type)
            {
                case eBuffType.PowerUp:
                    Debug.Log("시간이 초기화 됩니다.");
                    m_buffDic[type].m_lifeTime = duration;
                    break;
            }
        }
    }
    #endregion

    #region Unity Method
    // Start is called before the first frame update
    protected override void OnStart()
    {
        m_player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //foreach문을 활용한 Buff 시간이 끝날 때, 원상복귀와 Dictionary에서 Buff 제거
        foreach(var buff in m_buffDic)
        {
            var curBuff = buff.Value;
            curBuff.m_lifeTime -= Time.deltaTime;
            if(curBuff.m_lifeTime <= 0f)
            {
                switch(curBuff.m_type)
                {
                    case eBuffType.PowerUp:
                        m_player.Atk = 1;
                        break;
                    //case eBuffType.Hill:
                    //    break;
                }
                m_buffDic.Remove(curBuff.m_type);
                break;
            }
        }

        //for문을 활용한 Buff 시간이 끝날 때, 원상복귀와 Dictionary에서 Buff 제거
        //for(int i = 0; i < m_buffDic.Count; i++)
        //{
        //    var data = m_buffDic.GetEnumerator();
        //    data.MoveNext();
        //    var valuePair = data.Current;
        //    var buff = m_buffDic[valuePair.Key];
        //    buff.m_lifeTime -= Time.deltaTime;
        //    if(buff.m_lifeTime <= 0f)
        //    {
        //        switch (buff.m_type)
        //        {
        //            case eBuffType.PowerUp:
        //                m_player.Atk = 1;
        //                break;
        //            case eBuffType.Hill:
        //                break;
        //        }
        //    }
        //    m_buffDic.Remove(buff.m_type);
        //}
    }
    #endregion
}
