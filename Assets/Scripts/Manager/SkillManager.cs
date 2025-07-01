using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [Header("모든 스킬 목록")]
    public List<SkillData> allSkills;

    [Header("해금된 스킬 및 상태")]
    public List<SkillSaveData> unlockedSkills = new();

    [Header("현재 장착된 스킬 슬롯(QWER 순)")]
    public List<int> skillSlotIds = new();

    // 속성별 기본 공격 저장용
    private Dictionary<ElementType, int> basicAttackSkillMap = new();

    private void Start()
    {
        RegisterBasicAttack(ElementType.Fire, 100);
        RegisterBasicAttack(ElementType.Ice, 200);
        RegisterBasicAttack(ElementType.Lightning, 300);
        RegisterBasicAttack(ElementType.Earth, 400);
    }
    public void UnlockSkill(int skillId)
    {
        if (!GameManager.Instance.playerManager.stat.UseSkillPoint(1)) return;

        var skill = unlockedSkills.Find(s => s.id == skillId);
        if (skill != null)
        {
            skill.level++;
        }
        else 
        {
            unlockedSkills.Add(new SkillSaveData { id = skillId, level = 1, isUnlocked = true });
        }
    }
    // 현재 스킬 슬롯 설정
    public void SetSkillSlots(List<int> slotIds)
    {
        skillSlotIds = new List<int>(slotIds);
    }
    public SkillSaveData GetSkillSave(int skillId)
    {
        return unlockedSkills.Find(s => s.id == skillId);
    }
    public List<int> GetSkillSlotIds() => skillSlotIds;

    public List<SkillSaveData> GetUnlockedSkillSaveData() => unlockedSkills;
    public void LoadUnlockedSkills(List<SkillSaveData> data) => unlockedSkills = data;

    // ID로 SkillData를 가져옴
    public SkillData GetSkillDataById(int id)
    {
        return allSkills.Find(s => s.id == id);
    }
    // 스킬 해금 여부 확인
    public bool IsUnlocked(int id)
    {
        return unlockedSkills.Exists(s => s.id == id && s.isUnlocked);
    }
    // 스킬 현재 레벨 현황
    public int GetSkillLevel(int id)
    {
        var skill = unlockedSkills.Find(s => s.id == id); 
        return skill != null ? skill.level : 0;
    }
    // 기본 공격 등록
    public void RegisterBasicAttack(ElementType element, int skillId)
    {
        if (!basicAttackSkillMap.ContainsKey(element))
            basicAttackSkillMap.Add(element, skillId);
        else
            basicAttackSkillMap[element] = skillId;
    }
    // 기본 공격 가져오기
    public SkillData GetBasicAttackByElement(ElementType element)
    {
        if (basicAttackSkillMap.TryGetValue(element, out int skillId))
        {
            return GetSkillDataById(skillId);
        }
        return null;
    }
}
