using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPhase
    {
        public GameObject[] monsters;
    }

    public SpawnPhase[] phases;
    private int currentPhase = 0;
    private List<GameObject> activeMonsters = new();

    public void StartPhase()
    {
        if (currentPhase >= phases.Length) return;

        foreach (var prefab in phases[currentPhase].monsters)
        {
            GameObject monster = Instantiate(prefab, transform.position, Quaternion.identity);
            activeMonsters.Add(monster);
        }

        StartCoroutine(CheckPhaseClear());
    }

    private IEnumerator CheckPhaseClear()
    {
        while (true)
        {
            activeMonsters.RemoveAll(m => m == null);

            if (activeMonsters.Count == 0)
            {
                currentPhase++;
                StartPhase(); // 다음 페이즈 시작
                yield break;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    void Start() => StartPhase();
}