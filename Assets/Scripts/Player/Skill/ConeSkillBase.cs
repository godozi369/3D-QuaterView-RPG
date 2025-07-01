using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeSkillBase : MonoBehaviour
{
    [Header("Settings")]
    public float lifetime = 3f; 

    private SkillData skillData;
    private SkillSystem skillSystem;

    // 중복 히트 방지
    private HashSet<GameObject> alreadyHitTargets = new();

    public void Initialize(SkillData skillData, SkillSystem skillSystem)
    {
        this.skillData = skillData;
        this.skillSystem = skillSystem;
        
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        GameObject enemyObject = other.GetComponentInParent<EnemyCombatHandler>()?.gameObject;
        if (enemyObject == null) return;

        if (alreadyHitTargets.Contains(enemyObject)) return;

        alreadyHitTargets.Add(enemyObject);

        Vector3 hitPoint = other.ClosestPoint(transform.position);
        skillSystem.ApplySkillEffect(skillData, enemyObject, hitPoint);
    }
}
