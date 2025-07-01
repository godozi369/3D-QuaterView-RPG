using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDataLoader : MonoBehaviour
{
    public static EnemyDataLoader Instance { get; private set; }

    public TextAsset csvFile;
    public List<EnemyData> enemyList = new List<EnemyData>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        ParseCSV();
    }

    void ParseCSV()
    {
        string[] lines = csvFile.text.Split('\n');
        for (int i = 1; i < lines.Length; i++) // 첫 줄은 헤더니까 스킵
        {
            string[] values = lines[i].Split(',');
            if (values.Length < 12) continue;

            EnemyData data = new EnemyData
            {
                id = int.Parse(values[0]),
                name = values[1],
                maxHp = float.Parse(values[2]),
                attackDamage = float.Parse(values[3]),
                attackCooldown = float.Parse(values[4]),
                moveSpeed = float.Parse(values[5]),
                detectRange = float.Parse(values[6]),
                attackRange = float.Parse(values[7]),
                hurtTime = float.Parse(values[8]),
                exp = int.Parse(values[9]),
                dropGoldMin = int.Parse(values[10]),
                dropGoldMax = int.Parse(values[11]),
            };

            enemyList.Add(data);
        }
    }
}
