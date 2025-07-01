using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDieState : IEnemyState
{
    private EnemyStateMachine enemy;
    private EnemyData enemyData;

    public EnemyDieState(EnemyStateMachine enemy)
    {
        this.enemy = enemy;
        this.enemyData = enemy.GetData();
    }

    public void Enter()
    {
        enemy.agent.isStopped = true;
        enemy.agent.updatePosition = false;
        enemy.agent.updateRotation = false;

        enemy.animator.SetTrigger("Die");
        DropLoot();
        GameObject.Destroy(enemy.gameObject, 1f);
    }

    public void Tick() { }
    public void Exit() { }

    private void DropLoot()
    {
        Vector3 basePos = enemy.transform.position;

        // 골드 드랍
        int gold = Random.Range(enemyData.dropGoldMin, enemyData.dropGoldMax + 1);
        if (gold > 0)
        {
            GameObject goldPrefab = Resources.Load<GameObject>("Prefabs/Loot/Gold");
            Vector3 dropPos = basePos + new Vector3(Random.Range(-0.5f, 0.5f), 0.1f, Random.Range(-0.5f, 0.5f));

            GameObject goldObject = GameObject.Instantiate(goldPrefab, dropPos, Quaternion.identity);
            goldObject.GetComponent<GoldPickUp>()?.Init(gold);
        }

        // 아이템 드랍 
        if (enemyData.dropTable != null)
        {
            foreach (DropEntry entry in enemyData.dropTable.drops)
            {
                float chance = Random.value;
                if (chance <= entry.dropRate)
                {
                    int amount = Random.Range(entry.minAmount, entry.maxAmount + 1);
                    Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), 0.1f, Random.Range(-0.5f, 0.5f));
                    Vector3 dropPos = enemy.transform.position + offset;

                    GameObject itemObject = GameObject.Instantiate(entry.dropPrefab, dropPos, Quaternion.identity);

                    ItemData data = ItemDataBase.Instance.GetItemById(entry.itemId);
                    itemObject.GetComponent<ItemPickUp>()?.Init(data, amount);
                }
            }
        }
    }
}
