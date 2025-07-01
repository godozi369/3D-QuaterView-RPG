using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class PlayerDataHandler 
{
    [SerializeField] private PlayerStat stat;
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private SkillManager skillManager;

    private string savePath;

    public PlayerDataHandler(PlayerStat stat, PlayerInventory inventory, SkillManager skillManager)
    {
        this.stat = stat;
        this.inventory = inventory;
        this.skillManager = skillManager;
        this.savePath = Application.persistentDataPath + "/playerdata.json";
    }
    public void Save()
    {
        PlayerSaveData data = new PlayerSaveData
        {
            maxHp = stat.maxHp,
            currentHp = stat.currentHp,
            maxMp = stat.maxMp,
            currentMp = stat.currentMp,
            level = stat.level,
            exp = stat.exp,
            playerName = stat.playerName,

            intelligence = stat.intelligence,
            dexerity = stat.dexterity,
            vital = stat.vital,
            luck = stat.luck,
            energy = stat.energy,
            defense = stat.defense,
            statPoint = stat.statPoint,

            gold = stat.gold,

            inventoryItemIds = inventory.items.Select(item => item.id).ToList(),
            inventoryItemCounts = inventory.items
                .Select(item => item.itemType == ItemType.Consumable ? inventory.GetItemCount(item.id) : 1)
                .ToList(),

            equippedWeaponId = inventory.equippedWeapon?.id ?? -1,
            equippedArmorId = inventory.equippedArmor?.id ?? -1,

            skillPoint = stat.skillPoint,
            unlockedSkills = skillManager.GetUnlockedSkillSaveData(),
            skillSlotIds = skillManager.GetSkillSlotIds()
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("플레이어 데이터 저장 완료");
    }

    public void Load()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("저장된 데이터가 없습니다.");
            return;
        }

        string json = File.ReadAllText(savePath);
        PlayerSaveData data = JsonUtility.FromJson<PlayerSaveData>(json);

        stat.maxHp = data.maxHp;
        stat.currentHp = data.currentHp;
        stat.maxMp = data.maxMp;
        stat.currentMp = data.currentMp;
        stat.level = data.level;
        stat.exp = data.exp;
        stat.playerName = data.playerName;

        stat.intelligence = data.intelligence;
        stat.dexterity = data.dexerity;
        stat.vital = data.vital;
        stat.luck = data.luck;
        stat.energy = data.energy;
        stat.defense = data.defense;
        stat.statPoint = data.statPoint;

        stat.gold = data.gold;

        inventory.items.Clear();
        Dictionary<int, int> counts = new();

        for (int i = 0; i < data.inventoryItemIds.Count; i++)
        {
            var item = ItemDataBase.Instance.GetItemById(data.inventoryItemIds[i]);
            if (item != null)
            {
                inventory.items.Add(item);

                if (item.itemType == ItemType.Consumable)
                {
                    int count = (i < data.inventoryItemCounts.Count) ? data.inventoryItemCounts[i] : 1;
                    counts[item.id] = count;
                }
            }
        }

        inventory.SetItemCounts(counts);

        inventory.EquipWeaponById(data.equippedWeaponId);
        inventory.EquipArmorById(data.equippedArmorId);

        stat.skillPoint = data.skillPoint;

        skillManager.LoadUnlockedSkills(data.unlockedSkills);
        skillManager.SetSkillSlots(data.skillSlotIds);

        Debug.Log("플레이어 데이터 불러오기 완료");
    }
}
