using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyStateMachine : MonoBehaviour
{
    public Animator animator;
    public Transform player;
    public NavMeshAgent agent;

    protected EnemyData enemyData;
    protected IEnemyState currentState;
    private EnemyCombatHandler combatHandler;

    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
        combatHandler = GetComponent<EnemyCombatHandler>();
        agent = GetComponent<NavMeshAgent>();   

        if (enemyData == null)
        {
            Debug.LogError("enemyData가 설정되지 않음");
            return;
        }

        combatHandler.Init(enemyData.maxHp);
        ChangeState(new EnemyRoamState(this));
    }

    protected virtual void Update()
    {
        currentState?.Tick();
    }

    public void SetData(EnemyData data) => enemyData = data;
    public EnemyData GetData() => enemyData;
    public Animator GetAnimator() => animator;
    public EnemyCombatHandler GetCombatHandler() => combatHandler;

    public void MoveTo(Vector3 dest)
    {
        if (agent != null && agent.enabled)
        {
            agent.SetDestination(dest);
        }
    }

    public void ChangeState(IEnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
    
    public bool IsPlayerInDetectRange()
    {
        return Vector3.Distance(transform.position, player.position) <= enemyData.detectRange;
    }

    public bool IsPlayerInAttackRange()
    {
        return Vector3.Distance(transform.position, player.position) <= enemyData.attackRange;
    }

    private void OnDrawGizmos()
    {
        if(enemyData ==null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemyData.detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyData.attackRange);
    }
}
