using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHit : FSMSingleton<PlayerHit>,IFSMState<Player>
{
    public void Enter(Player player)
    {
        if (player.GetAnimController().GetAnimState() != PlayerAnimController.eAnimState.HIT)
            player.GetAnimController().Play(PlayerAnimController.eAnimState.HIT, false);

        var hitDir = player.transform.position - player.TargetTransform.position;
        hitDir = new Vector3(hitDir.x, 0f, hitDir.z);

        player.GetAnimCurve().SetMove(player.transform.position, player.transform.position + hitDir.normalized, 1f, () =>
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
    }

    public void Exit(Player player)
    {

    }
}
