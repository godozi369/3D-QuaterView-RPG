using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerSaveData 
{
    public float maxHp, currentHp, maxMp, currentMp;
    public int level, exp;
    public string playerName;

    public float intelligence, dexerity, vital, luck, energy, defense;
    public int statPoint;

    public int gold;
    public List<int> inventoryItemIds;
    public List<int> inventoryItemCounts;
    public int equippedWeaponId;
    public int equippedArmorId;

    public int skillPoint;
    public List<SkillSaveData> unlockedSkills;
    public List<int> skillSlotIds;
}
