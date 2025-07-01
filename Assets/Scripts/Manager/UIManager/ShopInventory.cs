using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ShopCategoryType { Weapon, Armor, Potion };
public class ShopInventory : MonoBehaviour
{
    [System.Serializable]
    public class ShopCategory
    {
        public ShopCategoryType type;
        public List<int> itemIds = new(); // ItemData의 id 목록
    }

    [Header("Category Switching")]
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private GameObject weaponIcon, armorIcon, potionIcon;

    [Header("Slot Group")]
    [SerializeField] private GameObject slotsGroup;
    [SerializeField] private GameObject weaponSlots;
    [SerializeField] private GameObject armorSlots;
    [SerializeField] private GameObject potionSlots;

    [Header("Shop Data")]
    [SerializeField] private List<ShopCategory> shopCategories = new();

    private int currentCategoryIndex = 0;

    private void Start()
    {
        leftButton.onClick.AddListener(() => SwitchCategory(-1));
        rightButton.onClick.AddListener(() => SwitchCategory(1));
        DisplayCategory();
    }

    private void SwitchCategory(int direction)
    {
        currentCategoryIndex += direction;

        if (currentCategoryIndex < 0)
            currentCategoryIndex = shopCategories.Count - 1;
        else if (currentCategoryIndex >= shopCategories.Count)
            currentCategoryIndex = 0;

            DisplayCategory();
    }
    private void DisplayCategory()
    {
        ShopCategory category = shopCategories[currentCategoryIndex];

        weaponSlots.SetActive(false);
        armorSlots.SetActive(false);
        potionSlots.SetActive(false);

        switch (category.type)
        {
            case ShopCategoryType.Weapon:
                slotsGroup = weaponSlots;
                weaponSlots.SetActive(true);
                weaponIcon.SetActive(true);
                armorIcon.SetActive(false);
                potionIcon.SetActive(false);
                break;
            case ShopCategoryType.Armor:
                slotsGroup = armorSlots;
                armorSlots.SetActive(true);
                weaponIcon.SetActive(false);
                armorIcon.SetActive(true);
                potionIcon.SetActive(false) ;
                break;
            case ShopCategoryType.Potion:
                slotsGroup = potionSlots;
                potionSlots.SetActive(true);
                weaponIcon.SetActive(false);
                armorIcon.SetActive(false);
                potionIcon.SetActive(true);
                break;
        }

        foreach (Transform child in slotsGroup.transform)
            child.gameObject.SetActive(false);

        for (int i = 0; i < category.itemIds.Count; i++)
        {
            if (i >= slotsGroup.transform.childCount)
            {
                Debug.LogWarning($"슬롯이 부족합니다 : {i}번 슬롯이 없음");
                continue;
            }

            Transform slot = slotsGroup.transform.GetChild(i);
            slot.gameObject.SetActive(true);

            ItemData data = ItemDataBase.Instance.GetItemById(category.itemIds[i]);
            if (data == null)
            {
                Debug.LogWarning($"ItemData를 찾을 수 없음. ID : {category.itemIds[i]}");
                continue;
            }

            slot.GetComponent<ShopItemUI>()?.SetItem(data);
        }

        // 떨이 카테고리 (랜덤)
    }
}

