using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class DragonBossAi : EnemyStateMachine, IPatternEnemy
{
    private float attackTimer;
    private bool isBreathingFire;
    private bool isFlying;
    private bool isPhase2;

    public int enemyID;

    [Header("보스 패턴 설정")]
    public float biteRange = 3f;
    public float fireRange = 10f;
    public float fireCooldown = 5f;


    protected override void Start()
    {
        // 데이터 로드
        var data = EnemyDataLoader.Instance.enemyList.Find(e => e.id == enemyID);

        if (data == null)
        {
            Debug.LogError("DragonBoss 데이터 로드 실패");
            return;
        }

        SetData(data);
        base.Start();

        Debug.Log($"DragonBoss 데이터 로드 완료: {enemyData.name}");

        attackTimer = fireCooldown;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = enemyData.moveSpeed;

    }

    protected override void Update()
    {
        base.Update();
        if (currentState is EnemyDieState || currentState is EnemyHurtState) return;
        
        attackTimer -= Time.deltaTime;

        UpdatePhase();
        AttackPattern();
    }
    private void UpdatePhase()
    {
        if (!isPhase2 && GetCurrentHp() <= enemyData.maxHp * 0.5f)
        {
            isPhase2 = true;
            animator.SetTrigger("BattleStance");
            Debug.Log("[DragonBoss] 페이즈2 진입");
        }
    }

    public void AttackPattern()
    {
        if (attackTimer > 0 || isBreathingFire || isFlying) return;

        int pattern = Random.Range(0, isPhase2 ? 3 : 2); // 0:브레스, 1:물기

        switch (pattern)
        {
            case 0: // 브레스
                if (IsPlayerInAttackRange())
                {
                    StartCoroutine(FireBreath());
                    attackTimer -= fireCooldown;
                }
                break;
            case 1: // 물기
                if (IsPlayerInAttackRange())
                {
                    animator.SetTrigger("Bite");
                    attackTimer = 1.5f;
                }
                break;
            case 3:
                StartCoroutine(FlyingAttack());
                attackTimer = 6f;
                break;
        }
    }

    private IEnumerator FireBreath()
    {
        isBreathingFire = true;
        animator.SetTrigger("Drakaris");
        yield return new WaitForSeconds(2.5f);

        // 이펙트 및 데미지 처리

        isBreathingFire = false;
    }

    private IEnumerator FlyingAttack()
    {
        isFlying = true;
        animator.SetTrigger("TakeOff");
        yield return new WaitForSeconds(1f);

        animator.SetTrigger("FlyingAttack");
        yield return new WaitForSeconds(1.5f);

        animator.SetTrigger("Lands");
        yield return new WaitForSeconds(1f);

        isFlying = false;
    }

    private float GetCurrentHp()
    {
        var field = typeof(EnemyStateMachine).GetField("currentHp", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (float)field.GetValue(this);
    }
}
