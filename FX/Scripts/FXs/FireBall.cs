using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour, IFX
{
    #region Field
    bool m_onShooting;

    [SerializeField] FXManager.eType m_type;

    [SerializeField] MoveAnimCurve m_animCurve;
    #endregion

    #region Method
    public void SetMove(Vector3 targetPos)
    {
        m_onShooting = true;

        m_animCurve.SetMove(transform.position, new Vector3(targetPos.x, transform.position.y, targetPos.z), 0.5f, () =>
        {
            m_onShooting = false;

            FXManager.Instance.RemoveFX(this);

            gameObject.SetActive(false);
        });
    }
    #endregion

    #region Public Method
    public bool OnEffect()
    {
        return m_onShooting;
    }

    public void SetPosition(GameObject parent)
    {
        if(!gameObject.activeSelf)
            gameObject.SetActive(true);

        if (m_type == FXManager.eType.NoneStatic)
        {
            gameObject.transform.SetParent(parent.transform);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
            gameObject.transform.localScale = Vector3.one;
        }
        else
        {
            gameObject.transform.position = parent.transform.position;
        }
    }

    public void Effect(GameObject target)
    {
        SetMove(target.transform.position);
    }
    #endregion

    #region Unity Method
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            m_onShooting = false;

            FXManager.Instance.RemoveFX(this);

            gameObject.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        m_type = FXManager.eType.NoneStatic;
        m_animCurve = GetComponent<MoveAnimCurve>();
        m_animCurve.SetType(MoveAnimCurve.eMoveType.None);

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion
}
