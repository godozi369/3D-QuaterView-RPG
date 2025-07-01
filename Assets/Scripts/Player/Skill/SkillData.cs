using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skill/SkillData")]
public class SkillData : ScriptableObject
{
    [Header("Basic Info")]
    public int id;
    public string skillName;
    public string description;
    public Sprite icon;
    public GameObject skillPrefab;

    [Header("Type Info")]
    public SkillType skillType;
    public ElementType elementType;
    public TargetingType targetingType;

    [Header("Combat Stats")]
    public float damage;
    public float cooldown;
    public int manaCost;

    [Header("Leveling")]
    public int maxLevel = 6;
    public int requireLevel;
    public AnimationCurve damageByLevel;
    public AnimationCurve cooldownByLevel;
    public AnimationCurve manaCostByLevel;


    [Header("Effects")]
    public GameObject flashEffect;
    public GameObject hitEffect;

    [Header("AOE Settings")]
    public GameObject aoeIndicatorPrefab;  

    [Header("Animation")]
    public string animationTrigger = "";
}
public enum SkillType
{
    BasicAttack,
    Active,
    Passive,
    Utility
}
public enum ElementType
{
    None,
    Fire,
    Ice,
    Lightning,
    Earth,
}
public enum TargetingType
{
    SingleTarget,
    AOE,
    Projectile,
    Self,
    Cone
}