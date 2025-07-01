using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI countText;

    private ItemData currentItem;
    private int count;

    public void SetItem(ItemData item, int count)
    {
        Sprite icon = Resources.Load<Sprite>(item.iconPath);
        if (icon == null) Debug.LogError($"아이템 아이콘 로드 실패: {item.iconPath}");

        item.icon = icon;

        this.count = count;
        currentItem = item;

        iconImage.sprite = icon;
        iconImage.enabled = true;
        iconImage.gameObject.SetActive(true);

        countText.text = count.ToString();
        countText.enabled = true;
        countText.gameObject.SetActive(count >= 1); // 1 이상일 때만 표시

        var dragger = GetComponent<DraggableUI>();
        if (dragger != null)
            dragger.SetData(item, DraggableUI.UIType.Item);
    }
    public void AddCount(int amount)
    {
        count += amount;
        countText.text = count.ToString();
        countText.gameObject.SetActive(count >1);
    }

    public void ClearSlot()
    {
        currentItem = null;
        iconImage.sprite = null;
        iconImage.enabled = false;
        iconImage.gameObject.SetActive(false);

        countText.text = "";
        countText.enabled = false;
        countText.gameObject.SetActive(false);
    }

    public ItemData GetItem() => currentItem;
}

