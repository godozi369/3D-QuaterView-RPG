using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : IEnemyState
{
    private EnemyStateMachine enemy;
    private EnemyData enemyData;
    private float timer = 0f;

    public EnemyAttackState(EnemyStateMachine enemy)
    {
        this.enemy = enemy;
        this.enemyData = enemy.GetData();
    }
    
    public void Enter()
    {
        timer = 0f;
        enemy.agent.isStopped = true;
        if (enemy is IPatternEnemy patternEnemy)
        {
            patternEnemy.AttackPattern();
        }
        else
        {
            enemy.animator.SetTrigger("Attack");
        }
    }

    public void Tick()
    {
        timer += Time.deltaTime;

        if (timer >= enemy.GetData().attackCooldown)
        {
            if (!enemy.IsPlayerInAttackRange())
                enemy.ChangeState(new EnemyChaseState(enemy));
            else
                enemy.ChangeState(new EnemyAttackState(enemy));
        }
    }

    public void Exit()
    {
        enemy.agent.isStopped = false;
    }
}
