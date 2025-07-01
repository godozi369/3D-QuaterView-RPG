using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerStat stat;

    [HideInInspector] public Animator animator;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public PlayerInputAction inputAction;
    [HideInInspector] public Vector3 moveDir;

    public ElementType currentElement;
    public SkillSystem skillSystem;
    public SkillInventory skillInventory;
    public SkillHUD skillHud;

    private PlayerStateMachine stateMachine;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        stat = stat != null ? stat : GetComponent<PlayerStat>();
        inputAction = new PlayerInputAction();
        stateMachine = GetComponent<PlayerStateMachine>();
        skillSystem = GetComponent<SkillSystem>();

    }
    private void Start()
    {
        stateMachine.ChangeState(new IdleState(this, stateMachine));
    }
   
    private void OnEnable()
    {
        Debug.Log("OnEnable 호출됨");
        inputAction.Enable();
        inputAction.Player.Move.performed += OnClick;
        inputAction.Player.Roll.performed += _ => Roll();
        inputAction.Player.Attack.performed += _ => Attack();
        inputAction.Player.Skill.performed += OnSkillPerformed;
    }
    private void OnDisable()
    {
        inputAction.Disable();
    }
    public void Init(PlayerStat stat, SkillInventory skillInventory, SkillHUD hud)
    {
        this.stat = stat;
        this.skillInventory = skillInventory;
        this.skillHud = hud;
    }
    private void OnClick(InputAction.CallbackContext context)
    {
        if (stateMachine.CurrentState is RollState) return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (agent.enabled)
            {
                agent.SetDestination(hit.point);
                stateMachine.ChangeState(new MoveState(this, stateMachine));
            }
        }
        
    }
    
    private void Roll()
    {
        if (stateMachine.CurrentState is not RollState)
            stateMachine.ChangeState(new RollState(this, stateMachine, stat));
    }
    private void Attack()
    {
        if (stateMachine.CurrentState is not AttackState)
            stateMachine.ChangeState(new AttackState(this, stateMachine));
    }
    public void TakeDamage(float damage)
    {
        stat.currentHp -= damage;
        stat.currentHp = Mathf.Max(0, stat.currentHp);

        Debug.Log($"피격! 플레이어 남은 체력:{stat.currentHp}");

        if(stat.damageTextSpawner != null)
            stat.damageTextSpawner.ShowDamage(damage);

        if (stat.currentHp <= 0f)
        {
            Die();
        }
        else
        {
            stateMachine.ChangeState(new HurtState(this, stateMachine));
        }

        stat.InvokeStatChanged();
    }
    public void Die()
    {
        Debug.Log("플레이어 사망");
        stateMachine.ChangeState(new DieState(this, stateMachine));
    }
    public bool IsArrived()
    {
        if (!agent.enabled || agent.pathPending) return false;
        return agent.remainingDistance <= agent.stoppingDistance;
    }
    public void MouseDirection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 dir = (hit.point - transform.position).normalized;
            dir.y = 0;
            if (dir != Vector3.zero)
            {
                transform.forward = dir.normalized;
            }
        }
    }
    public void MoveAnim(Vector3 moveDir)
    {
        float speed = moveDir.magnitude;
        animator?.SetFloat("Move", speed);
    }

    public void RollAnim()
    {
        animator?.SetTrigger("Roll");
    }
    private void OnSkillPerformed(InputAction.CallbackContext ctx)
    {
        string key = ctx.control.displayName;

        int index = key switch
        {
            "Q" => 0,
            "W" => 1,
            "E" => 2,
            "R" => 3,
            _ => -1
        };

        if (index == -1) return;

        SkillData skill = skillSystem.GetSkillAt(index);
        if (skill == null) return;

        SkillSlotUI slot = skillHud.GetSlotByIndex(index);
        if (slot == null || !slot.IsUsable()) return;

        // 쿨타임 시작
        slot.UseSkill();

        stateMachine.ChangeState(new SkillState(this, stateMachine, skill));
    }

}
