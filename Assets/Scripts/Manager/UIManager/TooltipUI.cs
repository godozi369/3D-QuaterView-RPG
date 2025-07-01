using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI rarityText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private void Start()
    {
        panel.SetActive(false);
    }

    public void Show(ItemData data)
    {
        if (data == null) return;

        rarityText.text = data.rarity; 
        nameText.text = data.name;
        descriptionText.text = data.description;

        switch (data.itemType)
        {
            case ItemType.Weapon:
                descriptionText.text += $"\n공격력: {data.weaponStat.weaponDamage}" +
                                            $"\n공격속도: {data.weaponStat.weaponSpeed}";
                break;

            case ItemType.Armor:
                descriptionText.text += $"\n방어력: {data.armorStat.defense}";
                break;

            case ItemType.Consumable:
                descriptionText.text += $"\n효과: {GetEffectText(data.effect)}";
                break;
        }

        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
    private string GetEffectText(ConsumableEffect effect)
    {
        return effect.type switch
        {
            EffectType.Hp => $"HP 회복 +{effect.value}\n쿨타임 : {effect.cooldown}", 
            EffectType.Mp => $"MP 회복 +{effect.value}\n쿨타임 : {effect.cooldown}", 
            _ => "알 수 없는 효과"
        };
    }
}
