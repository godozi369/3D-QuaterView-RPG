using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyCombatHandler : MonoBehaviour, IDamageable
{
    private EnemyStateMachine stateMachine;
    private DamageTextSpawner spawner;
    private float currentHp;

    public static event Action<EnemyCombatHandler> OnAnyEnemyDamaged;

    private void Awake()
    {
        stateMachine = GetComponent<EnemyStateMachine>();
        spawner = GetComponent<DamageTextSpawner>();
    }

    public void Init(float maxHp)
    {
        currentHp = maxHp;
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        spawner?.ShowDamage(damage);
        Debug.Log($"[ShowDamage] {damage} 데미지 표시 시도함");

        OnAnyEnemyDamaged?.Invoke(this);

        if (currentHp <= 0)
        {
            currentHp = 0;
            stateMachine.ChangeState(new EnemyDieState(stateMachine));
        }
        else
        {
            stateMachine.ChangeState(new EnemyHurtState(stateMachine));
        }
    }

    public float GetCurrentHp() => currentHp;
    public EnemyData GetData() => stateMachine.GetData();
}
