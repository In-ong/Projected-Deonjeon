using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExplosion : MonoBehaviour, IFx
{
    #region Field
    [SerializeField] FXManager.eFxCategory m_category;
    [SerializeField] FXManager.eType m_type;
    #endregion

    #region Public Method
    public FXManager.eFxCategory GetCategory()
    {
        return m_category;
    }

    public bool OnEffect()
    {
        return false;
    }

    public void SetParentGameObject(GameObject parent)
    {

    }

    public void SetPosition(GameObject target)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        gameObject.transform.position = target.transform.position;
    }

    public void Effect(GameObject target)
    {
        
    }
    #endregion

    #region Unity Method
    // Start is called before the first frame update
    void Start()
    {
        m_category = FXManager.eFxCategory.FireExplosion;
        m_type = FXManager.eType.Static;

        gameObject.SetActive(false);
    }
    #endregion
}
