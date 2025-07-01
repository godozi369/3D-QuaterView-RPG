using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 5f;
    public GameObject firePoolPrefab;
    public LayerMask groundLayer;

    private float damage;
    private Transform target;
    private Vector3 targetPoint;

    public void SetDamage(float value)
    {
        damage = value;
    }
    public void SetTarget(Transform t)
    {
        target = t;

        targetPoint = new Vector3(target.position.x, target.position.y + 0.3f, target.position.z);
    }
    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    private void Update()
    {
        Vector3 dir = (targetPoint - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        float distance = Vector3.Distance(transform.position, targetPoint);
        if (distance < 0.5f)
        {
            if (firePoolPrefab != null)
            {
                Instantiate(firePoolPrefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
