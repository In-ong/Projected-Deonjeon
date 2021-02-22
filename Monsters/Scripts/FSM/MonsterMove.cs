using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : FSMSingleton<MonsterMove>, IFSMState<Monster>
{
    #region Field
    bool IsTargetDetectedSight(Monster monster)
    {
        if ((monster.Player.transform.position - monster.transform.position).sqrMagnitude < monster.DetectedSight * monster.DetectedSight != Util.IsFloatEqual((monster.Player.transform.position - monster.transform.position).sqrMagnitude, monster.DetectedSight * monster.DetectedSight))
        {
            if (!monster.Player.Death)
            {
                monster.IsPatrol = false;
                monster.WayNum = 0;

                return true;
            }
            else
                return false;
        }
        else
            return false;
    }

    void Patrol(Monster monster)
    {
        if(!IsTargetDetectedSight(monster))
        {
            monster.IsPatrol = true;
            monster.NavMashAgent.SetDestination(monster.WayPoints[monster.WayNum].transform.position);
            //객체가 움직이고 있고 목적지에 도착했는 가를 확인
            if(monster.NavMashAgent.velocity.sqrMagnitude >= 0.2f * 0.2f && monster.NavMashAgent.remainingDistance <= 0.3f)
            {
                monster.NavMashAgent.isStopped = true;
            }
            
            if(monster.NavMashAgent.isStopped)
            {
                monster.Delay = true;
                monster.ChangeState(MonsterIdle.Instance);
            }
        }
        else
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
    }
    #endregion

    public void Enter(Monster monster)
    {
        if (monster.AnimController.GetAnimState() != MonsterAnimController.eAnimState.WALK)
            monster.AnimController.Play(MonsterAnimController.eAnimState.WALK);
    }

    public void Execute(Monster monster)
    {
        Patrol(monster);
    }

    public void Exit(Monster monster)
    {
        monster.NavMashAgent.ResetPath();

        if (monster.IsPatrol)
        {
            monster.WayNum++;
            monster.IsPatrol = false;

            if (monster.WayNum > monster.WayPoints.Length - 1)
                monster.WayNum = 0;
        }
    }
}
