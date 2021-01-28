using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : FSMSingleton<PlayerDeath>,IFSMState<Player>
{
    public void Enter(Player player)
    {
        if (player.GetAnimController().GetAnimState() != PlayerAnimController.eAnimState.DEATH)
            player.GetAnimController().Play(PlayerAnimController.eAnimState.DEATH);
    }

    public void Execute(Player player)
    {
        player.NavMesh.enabled = false;
    }

    public void Exit(Player player)
    {

    }
}
