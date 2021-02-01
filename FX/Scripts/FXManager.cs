using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : SingleTonMonoBehaviour<FXManager>
{
    #region Enum
    public enum eType
    {
        Static,
        NoneStatic
    }
    #endregion

    #region Field
    [SerializeField] GameObject m_fxPrefab;
    GameObjectPool<IFX> m_fxPool;

    List<IFX> m_fxList = new List<IFX>();
    #endregion

    #region Public Method
    public void CreateFX(GameObject parent)
    {
        var fx = m_fxPool.Get();
        fx.SetPosition(parent);
        m_fxList.Add(fx);
    }

    public void RemoveFX(IFX fx)
    {
        fx.SetPosition(this.gameObject);

        if (m_fxList.Remove(fx))
            m_fxPool.Set(fx);
    }

    public void FxEffect(GameObject target)
    {
        for(int i = 0; i < m_fxList.Count; i++)
        {
            if (!m_fxList[i].OnEffect())
                m_fxList[i].Effect(target);
        }
    }
    #endregion

    #region Unity Method
    protected override void OnAwake()
    {
        GameObjectPool<IFX> pool = new GameObjectPool<IFX>(10, () =>
        {
            var obj = Instantiate<GameObject>(m_fxPrefab);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
            var fx = obj.GetComponent<IFX>();

            return fx;
        });

        m_fxPool = pool;
    }
    #endregion
}
