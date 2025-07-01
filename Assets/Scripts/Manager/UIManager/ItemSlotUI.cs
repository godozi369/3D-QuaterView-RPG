using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour, IDropHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Image iconImage;
    public Image cooldownImage;
    public Text countText;

    private ItemData currentItem;
    private float cooldownTimer;

    private void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            float maxCooldown = currentItem?.effect.cooldown ?? 1f;
            cooldownImage.fillAmount = cooldownTimer / maxCooldown;
        }
    }

    public void SetItem(ItemData item, int count)
    {
        if (!IsCompatible(item)) return;

        currentItem = item;

        Sprite icon = Resources.Load<Sprite>(item.iconPath);
        if (icon != null)
        {
            iconImage.sprite = icon;
            iconImage.enabled = true;
        }
        else
        {
            Debug.LogWarning($"아이콘 로드 실패 : {item.iconPath}");
            iconImage.enabled = false;
        }

        countText.text = count.ToString();
        countText.enabled = true;

        cooldownImage.fillAmount = 0f;
    }

    public void UseItem()
    {
        if (currentItem == null || cooldownTimer > 0f) return;

        // 효과 적용 (예: 체력 회복)
        Debug.Log($"[아이템 사용]: {currentItem.name} 효과 적용됨");

        float cd = currentItem.effect.cooldown;
        cooldownTimer = cd;

        // 인벤토리에서 수량 감소
        GameManager.Instance.playerManager.inventory.RemoveItem(currentItem);

        // 수량 업데이트
        int newCount = GameManager.Instance.playerManager.inventory.GetItemCount(currentItem.id);
        countText.text = newCount > 0 ? newCount.ToString() : "";

        if (newCount <= 0) ClearSlot();
    }
    public bool IsUsable()
    {
        return currentItem != null && cooldownTimer <= 0f;
    }
    public void ClearSlot()
    {
        currentItem = null;
        iconImage.sprite = null;
        iconImage.enabled = false;

        countText.text = "";
        countText.enabled = false;

        cooldownImage.fillAmount = 0f;
    }

    private bool IsCompatible(ItemData item)
    {
        return item.itemType == ItemType.Consumable;
    }

    public void OnDrop(PointerEventData eventData)
    {
        var dragged = eventData.pointerDrag;
        if (dragged == null) return;

        var dragUI = dragged.GetComponent<DraggableUI>();
        if (dragUI == null || dragUI.GetItemType() != DraggableUI.UIType.Item) return;

        ItemData item = dragUI.GetData() as ItemData;
        if (item == null || !IsCompatible(item)) return;

        int count = GameManager.Instance.playerManager.inventory.GetItemCount(item.id);
        if (count <= 0) return;

        SetItem(item, count);
    }

    public void OnBeginDrag(PointerEventData eventData) { }
    public void OnDrag(PointerEventData eventData) { }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), Input.mousePosition))
        {
            ClearSlot();
        }
    }

    public ItemData GetCurrentItem() => currentItem;
}
