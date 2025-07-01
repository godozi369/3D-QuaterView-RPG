using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform slotParent;
    [SerializeField] private TextMeshProUGUI goldText;

    private List<InventorySlotUI> slotUIs = new();
    private PlayerInventory inventory;
    private PlayerStat playerStat;


    private void Awake()
    {
        inventory = GameManager.Instance.playerManager.inventory;
        playerStat = GameManager.Instance.playerManager.stat;

        PlayerInventory.OnInventoryChanged += RefreshUI;
    }
    private void OnEnable()
    {
        slotUIs = slotParent.GetComponentsInChildren<InventorySlotUI>().ToList(); 
        RefreshUI();
    }

    private void OnDestroy()
    {
        PlayerInventory.OnInventoryChanged -= RefreshUI;
    }

    public void RefreshUI()
    {
        var items = inventory.items;
        var consumableCounts = inventory.GetConsumableCounts();

        Debug.Log($"[UI] 인벤토리 아이템 수: {items.Count}");

        int index = 0;
        foreach (var item in items)
        {
            if (index >= slotUIs.Count)
                break;

            int count = inventory.GetItemCount(item.id); // <-- 여기 통일
            slotUIs[index].SetItem(item, count);
            index++;
        }

        // 남은 슬롯 비우기
        for (int i = index; i < slotUIs.Count; i++)
            slotUIs[i].ClearSlot();

        goldText.text = playerStat.gold.ToString();
    }
}
