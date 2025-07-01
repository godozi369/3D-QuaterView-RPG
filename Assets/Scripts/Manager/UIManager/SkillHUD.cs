using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHUD : MonoBehaviour
{
    [Header("기본공격/회피 슬롯")]
    [SerializeField] private SkillSlotUI basicAttackSlot;
    [SerializeField] private SkillSlotUI dodgeSlot;

    [Header("속성 기반 QWER 슬롯")]
    [SerializeField] private List<SkillSlotUI> elementSlots;

    [Header("소비 아이템 슬롯")]
    [SerializeField] private List<ItemSlotUI> consumableSlots;

    [Header("참조")]
    [SerializeField] private PlayerController player;

    public void UpdateSkillSlots()
    {
        var skills = player.skillSystem.GetCurrentSkills();

        for (int i = 0; i < elementSlots.Count; i++)
        {
            var skill = (i < skills.Count) ? skills[i] : null;
            elementSlots[i].SetSkill(skill);
        }
    }
    public void InitializeDefaultSlots(SkillData basic, SkillData dodge)
    {
        basicAttackSlot.SetSkill(basic);
        dodgeSlot.SetSkill(dodge);
    }
    public void SetBasicAttackSlot(SkillData basic)
    {
        basicAttackSlot.SetSkill(basic);
    }
    public SkillSlotUI GetSlotByIndex(int index)
    {
        if (index < 0 || index >= elementSlots.Count) return null;
        return elementSlots[index];
    }

    public bool IsSkillOnCooldown(int index)
    {
        var slot = GetSlotByIndex(index);
        return slot != null && !slot.IsUsable();
    }

    public bool IsBasicAttackOnCooldown() => basicAttackSlot != null && !basicAttackSlot.IsUsable();
    public bool IsDodgeOnCooldown() => dodgeSlot != null && !dodgeSlot.IsUsable();

    public bool IsConsumableOnCooldown(int index)
    {
        if (index < 0 || index >= consumableSlots.Count) return false;
        return !consumableSlots[index].IsUsable();
    }

}
