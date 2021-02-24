﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDie : FSMSingleton<MonsterAttack>, IFSMState<Monster>
{
    public void Enter(Monster monster)
    {
        if (monster.AnimController.GetAnimState() != MonsterAnimController.eAnimState.DIE)
            monster.AnimController.Play(MonsterAnimController.eAnimState.DIE);

        monster.AnimController.CurrentAnimEnd(() =>
        {
            ItemManager.Instance.CreateConsumeItem(monster.transform.position);
            MonsterManager.Instance.RemoveMonsters(monster);
        });
    }

    public void Execute(Monster monster)
    {
        
    }

    public void Exit(Monster monster)
    {
        
    }
}
