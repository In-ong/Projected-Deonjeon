using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : SingleTonMonoBehaviour<FXManager>
{
    #region Field
    [SerializeField] GameObject m_fxPrefab;
    GameObjectPool<IFX> m_fxPool;
    #endregion

    #region Public Method
    public void CreateFX()
    {
        
    }

    public void RemoveFX(IFX fx)
    {

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
    }
    #endregion
}
