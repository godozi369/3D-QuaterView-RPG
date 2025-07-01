using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    [SerializeField] private List<PoolObject> poolObjects;
    private Dictionary<string, Queue<GameObject>> poolDictionary = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializePools();
    }

    private void InitializePools()
    {
        foreach (var obj in poolObjects)
        {
            Queue<GameObject> objectPool = new();

            for (int i = 0; i < obj.initialSize; i++)
            {
                GameObject instance = Instantiate(obj.prefab);
                instance.SetActive(false);
                objectPool.Enqueue(instance);
            }
            poolDictionary.Add(obj.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
        }

        GameObject objectToSpawn = null;

        if (poolDictionary[tag].Count > 0)
        {
            objectToSpawn = poolDictionary[tag].Dequeue();
        }
        else
        {
            var prefab = poolObjects.Find(p => p.tag == tag)?.prefab;
            if (prefab != null)
                objectToSpawn = Instantiate(prefab);
        }

        objectToSpawn.transform.SetPositionAndRotation(position, rotation);
        objectToSpawn.transform.SetParent(parent);
        objectToSpawn.SetActive(true);

        objectToSpawn.GetComponent<IPoolable>()?.OnSpawn();
        return objectToSpawn;
    }

    public void ReturnToPool(string tag, GameObject obj)
    {
        obj.GetComponent<IPoolable>()?.OnDespawn();
        obj.SetActive(false);
        obj.transform.SetParent(null);

        if (!poolDictionary.ContainsKey(tag))
            poolDictionary.Add(tag, new Queue<GameObject>());

        poolDictionary[tag].Enqueue(obj);
    }
}

