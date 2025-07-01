using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointSetter : MonoBehaviour
{
    void Start()
    {
        string objName = PlayerPrefs.GetString("ReturnObject", "");
        if (!string.IsNullOrEmpty(objName))
        {
            GameObject spawnTarget = GameObject.Find(objName);
            if (spawnTarget != null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    player.transform.position = spawnTarget.transform.position;
                }
            }
        }
    }
}
