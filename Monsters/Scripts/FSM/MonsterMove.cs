using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : FSMSingleton<MonsterMove>, IFSMState<Monster>
{
    public void Enter(Monster monster)
    {
        if (monster.AnimController.GetAnimState() != MonsterAnimController.eAnimState.WALK)
            monster.AnimController.Play(MonsterAnimController.eAnimState.WALK);
    }

    public void Execute(Monster monster)
    {
        monster.transform.LookAt(monster.Player.transform);
        monster.NavMashAgent.SetDestination(monster.Player.transform.position);

        if (!monster.Player.Death)
        {
            if ((monster.Player.transform.position - monster.transform.position).sqrMagnitude < monster.AttackSight * monster.AttackSight != Util.IsFloatEqual((monster.Player.transform.position - monster.transform.position).sqrMagnitude, monster.AttackSight * monster.AttackSight))
            {
                monster.ChangeState(MonsterAttack.Instance);
            }
        }
        else
            monster.ChangeState(MonsterIdle.Instance);
    }

    public void Exit(Monster monster)
    {
        monster.NavMashAgent.ResetPath();
    }
}
