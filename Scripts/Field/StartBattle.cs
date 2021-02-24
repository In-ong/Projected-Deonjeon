using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBattle : MonoBehaviour
{
    #region Unity Method
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Field.Instance.BattleStart();
            Field.Instance.OnPlayer = true;
        }
    }
    #endregion
}
