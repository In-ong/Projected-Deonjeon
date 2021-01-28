using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart_Up : ItemController
{
    #region Public Method
    public override void Effect(Player player)
    {
        player.Hp += 3;
        UI_InGame.Instance.AddStatus();
        player.CurrentHp = player.Hp;
    }
    #endregion
}
