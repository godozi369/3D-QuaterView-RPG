using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FIrePool : MonoBehaviour
{
    public float damage = 33f;
    public float damageInterval = 1f;
    public float duration = 6f;
    public LayerMask targetLayer;

    private Dictionary<Collider, float> lastHitTime = new();

    private void Start()
    {
        Destroy(gameObject, duration);
    }

    private void OnTriggerStay(Collider other)
    {
        // 레이어 마스크 체크
        if (((1 << other.gameObject.layer) & targetLayer) == 0) return;

        // 간격 체크
        if (!lastHitTime.ContainsKey(other)) lastHitTime[other] = -damageInterval;

        if (Time.time - lastHitTime[other] >= damageInterval)
        {
            if (other.TryGetComponent(out IDamageable target))
            {
                target.TakeDamage(damage);
                lastHitTime[other] = Time.time;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (lastHitTime.ContainsKey(other))
            lastHitTime.Remove(other);
    }
}
