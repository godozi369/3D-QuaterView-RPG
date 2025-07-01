using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropTable", menuName = "Game Data/Drop Table")]
public class DropTable : ScriptableObject
{
    public int monsterId;
    public List<DropEntry> drops = new();
}
