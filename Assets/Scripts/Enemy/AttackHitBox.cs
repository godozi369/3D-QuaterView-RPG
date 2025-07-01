using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    private EnemyStateMachine enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<EnemyStateMachine>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("플레이어 타격됨");

            float damage = enemy.GetData().attackDamage; 

            var stat = other.GetComponent<PlayerStat>();
            if (stat != null)
            {
                stat.TakeDamage(damage);
            }
        }
    }
}
