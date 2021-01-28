using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power_Potion : ItemController
{
    #region Field
    float m_duration = 4f;
    #endregion

    #region Public Method
    public override void Effect(Player player)
    {
        BuffManager.Instance.ActiveBuff(m_duration, BuffManager.eBuffType.PowerUp);
    }
    #endregion
}
