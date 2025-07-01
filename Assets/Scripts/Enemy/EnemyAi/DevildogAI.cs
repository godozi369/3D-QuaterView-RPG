using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DevildogAI : EnemyStateMachine
{
    public int enemyID;

    protected override void Start()
    {
        var data = EnemyDataLoader.Instance.enemyList.Find(e => e.id == enemyID);

        if (data == null)
        {
            Debug.LogError("데빌독 데이터 로드 실패");
            return;
        }

        SetData(data);
        Debug.Log($"데빌독 데이터 로드 성공 : {enemyData.name}");
        
        agent = GetComponent<NavMeshAgent>();
        agent.speed = enemyData.moveSpeed;

        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }
}
