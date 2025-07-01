using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour, IDropHandler
{
    [SerializeField] private Image iconImage;
    private ItemData currentItem;

    public void SetItem(ItemData item)
    {
        currentItem = item;

        if (item != null && !string.IsNullOrEmpty(item.iconPath))
        {
            Sprite icon = Resources.Load<Sprite>(item.iconPath);
            iconImage.sprite = icon;
            iconImage.enabled = true;
        }
        else
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        // 드래그 중인 오브젝트에서 SkillSlotUI 또는 ItemSlotUI 가져오기
        var dragged = eventData.pointerDrag;
        if (dragged == null) return;

        var inventorySlotUI = dragged.GetComponent<InventorySlotUI>();
        if (inventorySlotUI == null) return;

        ItemData draggedItem = inventorySlotUI.GetItem();
        if (draggedItem == null) return;

        // 아이템 타입 확인: 무기 또는 방어구만 장착 허용
        if (draggedItem.itemType != ItemType.Weapon && draggedItem.itemType != ItemType.Armor)
        {
            Debug.Log("장비 슬롯에는 무기/방어구만 장착할 수 있습니다.");
            return;
        }

        PlayerInventory playerInventory = GameManager.Instance.playerManager.inventory;

        if (draggedItem.itemType == ItemType.Weapon)
            playerInventory.EquipWeaponById(draggedItem.id);
        else if (draggedItem.itemType == ItemType.Armor)
            playerInventory.EquipArmorById(draggedItem.id);
    }

    public ItemData GetItem() => currentItem;
}
