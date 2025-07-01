using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum SlotType
{
    BasicAttack = 0,
    Dodge = 1,
    ElementQ = 2,
    ElementW = 3,
    ElementE = 4,
    ElementR = 5,
    Consumable = 6
}

public class SkillSlotUI : MonoBehaviour, IDropHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public SlotType slotType;
    public Image iconImage;
    public Image cooldownImage;

    private SkillData currentSkill;
    private float cooldownTimer;
    private PlayerController player;
    public void SetSkill(SkillData skill)
    {
        if (!IsCompatible(skill))
        {
            Debug.LogWarning($"[SetSkill] 호환 안됨: {slotType} ≠ {skill.skillType}");
            return;
        }

        currentSkill = skill;
        iconImage.sprite = skill.icon;
        iconImage.color = new Color(1f, 1f, 1f, 1f);
        cooldownImage.fillAmount = 0;
    }
    public void InitBasicAttackUI(SkillData basicAttack)
    {
        SkillSlotUI[] allSlots = FindObjectsOfType<SkillSlotUI>();

        foreach (var slot in allSlots)
        {
            if (slot.slotType == SlotType.BasicAttack)
            {
                slot.SetSkill(basicAttack);
                Debug.Log($"[InitBasicAttackUI] 기본공격 등록 완료: {basicAttack.skillName}");
            }
        }
    }
    public void UseSkill()
    {
        if (currentSkill == null || cooldownTimer > 0f) return;
        cooldownTimer = currentSkill.cooldown;
    }
    public bool IsUsable()
    {
        return currentSkill != null && cooldownTimer <= 0f;
    }

    private void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownImage.fillAmount = cooldownTimer / currentSkill.cooldown;
        }
    }
    private bool IsCompatible(SkillData skill)
    {
        return slotType switch
        {
            SlotType.BasicAttack => skill.skillType == SkillType.BasicAttack,
            SlotType.Dodge => skill.skillType == SkillType.Utility,
            SlotType.ElementQ => skill.skillType == SkillType.Active || skill.skillType == SkillType.Utility,
            SlotType.ElementW => skill.skillType == SkillType.Active || skill.skillType == SkillType.Utility,
            SlotType.ElementE => skill.skillType == SkillType.Active || skill.skillType == SkillType.Utility,
            SlotType.ElementR => skill.skillType == SkillType.Active || skill.skillType == SkillType.Utility,
            SlotType.Consumable => false, // 스킬창은 소비 아이템 허용 안 함
            _ => false,
        };
    }
    public void ClearSlot()
    {
        currentSkill = null;
        iconImage.sprite = null;
        iconImage.color = new Color(1f, 1f, 1f, 0f);
        cooldownImage.fillAmount = 0;
    }
    public SkillData GetCurrentSkill() => currentSkill;

    public void OnDrop(PointerEventData eventData)
    {
        var dragged = eventData.pointerDrag;
        if (dragged == null) return;

        var dragUI = dragged.GetComponent<DraggableUI>();
        if (dragUI == null || dragUI.GetItemType() != DraggableUI.UIType.Skill) return;

        SkillData skill = dragUI.GetData() as SkillData;
        if (skill == null) return;


        if (player == null) player = FindObjectOfType<PlayerController>(); 
        
        SetSkill(skill);

        if (player != null)
        {
            int index = GetSlotIndex();
            player.skillSystem.UpdateSkillAt(index, skill);
        }
    }
    private int GetSlotIndex()
    {
        return slotType switch
        {
            SlotType.BasicAttack => 0,
            SlotType.Dodge => 1,
            SlotType.ElementQ => 0,
            SlotType.ElementW => 1,
            SlotType.ElementE => 2,
            SlotType.ElementR => 3,
            _ => -1
        };
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
}
