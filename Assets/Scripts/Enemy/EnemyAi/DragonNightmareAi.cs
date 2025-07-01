using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DragonNightmareAi : EnemyStateMachine, IPatternEnemy
{
    [Header("Enemy Settings")]
    public int enemyID;

    [Header("Attack Colliders")]
    [SerializeField] private GameObject basicCol;
    [SerializeField] private GameObject hornCol;
    [SerializeField] private GameObject clawCol;


    protected override void Start()
    {
        // 데이터 로드
        var data = EnemyDataLoader.Instance.enemyList.Find(e => e.id == enemyID);

        if (data == null)
        {
            Debug.LogError("DragonNightmare 데이터 로드 실패");
            return;
        }

        SetData(data);
        base.Start();
        Debug.Log($"DragonNightmare 데이터 로드 완료: {enemyData.name}");

        agent = GetComponent<NavMeshAgent>();
        agent.speed = enemyData.moveSpeed;

        DisableAllColliders();
    }
    protected override void Update()
    {
        base.Update();
    }
    public void AttackPattern()
    {
        int rand = Random.Range(0, 3);
        switch (rand)
        {
            case 0: animator.SetTrigger("Basic Attack"); break;
            case 1: animator.SetTrigger("Claw Attack"); break;
            case 2: animator.SetTrigger("Horn Attack"); break; 
        }
    }

    public void EnableBasicCol() => basicCol.SetActive(true);
    public void DisableBasicCol() => basicCol.SetActive(false);

    public void EnableHornCol() => hornCol.SetActive(true);
    public void DisableHornCol() => hornCol.SetActive(false);
    public void EnableClawCol() => clawCol.SetActive(true);
    public void DisableClawCol() => clawCol.SetActive(false);
    private void DisableAllColliders()
    {
        basicCol.SetActive(false);
        hornCol.SetActive(false);
        clawCol.SetActive(false);
    }
}
