using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory List")]
    public List<ItemData> items = new();

    [Header("Equipped Items")]
    public ItemData equippedWeapon;
    public ItemData equippedArmor;

    // 아이템 수량 통합 관리
    private Dictionary<int, int> itemCounts = new();

    public static event Action OnInventoryChanged;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("인벤토리 정리 완료");
            ResetInventory();
        }

    }
    // 무기 id를 기반으로 무기를 장착
    public void EquipWeaponById(int id)
    {
        ItemData newItem = ItemDataBase.Instance.GetItemById(id);
        if (newItem == null || newItem.itemType != ItemType.Weapon) return;

        // 기존 장비가 있으면 인벤토리로 되돌림
        if (equippedWeapon != null)
        {
            AddItem(equippedWeapon, 1); // 수량 1로 인벤토리에 복귀
        }

        equippedWeapon = newItem;
        RemoveItem(newItem);

        Debug.Log($"무기 장착 : {newItem.name}");
    }
    // 방어구 id를 기반으로 방어구 장착
    public void EquipArmorById(int id)
    {
        ItemData newItem = ItemDataBase.Instance.GetItemById(id);
        if (newItem == null || newItem.itemType != ItemType.Armor) return;

        // 기존 장비가 있으면 인벤토리로 되돌림
        if (equippedArmor != null)
        {
            AddItem(equippedArmor, 1); // 수량 1로 인벤토리에 복귀
        }

        equippedArmor = newItem;
        RemoveItem(newItem);

        Debug.Log($"방어구 장착 : {newItem.name}");
    }
    // 아이템 추가
    public void AddItem(ItemData item, int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"인벤토리 수량 0인 아이템 추가 시도 차단 : {item?.name}");
            return;
        }

        if (!itemCounts.ContainsKey(item.id))
        {
            itemCounts[item.id] = 0;
            if (!items.Any(i => i.id == item.id))
                items.Add(item);
        }

        itemCounts[item.id] = Mathf.Clamp(itemCounts[item.id] + amount, 0, 999);
        OnInventoryChanged?.Invoke();
    }
    // 아이템 제거
    public void RemoveItem(ItemData item)
    {
        if (itemCounts.ContainsKey(item.id))
        {
            itemCounts[item.id]--;
            if (itemCounts[item.id] <= 0)
            {
                itemCounts.Remove(item.id);
                items.RemoveAll(i => i.id == item.id);
            }
        }

        OnInventoryChanged?.Invoke();
    }
    // 아이템 사용
    public void UseItem(int itemId)
    {
        if (!itemCounts.ContainsKey(itemId)) return;

        itemCounts[itemId]--;
        if (itemCounts[itemId] <= 0)
        {
            itemCounts.Remove(itemId);
            items.RemoveAll(i => i.id == itemId);
        }

        OnInventoryChanged?.Invoke();
    }
    // 아이템 수량 조회
    public int GetItemCount(int itemId)
    {
        itemCounts.TryGetValue(itemId, out int count);
        return count;
    }

    public bool HasItem(int itemId) => GetItemCount(itemId) > 0;
    public bool IsEquipped(ItemData item) => item != null && (item == equippedWeapon || item == equippedArmor);

    public Dictionary<int, int> GetConsumableCounts() => itemCounts; // 저장용 getter
    public void SetItemCounts(Dictionary<int, int> data)
    {
        itemCounts = new Dictionary<int, int>(data);
        OnInventoryChanged?.Invoke();
    }

    // Test YONG
    public void ResetInventory()
    {
        items.Clear();
        itemCounts.Clear();
        equippedArmor = null;
        equippedWeapon = null;

        OnInventoryChanged?.Invoke();
    }
}
