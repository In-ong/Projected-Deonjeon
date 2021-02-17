using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterIdle : FSMSingleton<MonsterIdle>, IFSMState<Monster>
{
    #region Method
    void ChangeMove(Vector3 dir, Monster monster)
    {
        if(dir.sqrMagnitude < monster.DetectedSight * monster.DetectedSight != Util.IsFloatEqual(dir.sqrMagnitude, monster.DetectedSight * monster.DetectedSight))
        {
            monster.ChangeState(MonsterMove.Instance);
        }
    }
    #endregion

    public void Enter(Monster monster)
    {
        if (monster.AnimController.GetAnimState() != MonsterAnimController.eAnimState.IDLE)
            monster.AnimController.Play(MonsterAnimController.eAnimState.IDLE);
    }

    public void Execute(Monster monster)
    {
        monster.SearchDir = monster.Player.transform.position - monster.transform.position;

        if (!monster.Player.Death)
        {
            if (monster.Delay)
            {
                monster.MonsterTime += Time.deltaTime;
                if (monster.DelayTime <= monster.MonsterTime)
                {
                    //레이를 사용해서 플레이어가 앞에 있는지 판단하는 거로 변경 필요
                    if ((monster.Player.transform.position - monster.transform.position).sqrMagnitude < monster.AttackSight * monster.AttackSight)
                    {
                        var dir = ((monster.Player.transform.position + Vector3.up * 1f) - (monster.transform.position + Vector3.up * 1f));

                        if (Physics.Raycast(monster.transform.position + Vector3.up * 1f, dir.normalized, monster.AttackSight, 1 << LayerMask.NameToLayer("Player")))
                        {
                            monster.ChangeState(MonsterAttack.Instance);
                        }
                        else
                        {
                            monster.transform.LookAt(monster.Player.transform);
                            monster.ChangeState(MonsterAttack.Instance);
                        }
                    }
                    else
                        ChangeMove(monster.SearchDir, monster);
                }
            }
            else
                ChangeMove(monster.SearchDir, monster);
        }
    }

    public void Exit(Monster monster)
    {
        if (monster.Delay)
        {
            monster.Delay = false;
            monster.MonsterTime = 0f;
        }
    }
}
