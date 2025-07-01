using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData 
{
    public int id;
    public string name;
    public float maxHp;
    public float attackDamage;
    public float attackCooldown;
    public float moveSpeed;
    public float detectRange;
    public float attackRange;
    public float hurtTime;
    public int exp;
    public int dropGoldMin;
    public int dropGoldMax;

    [Header("드랍 테이블")]
    public DropTable dropTable;
}
