using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SkillInventory : MonoBehaviour
{
    [System.Serializable]
    public class SkillSet
    {
        public ElementType element;
        public List<SkillData> skills = new();
    }

    [Header("Element Switching")]
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private GameObject commonIcon ,fireIcon, iceIcon, lightningIcon, earthIcon;

    [Header("Slot Group")]
    [SerializeField] private GameObject slotsGroup;

    [Header("Skill Data")]
    [SerializeField] private List<SkillSet> skillSets = new();

    private int currentIndex = 0;
    private ElementType[] elementOrder = { ElementType.None ,ElementType.Fire, ElementType.Ice, ElementType.Lightning, ElementType.Earth };

    private void Start()
    {
        leftButton.onClick.AddListener(() => SwitchElement(-1));
        rightButton.onClick.AddListener(() => SwitchElement(1));

        ApplyElement(elementOrder[currentIndex]);
    }

    private void SwitchElement(int dir)
    {
        currentIndex = (currentIndex + dir + elementOrder.Length) % elementOrder.Length;
        ApplyElement(elementOrder[currentIndex]);
    }

    private void ApplyElement(ElementType type)
    {
        UpdateSlotGroup(type);
        UpdateElementIcon(type);
    }

    private void UpdateSlotGroup(ElementType type)
    {
        var skillSet = GetSkillSet(type);
        var slots = slotsGroup.GetComponentsInChildren<SkillInventorySlotUI>(true);

        foreach (var slot in slots)
        {
            int index = slot.GetSlotIndex();
            SkillData data = (index < skillSet.skills.Count) ? skillSet.skills[index] : null;
            bool learned = data != null;
            slot.SetSkillData(data, learned);
        }
    }

    private void UpdateElementIcon(ElementType type)
    {
        commonIcon.SetActive(false);
        fireIcon.SetActive(false);
        iceIcon.SetActive(false);
        lightningIcon.SetActive(false);
        earthIcon.SetActive(false);

        switch (type)
        {
            case ElementType.None:
                commonIcon.SetActive(true);
                break;
            case ElementType.Fire:
                fireIcon.SetActive(true);
                break;
            case ElementType.Ice:
                iceIcon.SetActive(true);
                break;
            case ElementType.Lightning:
                lightningIcon.SetActive(true);
                break;
            case ElementType.Earth:
                earthIcon.SetActive(true);
                break;
        }
    }

    public List<SkillData> GetSkillsFor(ElementType element)
    {
        foreach (var set in skillSets)
        {
            if (set.element == element)
                return set.skills;
        }
        return new List<SkillData>();
    }

    public void LearnSkill(ElementType element, SkillData newSkill)
    {
        var set = skillSets.Find(s => s.element == element);
        if (set == null)
        {
            set = new SkillSet { element = element };
            skillSets.Add(set);
        }
        if (!set.skills.Contains(newSkill))
            set.skills.Add(newSkill);
    }

    public SkillSet GetSkillSet(ElementType element)
    {
        return skillSets.Find(s => s.element == element);
    }

    public ElementType GetCurrentElement() => elementOrder[currentIndex];
}

