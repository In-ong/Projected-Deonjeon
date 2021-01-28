﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHit : FSMSingleton<MonsterAttack>, IFSMState<Monster>
{
    public void Enter(Monster monster)
    {
        monster.IsHit = true;

        if (monster.AnimController.GetAnimState() != MonsterAnimController.eAnimState.HIT)
            monster.AnimController.Play(MonsterAnimController.eAnimState.HIT);
    }

    public void Execute(Monster monster)
    {
        monster.HitDir = monster.Player.transform.position - monster.transform.position;
        monster.HitDir = new Vector3(monster.HitDir.x, 0f, monster.HitDir.z);

        monster.MoveAnimCurve.SetMove(monster.transform.position, monster.transform.position + (monster.HitDir.normalized * 1f), monster.AnimController.CurrentAnimPlayTime(), () =>
       {
           if (monster.Hp <= 0)
               monster.ChangeState(MonsterDie.Instance);
           else
               monster.ChangeState(MonsterIdle.Instance);
       });
    }

    public void Exit(Monster monster)
    {
        monster.IsHit = false;
        monster.Delay = true;
    }
}
