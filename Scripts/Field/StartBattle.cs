using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBattle : MonoBehaviour
{
    #region Field
    [SerializeField] Field m_field;
    #endregion

    #region Unity Method
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_field.BattleStart();
            m_field.OnPlayer = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        m_field = transform.parent.parent.GetComponent<Field>();
    }
    #endregion
}
