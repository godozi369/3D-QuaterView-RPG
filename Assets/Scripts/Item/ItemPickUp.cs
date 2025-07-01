using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    private ItemData itemData;
    private int amount;
    private Transform player;

    public float pickupRange = 1.5f;

    [SerializeField] private GameObject rarityEffect_Common;
    [SerializeField] private GameObject rarityEffect_Rare;
    [SerializeField] private GameObject rarityEffect_Epic;
    [SerializeField] private GameObject rarityEffect_Legend;

    public void Init(ItemData data, int value)
    {
        itemData = data;
        amount = value;

        GameObject effectPrefab = GetEffectPrefabByRarity(data.rarity);
        if (effectPrefab != null)
        {
            Instantiate(effectPrefab, transform);
        }
    }
    private GameObject GetEffectPrefabByRarity(string rarity)
    {
        return rarity switch
        {
            "Common" => rarityEffect_Common,
            "Rare" => rarityEffect_Rare,
            "Epic" => rarityEffect_Epic,
            "Legend" => rarityEffect_Legend,
            _ => null
        };
    }
    private void Update()
    {
        if (player == null)
        {
            var playerObj = GameManager.Instance.playerManager?.controller;
            if (playerObj == null) return;
            player = playerObj.transform;
        }

        if (Vector3.Distance(transform.position, player.position) <= pickupRange && Input.GetKeyDown(KeyCode.Space))
        {
            TryPickup();
        }
    }
    private void TryPickup()
    {
        if (itemData == null || amount <= 0 )
        {
            Debug.LogWarning($"[ItemPickUp] 잘못된 아이템 상태: {itemData?.name ?? "null"} / 수량: {amount}");
            return;
        }
        var inventory = GameManager.Instance.playerManager?.inventory;
        if (inventory == null)
        {
            Debug.LogWarning("[ItemPickUp] 인벤토리 접근 실패");
            return;
        }

        inventory.AddItem(itemData, amount);
        Debug.Log($"[Item] 획득: {itemData.name} x{amount}");
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
