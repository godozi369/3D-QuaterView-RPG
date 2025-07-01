using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : MonoBehaviour
{
    public static ItemDataBase Instance { get; private set; }

    [SerializeField] private TextAsset jsonFile; // Resources/Data/ItemData.json

    private Dictionary<int, ItemData> itemDict = new Dictionary<int, ItemData>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        LoadItemFromJson();
    }

    private void LoadItemFromJson()
    {
        try
        {
            ItemDataList itemList = JsonUtility.FromJson<ItemDataList>(jsonFile.text);
            if (itemList?.items == null) return;

            foreach (var item in itemList.items)
            {
                itemDict[item.id] = item; // 중복 키 방지 불필요

                item.icon = Resources.Load<Sprite>(item.iconPath);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[ItemDataBase] JSON 로드 실패: {e.Message}");
        }
    }
    public ItemData GetItemById(int id)
    {
        return itemDict.TryGetValue(id, out var item) ? item : null;
    }
}
