using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : FSMSingleton<PlayerHit>,IFSMState<Player>
{
    public void Enter(Player player)
    {
        if (player.GetAnimController().GetAnimState() != PlayerAnimController.eAnimState.HIT)
            player.GetAnimController().Play(PlayerAnimController.eAnimState.HIT, false);
    }

    public void Execute(Player player)
    {
        /*
        player.NavMesh.enabled = true;

        if (player.GetCharacter().isGrounded)
        {
            player.VerticalVecter = -player.Gravity * Time.deltaTime;
        }
        else
        {
            player.VerticalVecter -= player.Gravity * Time.deltaTime;
        }
        */

        Vector3 moveVector = new Vector3(0f, player.VerticalVecter, 0f);
        player.NavMesh.Move(moveVector * Time.deltaTime);

        player.GetAnimController().CurrentAnimEnd(() =>
        {
            if (player.CurrentHp > 0)
            {
                player.IsCrash = false;
                player.ChangeState(PlayerIdle.Instance);
            }
            else if (player.CurrentHp <= 0)
            {
                player.IsCrash = false;
                player.ChangeState(PlayerDeath.Instance);
            }
        });
    }

    public void Exit(Player player)
    {

    }
}
