using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : FSMSingleton<PlayerAttack>,IFSMState<Player>
{
    #region Coroutine
    //IEnumerator Coroutine_AttackEnd(Player player)
    //{
    //    player.ChangeState(PlayerIdle.Instance);

    //    yield return new WaitForSeconds(0.3f);
    //}
    #endregion

    public void Enter(Player player)
    {
        if (player.Sword.activeSelf)
        {
            if (player.GetAnimController().GetAnimState() != PlayerAnimController.eAnimState.ATTACK)
                player.GetAnimController().Play(PlayerAnimController.eAnimState.ATTACK, false);
        }
        else if(player.Bow.activeSelf)
        {
            if (player.GetAnimController().GetAnimState() != PlayerAnimController.eAnimState.SHOOT2)
                player.GetAnimController().Play(PlayerAnimController.eAnimState.SHOOT2);
        }
    }

    public void Execute(Player player)
    {

    }

    public void Exit(Player player)
    {
        player.AttackArea.gameObject.SetActive(false);
    }
}
