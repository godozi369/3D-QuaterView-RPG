using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropEntry 
{
    public int itemId;
    public GameObject dropPrefab;
    [Range(0f, 1f)] public float dropRate;
    public int minAmount = 1;
    public int maxAmount = 1;
}
