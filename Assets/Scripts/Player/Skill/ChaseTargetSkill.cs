using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTargetSkill : MonoBehaviour
{
    public float speed = 10f;
    public float detectionRadius = 20f;
    public LayerMask targetLayer;

    private Transform target;

    private void Start()
    {
        FindTarget();
    }

    private void Update()
    {
        if (target == null) return;

        Vector3 dir = target.position - transform.position;
        dir.y = 0f;
        dir.Normalize();

        transform.position += dir * speed * Time.deltaTime;
    }

    private void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, targetLayer);
        float minDist = float.MaxValue;

        foreach (var hit in hits)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = hit.transform.position;

            myPos.y = 0;
            targetPos.y = 0;

            float dist = Vector3.Distance(myPos, targetPos);
            if (dist < minDist)
            {
                minDist = dist;
                target = hit.transform;
            }
        }
    }
}
