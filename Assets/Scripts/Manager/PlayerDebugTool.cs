using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebugTool : MonoBehaviour
{
    [SerializeField] private PlayerStat stat;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("경험치 획득");
            stat.GainExp(5);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("체력 닳기");
            stat.TakeDamage(5);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("마나 사용");
            stat.UseMp(5);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("플레이어 상태 초기화");
            stat.currentHp = stat.maxHp;
            stat.currentMp = stat.maxMp;
            stat.exp = 0;
            stat.level = 1;
            stat.statPoint = 0;
            stat.skillPoint = 0;
            stat.InvokeStatChanged();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            stat.maxHp = 6666f;
            stat.maxMp = 6666f;
            stat.level = 666;
            stat.baseDamage = 66;
            stat.currentHp = 6666f;
            stat.currentMp = 6666f;
            stat.gold = 6666666;
            stat.skillPoint = 666;
            stat.luck = 777;
            stat.playerName = "ANG";
            stat.InvokeStatChanged();
        }
    }
}
