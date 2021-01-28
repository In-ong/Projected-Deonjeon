using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : ItemController
{
    #region Field
    int atk;

    MoveAnimCurve m_animCurve;
    MeshCollider m_collider;
    #endregion

    #region Public Method
    public void SetAttack(Player player)
    {
        gameObject.SetActive(true);

        m_collider.isTrigger = true;

        atk = player.Atk;

        transform.position = player.Arrow.transform.position;
        transform.rotation = player.Arrow.transform.rotation;

        StopCoroutine(Coroutine_Shot(player.TargetTransform));
        StartCoroutine(Coroutine_Shot(player.TargetTransform));
    }
    #endregion

    #region Coroutine
    IEnumerator Coroutine_Shot(Transform target)
    {
        yield return new WaitForSeconds(0.3f);
        m_animCurve.SetMove(transform.position, new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z), 0.25f, () => {
            ItemManager.Instance.RemoveConsumeItem(this);
        });
    }
    #endregion

    #region Unity Method
    private void OnTriggerEnter(Collider other)
    {
        /*
        if(other.CompareTag("Monster"))
        {
            m_collider.isTrigger = false;

            StopCoroutine("Coroutine_Shot");

            other.GetComponent<Monster>().ChangeState(MonsterHit.Instance);

            ItemManager.Instance.RemoveConsumeItem(this);
        }*/
    }

    protected override void OnStart()
    {
        m_collider = GetComponent<MeshCollider>();
        m_animCurve = GetComponent<MoveAnimCurve>();
        m_animCurve.SetType(MoveAnimCurve.eMoveType.None);
    }
    #endregion
}
