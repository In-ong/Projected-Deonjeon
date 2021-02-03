using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour, IFx
{
    #region Field
    bool m_onShooting;

    [SerializeField] FXManager.eType m_type;

    Monster m_monster;
    [SerializeField] MoveAnimCurve m_animCurve;
    #endregion

    #region Method
    public void SetMove(Vector3 targetPos)
    {
        m_onShooting = true;

        #region MoveAnimCurve
        m_animCurve.SetMove(transform.position, targetPos, 1f, () =>
        {
            m_onShooting = false;

            FXManager.Instance.RemoveFX(this);

            gameObject.SetActive(false);
        });
        #endregion
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

            m_animCurve.InitTime();

            FXManager.Instance.RemoveFX(this);
            other.gameObject.GetComponent<Player>().ChangeState(PlayerHit.Instance);

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
    #endregion
}
