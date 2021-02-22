using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPatrol : FSMSingleton<MonsterAttack>, IFSMState<Monster>
{
    public void Enter(Monster monster)
    {
        if (monster.AnimController.GetAnimState() != MonsterAnimController.eAnimState.WALK)
            monster.AnimController.Play(MonsterAnimController.eAnimState.WALK);
    }

    public void Execute(Monster monster)
    {

    }

    public void Exit(Monster monster)
    {

    }
}
