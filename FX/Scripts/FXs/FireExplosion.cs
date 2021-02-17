using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExplosion : MonoBehaviour, IFx
{
    #region Field
    [SerializeField] bool m_isStopped;

    ParticleSystem m_particle;

    [SerializeField] FXManager.eFxCategory m_category;
    [SerializeField] FXManager.eType m_type;
    #endregion

    #region Method
    IEnumerator Coroutine_StopParticle()
    {
        yield return new WaitForSeconds(2.5f);

        FXManager.Instance.RemoveFX(this);

        m_isStopped = true;
        gameObject.SetActive(false);
    }
    #endregion

    #region Public Method
    public FXManager.eType GetFxType()
    {
        return m_type;
    }

    public FXManager.eFxCategory GetCategory()
    {
        return m_category;
    }

    public bool OnEffect()
    {
        return m_isStopped;
    }

    public void SetParentGameObject(GameObject parent)
    {

    }

    public void SetPosition(GameObject target)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        m_isStopped = false;

        gameObject.transform.position = target.transform.position;
    }

    public void Effect(GameObject target)
    {
        //StopCoroutine("Coroutine_StopParticle");
        StartCoroutine("Coroutine_StopParticle");
    }
    #endregion

    #region Unity Method
    // Start is called before the first frame update
    void Start()
    {
        m_particle = GetComponent<ParticleSystem>();

        m_category = FXManager.eFxCategory.FireExplosion;
        m_type = FXManager.eType.Static;

        gameObject.SetActive(false);
    }
    #endregion
}
