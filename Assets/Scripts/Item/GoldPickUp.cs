using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPickUp : MonoBehaviour
{
    public int amount;
    private Transform player;
    public float pickupRange = 999f;

    [SerializeField] private GameObject vfxEffect;

    private void Start()
    {
        Init(amount);
    }

    public void Init(int value)
    {
        amount = value;

        if (vfxEffect != null)
            Instantiate(vfxEffect, transform.position, Quaternion.identity, transform);
    }
    private void Update()
    {
        if (player == null)
        {
            if (GameManager.Instance.playerManager != null)
                player = GameManager.Instance.playerManager.controller.transform;

            if (player == null) return; // 여전히 null이면 종료
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= pickupRange && Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.playerManager.stat.AddGold(amount);
            Debug.Log($"[Gold] 획득: {amount}");
            Destroy(gameObject);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
