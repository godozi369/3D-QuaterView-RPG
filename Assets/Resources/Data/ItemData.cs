using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData 
{
    public int id;
    public ItemType itemType;
    public string name;
    public string rarity;
    public string description;
    public string iconPath;

    // Json 파싱 무시
    [NonSerialized] public Sprite icon;

    public WeaponStat weaponStat;
    public ArmorStat armorStat;
    public ConsumableEffect effect;
    public MaterialInfo materialInfo;
}
public enum ItemType { Weapon, Armor, Consumable, Material }


[System.Serializable]
public class WeaponStat
{
    public float weaponDamage;
    public float weaponSpeed;
}
[System.Serializable]
public class ArmorStat
{
    public float defense;
}
[System.Serializable]
public class ConsumableEffect
{
    public EffectType type;
    public int value;
    public float cooldown;
}
[System.Serializable]
public class MaterialInfo
{
    public string element;
    public string grade;
    public int fusionValue;
}

public enum EffectType
{
    Hp,
    Mp
}