using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBreath : MonoBehaviour
{
    private EnemyData enemyData;
    private float damageInterval = 1f;
    private Dictionary<Collider, float> lastHitTime = new();

    public void SetData(EnemyData data)
    {
        enemyData = data;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent(out IDamageable target)) return;

        if (!lastHitTime.ContainsKey(other))
            lastHitTime[other] = -damageInterval;

        if (Time.time - lastHitTime[other] >= damageInterval)
        {
            target.TakeDamage(enemyData.attackDamage);
            lastHitTime[other] = Time.time;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (lastHitTime.ContainsKey(other))
            lastHitTime.Remove(other);
    }
}
