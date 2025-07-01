using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DragonBoarAi : EnemyStateMachine, IPatternEnemy
{
    [Header("Enemy Settings")]
    public int enemyID;

    [Header("Attack Colliders")]
    [SerializeField] private GameObject basicCol;
    [SerializeField] private GameObject hornCol;

    protected override void Start()
    {
        // 데이터 로드
        var data = EnemyDataLoader.Instance.enemyList.Find(e => e.id == enemyID);

        if (data == null)
        {
            Debug.LogError("DragonBoar 데이터 로드 실패");
            return;
        }

        SetData(data);
        base.Start();

        Debug.Log($"DragonBoar 데이터 로드 완료: {enemyData.name}");

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
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist > 5f)
        {
            animator.SetTrigger("Horn Attack");
        }
        else
        {
            animator.SetTrigger("Basic Attack");
        }
    }
    public void EnableBasicCol() => basicCol.SetActive(true);
    public void DisableBasicCol() => basicCol.SetActive(false);

    public void EnableHornCol() => hornCol.SetActive(true);
    public void DisableHornCol() => hornCol.SetActive(false);

    private void DisableAllColliders()
    {
        basicCol.SetActive(false);
        hornCol.SetActive(false);
    }
}
