using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private int price;

    [SerializeField] private PurchasePopupUI purchasePopupUI;
    [SerializeField] private TooltipUI tooltipUI;

    private ItemData itemData;

    public void SetItem(ItemData data)
    {
        itemData = data;

        if (iconImage != null)
            iconImage.sprite = Resources.Load<Sprite>(data.iconPath);
            Debug.LogWarning($"아이템 데이터 로드 실패 : {data.iconPath}");

        if (nameText != null)
            nameText.text = data.name;
            Debug.LogWarning($"아이템 데이터 로드 실패 : {data.iconPath}");

        if (priceText != null)
            priceText.text = price.ToString();
    }

    public void OnClick()
    {
        purchasePopupUI.Show(itemData, price);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            tooltipUI.Show(itemData);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipUI.Hide();
    }
}
