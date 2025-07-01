using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 15f;
    public float lifeTime = 5f;

    private SkillData skillData;
    private Transform castPoint;
    private Vector3 moveDirection;
    private SkillSystem skillSystem;

    public void Initialize(Vector3 direction, SkillData skillData, Transform castPoint, SkillSystem skillSystem)
    {
        this.skillData = skillData;
        this.castPoint = castPoint;
        this.moveDirection = direction.normalized;
        this.skillSystem = skillSystem; 

        Destroy(gameObject, lifeTime);
    }
    private void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        Vector3 hitPoint = other.ClosestPoint(transform.position);

        if (other.CompareTag("Enemy"))
        {
            GameObject enemyObject = other.GetComponentInParent<EnemyCombatHandler>()?.gameObject;
            if (enemyObject != null)
            {
                skillSystem.ApplySkillEffect(skillData, other.gameObject, hitPoint);
            }
        }

        if (skillData.hitEffect != null)
        {
            GameObject hitVFX = Instantiate(skillData.hitEffect, hitPoint, Quaternion.identity);
            Destroy(hitVFX, 0.5f); 
        }

        Destroy(gameObject);
    }
}
