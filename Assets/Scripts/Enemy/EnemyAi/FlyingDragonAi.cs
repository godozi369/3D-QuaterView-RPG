using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


public enum RangeAttackType
{
    Fireball,
    FlameSpread
}

public class FlyingDragonAi : EnemyStateMachine, IPatternEnemy
{
    [Header("Enemy Settings")]
    public int enemyID;

    [Header("Attack Type")]
    [SerializeField] private RangeAttackType attackType;

    [Header("Attack Colliders")]
    [SerializeField] private GameObject basicCol;
    [SerializeField] private GameObject basic2Col;

    [Header("Fire Attack VFX")]
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private Transform flameStartPoint;
    [SerializeField] private Transform flameEndPoint;

    private GameObject currentFlame;

    public bool IsFlying = false;
    public bool hasLanded = false;

    protected override void Start()
    {
        // 데이터 로드
        var data = EnemyDataLoader.Instance.enemyList.Find(e => e.id == enemyID);
        if (data == null)
        {
            Debug.LogError("FlyingDragons 데이터 로드 실패");
            return;
        }

        SetData(data);
        base.Start();
        Debug.Log($"FlyingDragons 데이터 로드 완료: {enemyData.name}");

        agent.speed = enemyData.moveSpeed;

        DisableAllColliders();
    }
    protected override void Update()
    {
        base.Update();
    }
    public void AttackPattern()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        float meleeRange = enemyData.attackRange / 2f;

        if (!IsFlying)
        {
            if (distance <= meleeRange)
            {
                MeleeAttack();
            }
            else if (distance <= enemyData.attackRange)
            {
                switch (attackType)
                {
                    case RangeAttackType.Fireball:
                        animator.SetTrigger("Fire Attack");
                        break;
                    case RangeAttackType.FlameSpread:
                        animator.SetTrigger("Flame Attack");
                        break;
                }
            }
        }
    }
    private void MeleeAttack()
    {
        int rand = Random.Range(0, 2);
        switch (rand)
        {
            case 0: animator.SetTrigger("Basic Attack"); break;
            case 1: animator.SetTrigger("Basic2 Attack"); break;
        }
    }
    public void TakeOff()
    {
        IsFlying = true;
        hasLanded = false;
        animator.SetBool("IsFlying", true);
        animator.SetTrigger("Take Off");
    }

    public void Land()
    {
        IsFlying = false;
        hasLanded = true;
        animator.SetBool("IsFlying", false);
        animator.SetTrigger("Land");
        agent.isStopped = false;

        ChangeState(new EnemyAttackState(this));
    }

    public void TriggerFireball()
    {
        if (firePrefab != null && flameStartPoint != null)
        {
            GameObject fire = Instantiate(firePrefab, flameStartPoint.position, flameStartPoint.rotation);

            if (fire.TryGetComponent(out FireProjectile proj))
            {
                proj.SetDamage(enemyData.attackDamage);      
                proj.SetTarget(player);                     
            }
        }
    }

    public void StartFlame()
    {
        if (firePrefab != null)
        {
            Vector3 spawnPos = flameStartPoint.position;
            currentFlame = Instantiate(firePrefab, spawnPos, Quaternion.identity);
            currentFlame.SetActive(true);

            // 정확한 방향 계산
            Vector3 direction = (flameEndPoint.position - flameStartPoint.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);
            currentFlame.transform.rotation = rotation;

            currentFlame.transform.SetParent(flameStartPoint, true);
        }
    }

    public void StopFlame()
    {
        if (firePrefab != null)
            Destroy(currentFlame);
    }

    public void EnableBasicCol() => basicCol.SetActive(true);
    public void DisableBasicCol() => basicCol.SetActive(false);

    public void EnableBasic2Col() => basic2Col.SetActive(true);
    public void DisableBasic2Col() => basic2Col.SetActive(false);

    private void DisableAllColliders()
    {
        basicCol.SetActive(false);
        basic2Col.SetActive(false);
    }
}
