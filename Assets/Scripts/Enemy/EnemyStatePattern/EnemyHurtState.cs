using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyHurtState : IEnemyState
{
    private EnemyStateMachine enemy;
    private EnemyData enemyData;
    private float timer = 0f;

    public EnemyHurtState(EnemyStateMachine enemy)
    {
        this.enemy = enemy;
        this.enemyData = enemy.GetData();
    }

    public void Enter()
    {
        timer = 0f;
        enemy.animator.SetTrigger("Get Hit");

        enemy.agent.isStopped = true;
        enemy.agent.updateRotation = false;
        enemy.agent.updatePosition = false;
    }

    public void Tick()
    {
        timer += Time.deltaTime; 
        if (timer >= enemyData.hurtTime)
        {
            enemy.ChangeState(new EnemyRoamState(enemy));
        }
    }

    public void Exit()
    {
        enemy.agent.isStopped = false;
        enemy.agent.updateRotation = true;
        enemy.agent.updatePosition = true;
    }
}
