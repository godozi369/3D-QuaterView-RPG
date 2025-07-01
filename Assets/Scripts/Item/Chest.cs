using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public DropTable dropTable; 
    public Animator animator;
    public Transform dropPoint;

    [Header("Gold Drop")]
    public int dropGoldMin = 100;
    public int dropGoldMax = 200;

    private bool isOpened = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && PlayerInRange() && !isOpened)
        {
            OpenChest();
        }
    }

    void OpenChest()
    {
        isOpened = true;
        animator.SetTrigger("Open");

        foreach (var drop in dropTable.drops)
        {
            if (Random.value <= drop.dropRate)
            {
                int amount = Random.Range(drop.minAmount, drop.maxAmount + 1);
                for (int i = 0; i < amount; i++)
                {
                    float radius = 2f;
                    Vector2 randomCircle = Random.insideUnitCircle * radius;
                    Vector3 offset = new Vector3(randomCircle.x, 0.1f, randomCircle.y);
                    Vector3 dropPos = dropPoint.position + dropPoint.forward * 2f + offset;

                    int id = drop.itemId;
                    ItemData itemData = ItemDataBase.Instance.GetItemById(id);

                    GameObject item = Instantiate(drop.dropPrefab, dropPos, drop.dropPrefab.transform.rotation);
                    var pickUp = item.GetComponent<ItemPickUp>();
                    if (pickUp != null)
                    {
                        pickUp.Init(itemData, amount);
                    }
                }
            }
        }

        int dropCount = Random.Range(3, 6); // 3~5개 골드 드랍
        GameObject goldPrefab = Resources.Load<GameObject>("Prefabs/Loot/Gold");

        for (int i = 0; i < dropCount; i++)
        {
            float angle = Random.Range(-90f, 90f);
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
            Vector3 direction = rotation * dropPoint.forward;

            Vector3 goldDropPos = dropPoint.position + direction * Random.Range(1.0f, 2.5f) + Vector3.up * 0.3f;

            GameObject goldObject = Instantiate(goldPrefab, goldDropPos, goldPrefab.transform.rotation);
            goldObject.GetComponent<GoldPickUp>()?.Init(Random.Range(dropGoldMin, dropGoldMax + 1));
        }
    }

    bool PlayerInRange()
    {
        float dist = Vector3.Distance(transform.position, GameManager.Instance.playerManager.controller.transform.position);
        return dist < 2.5f;
    }
}
