using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefence : FSMSingleton<PlayerDefence>,IFSMState<Player>
{
    #region Coroutine
    IEnumerator Coroutine_OnDefenece(Player player) //방패를 완전히 들었을 때, 콜라이더가 작동하도록 함
    {
        yield return new WaitForSeconds(0.3f);

        player.DefenceArea().SetActive(true);
    }
    #endregion

    public void Enter(Player player)
    {
        player.IsDefence = true;

        if (player.GetAnimController().GetAnimState() != PlayerAnimController.eAnimState.DEFENCE)
            player.GetAnimController().Play(PlayerAnimController.eAnimState.DEFENCE);
    }

    public void Execute(Player player)
    {
        StartCoroutine(Coroutine_OnDefenece(player));

        if (Input.GetKeyUp(KeyCode.D))
            player.ChangeState(PlayerIdle.Instance);
    }

    public void Exit(Player player)
    {
        StopAllCoroutines(); //상태가 변하면 바로 콜라이더를 켜는 행동을 멈추도록 함
        player.IsDefence = false;
        player.DefenceArea().SetActive(false);
    }
}
