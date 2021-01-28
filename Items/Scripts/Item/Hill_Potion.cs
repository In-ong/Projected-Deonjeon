using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hill_Potion : ItemController
{
    #region Field
    int hilling = 2;
    #endregion

    #region Pulic Method
    public override void Effect(Player player)
    {
        if (player.CurrentHp < player.Hp)
        {
            player.CurrentHp += hilling;
            UI_InGame.Instance.Hilling(hilling);
            if (player.CurrentHp > player.Hp)
                player.CurrentHp = player.Hp;
        }
        else
        {
            Debug.Log("Hp가 가득 차 있어서 더 이상 회복할 수 없습니다.");
            return;
        }
    }
    #endregion
}
