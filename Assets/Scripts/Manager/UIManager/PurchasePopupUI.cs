using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PurchasePopupUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI announceText;
    [SerializeField] private TMP_InputField quantityInput;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button exitButton;

    private ItemData currentItemData;
    private int pricePerUnit;

    private void Start()
    {
        popupPanel.SetActive(false);
        buyButton.onClick.AddListener(OnClickBuy);
        exitButton.onClick.AddListener(OnClickExit);
    }

    public void Show(ItemData itemData, int price)
    {
        currentItemData = itemData;
        pricePerUnit = price;

        itemNameText.text = itemData.name;
        priceText.text = price.ToString();
        quantityInput.text = "1";
        popupPanel.SetActive(true);
    }

    public void Hide()
    {
        popupPanel.SetActive(false);
    }

    public void OnClickBuy()
    {
        if (!int.TryParse(quantityInput.text, out int amount))
        {
            Debug.LogWarning("수량 입력이 올바르지 않습니다.");
            return;
        }

        int totalCost = amount * pricePerUnit;

        Debug.Log($"[상점] {currentItemData.name} {amount}개 구입 총 금액 {totalCost}Gold");

        // 인벤토리 추가 및 골드 차감 처리 

        Hide();
    }

    private void OnClickExit()
    {
        Hide();
    }
}
