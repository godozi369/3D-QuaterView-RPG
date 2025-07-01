using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : IEnemyState
{
    private EnemyStateMachine enemy;
    private EnemyData enemyData;

    public EnemyChaseState(EnemyStateMachine enemy)
    {
        this.enemy = enemy;
        this.enemyData = enemy.GetData();
    }

    public void Enter()
    {
        enemy.animator.SetFloat("Speed", 1f);
    }

    public void Tick()
    {
        if (!enemy.IsPlayerInDetectRange())
        {
            enemy.ChangeState(new EnemyRoamState(enemy));
            return;
        }

        if (enemy.IsPlayerInAttackRange())
        {
            if (enemy is FlyingDragonAi flyingDragon && flyingDragon.IsFlying)
            {
                flyingDragon.Land();
                return;
            }

            enemy.ChangeState(new EnemyAttackState(enemy));
            return;
        }
        enemy.MoveTo(enemy.player.position);
    }

    public void Exit() { }
}
