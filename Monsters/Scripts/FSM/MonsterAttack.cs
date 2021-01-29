using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : FSMSingleton<MonsterAttack>, IFSMState<Monster>
{
    public void Enter(Monster monster)
    {
        if (monster.MonsterCategory == MonsterManager.eMonsterCategory.Skeleton)
        {
            if (monster.AnimController.GetAnimState() != MonsterAnimController.eAnimState.ATTACK)
                monster.AnimController.Play(MonsterAnimController.eAnimState.ATTACK);
        }
        else if(monster.MonsterCategory == MonsterManager.eMonsterCategory.Skeleton_Soldier)
        {
            if (monster.AnimController.GetAnimState() != MonsterAnimController.eAnimState.ATTACK_STING)
                monster.AnimController.Play(MonsterAnimController.eAnimState.ATTACK_STING);
        }
        else if(monster.MonsterCategory == MonsterManager.eMonsterCategory.Witch)
        {
            if (monster.AnimController.GetAnimState() != MonsterAnimController.eAnimState.ATTACK_MAGIC)
                monster.AnimController.Play(MonsterAnimController.eAnimState.ATTACK_MAGIC);
        }
    }

    public void Execute(Monster monster)
    {
        
    }

    public void Exit(Monster monster)
    {
        monster.Delay = true;
    }
}
