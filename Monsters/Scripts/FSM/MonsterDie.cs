﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDie : FSMSingleton<MonsterDie>, IFSMState<Monster>
{
    public void Enter(Monster monster)
    {
        if (monster.AnimController.GetAnimState() != MonsterAnimController.eAnimState.DIE)
            monster.AnimController.Play(MonsterAnimController.eAnimState.DIE);
    }

    public void Execute(Monster monster)
    {
        monster.AnimController.CurrentAnimEnd(() =>
        {
            ItemManager.Instance.CreateConsumeItem(monster.transform.position);
            if (Field.Instance.OnPlayer)
            {
                MonsterManager.Instance.RemoveMonsters(monster);

                monster.ChangeState(MonsterIdle.Instance);
            }
        });
    }

    public void Exit(Monster monster)
    {
        
    }
}
