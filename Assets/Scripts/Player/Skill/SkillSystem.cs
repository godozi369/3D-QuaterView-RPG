using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SkillSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform castPoint;
    [SerializeField] private PlayerStat stat;
    [SerializeField] private Animator animator;
    [SerializeField] private AOESkillController aoeController;
    // [SerializeField] private WeaponController weaponController;

    private float cooldownTimer = 0f;
    private bool animationTriggered = false;

    private List<SkillData> currentSkills = new();
    [SerializeField] private SkillData basicAttackSkill;

    private SkillData pendingAOESkill = null; // 시전 대기 스킬

    private void Start()
    {
        SetBasicAttack(basicAttackSkill);
        currentSkills = new List<SkillData> { null, null, null, null };
    }
    public void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
    }
    public void SetBasicAttack(SkillData skill)
    {
        basicAttackSkill = skill;
    }
    public SkillData GetBasicAttack() => basicAttackSkill;
    public List<SkillData> GetCurrentSkills() => currentSkills;
    public void UpdateSkillAt(int index, SkillData skill)
    {
        while (currentSkills.Count <= index)
            currentSkills.Add(null);

        currentSkills[index] = skill;
    }
    public SkillData GetSkillAt(int index)
    {
        if (index < 0 || index >= currentSkills.Count)
            return null;

        return currentSkills[index];
    }
    public void TryCast(SkillData skillData)
    {
        if (cooldownTimer > 0f) return;

        if (skillData.targetingType == TargetingType.AOE)
        {
            pendingAOESkill = skillData;
            aoeController.StartAOETargeting(skillData, this);
            return;
        }
        
        TriggerSkillStart(skillData);
    }
    public void TriggerSkillStart(SkillData skillData)
    {
        cooldownTimer = skillData.cooldown;
        animationTriggered = false;

        SkillSlotUI[] allSlots = FindObjectsOfType<SkillSlotUI>();
        foreach (var slot in allSlots)
        {
            if (slot.GetCurrentSkill() == skillData)
            {
                slot.UseSkill();
                break;
            }
        }

        StartCoroutine(CastSkillOnAnimation(skillData));
    }
    public void TryUseSkill(int index)
    {
        if (index >= 0 && index < currentSkills.Count)
        {
            var skill = currentSkills[index];
            if (skill != null)
            {
                TryCast(skill);
            }
            else
            {
                Debug.Log($"[TryUseSkill] Slot {index} 비어있음");
            }
        }
    }

    private IEnumerator CastSkillOnAnimation(SkillData skill)
    {
        yield return new WaitUntil(() => animationTriggered);

        float finalDamage = CalculateDamage(skill);

        switch (skill.targetingType)
        {
            case TargetingType.Projectile:
                CreateProjectile(skill); 
                break;
            case TargetingType.Cone:
                CreateConeSkill(skill);
                break;
            case TargetingType.Self:
                ApplySkillEffect(skill, gameObject, transform.position);
                break;
        }
    }

    private float CalculateDamage(SkillData skill)
    {
        return stat.Damage + skill.damage;
    }

    private void CreateProjectile(SkillData skill)
    {
        GameObject projectile = Instantiate(skill.skillPrefab, castPoint.position, castPoint.rotation);
        if (projectile.TryGetComponent(out ProjectileBase proj))
        {
            proj.Initialize(castPoint.forward, skill, castPoint, this);
        }
    }
    private void CreateConeSkill(SkillData skill)
    {
        Vector3 castPos = castPoint.position;
        castPos.y = 0f; 

        GameObject coneEffect = Instantiate(skill.skillPrefab, castPos, castPoint.rotation);
        coneEffect.transform.forward = castPoint.forward;

        if (coneEffect.TryGetComponent(out ConeSkillBase cone))
        {
            cone.Initialize(skill, this);
        }

        Destroy(coneEffect, 2f);
    }

    public void ApplySkillEffect(SkillData skill, GameObject target, Vector3 hitPoint)
    {
        // 이펙트 실행
        if (skill.hitEffect != null)
        {
            Debug.Log($"hitEffect 확인: {skill.hitEffect}");
            GameObject effect = Instantiate(skill.hitEffect, hitPoint, Quaternion.identity);
            Destroy(effect, 2f);
        }

        // 데미지 전달
        if (target.TryGetComponent(out IDamageable damageable))
        {
            float totalDamage = CalculateDamage(skill); 
            damageable.TakeDamage(totalDamage);
            Debug.Log($"총 데미지: {totalDamage}");
        }
    }

    public void OnAOESkillConfirmed(Vector3 position)
    {
        if (pendingAOESkill == null) return;

        TriggerSkillStart(pendingAOESkill); 

        GameObject aoe = Instantiate(pendingAOESkill.skillPrefab, position, Quaternion.identity);
        if (aoe.TryGetComponent(out AOESkillBase aoeSkill))
        {
            aoeSkill.Initialize(pendingAOESkill, this);
        }

        pendingAOESkill = null;
    }

    public void CancelAOETargeting()
    {
        pendingAOESkill = null;
    }

    public void TriggerSkillExecution()
    {
        animationTriggered = true;
    }
}
