using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOESkillBase : MonoBehaviour
{
    private SkillData skillData;
    private SkillSystem skillSystem; 
    private HashSet<GameObject> alreadyHitTargets = new(); // 중복 타격 방지
    [SerializeField] private float lifeTime;

    public void Initialize(SkillData skillData, SkillSystem skillSystem)
    {
        this.skillData = skillData;
        this.skillSystem = skillSystem;

        // 파티클 지속 시간 이후 자동 제거
        ParticleSystem ps = GetComponent<ParticleSystem>();
        if (ps != null)
        {
            Destroy(gameObject, ps.main.duration);
        }
        else
        {
            Destroy(gameObject, lifeTime); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject target = other.GetComponentInParent<EnemyCombatHandler>()?.gameObject;
        if (target == null || alreadyHitTargets.Contains(target))
            return;

        alreadyHitTargets.Add(target);

        Vector3 hitPoint = other.ClosestPoint(transform.position);

        skillSystem.ApplySkillEffect(skillData, target, hitPoint);
    }
}
