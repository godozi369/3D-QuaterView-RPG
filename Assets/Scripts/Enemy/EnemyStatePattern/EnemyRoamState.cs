using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoamState : IEnemyState
{
    private EnemyStateMachine enemy;
    private EnemyData enemyData;
    private Vector3 roamTarget;
    private float stateTimer;
    private float idleDuration = 1.5f;
    private float roamDuration = 3.0f;
    private bool isIdlePhase;
    private bool hasTakenOff = false;

    public EnemyRoamState(EnemyStateMachine enemy)
    {
        this.enemy = enemy;
        this.enemyData = enemy.GetData();
    }

    public void Enter()
    {
        if (enemy is FlyingDragonAi flying && !flying.IsFlying && !hasTakenOff)
        {
            flying.TakeOff();
            hasTakenOff = true;
        }
        stateTimer = 0f;
        isIdlePhase = true;
        enemy.animator.SetFloat("Speed", 0f);
    }

    public void Tick()
    {
        if (enemy.IsPlayerInDetectRange())
        {
            enemy.ChangeState(new EnemyChaseState(enemy));
            return;
        }

        stateTimer += Time.deltaTime;

        if (isIdlePhase)
        {
            if (stateTimer >= idleDuration)
            {
                roamTarget = enemy.transform.position + new Vector3(Random.Range(-3, 3), 0 , Random.Range(-3, 3));
                enemy.MoveTo(roamTarget);
                enemy.animator.SetFloat("Speed", 0.5f);
                stateTimer = 0f;
                isIdlePhase = false;
            }
        }
        else
        {
            if (stateTimer >= roamDuration || Vector3.Distance(enemy.transform.position, roamTarget) < 0.2f)
            {
                enemy.animator.SetFloat("Speed", 0f);
                stateTimer = 0f;
                isIdlePhase = true;
            }
        }
    }

    public void Exit() {  }
}
