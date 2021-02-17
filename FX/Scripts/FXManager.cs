using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : SingleTonMonoBehaviour<FXManager>
{
    #region Enum
    public enum eFxCategory
    {
        FireBall,
        FireExplosion
    }

    public enum eType
    {
        Static,
        NoneStatic
    }
    #endregion
    #region Field
    [SerializeField] GameObject[] m_fxPrefab;

    List<IFx> m_fieldFxList = new List<IFx>();
    public List<IFx> FieldFxList { get { return m_fieldFxList; } }
    Dictionary<eFxCategory, GameObjectPool<IFx>> m_fxPoolDic = new Dictionary<eFxCategory, GameObjectPool<IFx>>();
    #endregion

    #region Public Method
    public void CreateFX(eFxCategory category, GameObject parent, GameObject realParent)
    {
        var fx = m_fxPoolDic[category].Get();
        fx.SetPosition(parent);
        fx.SetParentGameObject(realParent);
        m_fieldFxList.Add(fx);
    }

    public void RemoveFX(IFx fx)
    {
        fx.SetPosition(this.gameObject);

        if (m_fieldFxList.Remove(fx))
            m_fxPoolDic[fx.GetCategory()].Set(fx);
    }

    public void FxEffect(GameObject target)
    {
        for(int i = 0; i < m_fieldFxList.Count; i++)
        {
            if (!m_fieldFxList[i].OnEffect())
            {
                if (m_fieldFxList[i].GetFxType() == eType.NoneStatic)
                    m_fieldFxList[i].Effect(target);
                else
                    m_fieldFxList[i].Effect(target);
            }
        }
    }
    #endregion

    #region Unity Method
    protected override void OnAwake()
    {
        foreach(var fx in m_fxPrefab)
        {
            var num = (eFxCategory)int.Parse(fx.name.Substring(fx.name.Length - 1));
            GameObjectPool<IFx> pool = new GameObjectPool<IFx>(7, () =>
            {
                var obj = Instantiate<GameObject>(fx);
                obj.transform.SetParent(transform);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.identity;
                obj.transform.localScale = Vector3.one;
                var ifx = obj.GetComponent<IFx>();

                return ifx;
            });

            m_fxPoolDic.Add(num, pool);
        }
    }
    #endregion
}
