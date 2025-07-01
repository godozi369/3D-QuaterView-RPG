using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class AttackState : IPlayerState
{
    private readonly PlayerController controller;
    private readonly PlayerStateMachine stateMachine;

    public AttackState(PlayerController controller, PlayerStateMachine machine)
    {
        this.controller = controller;
        this.stateMachine = machine;
    }

    public void Enter()
    {
        Debug.Log("AttackState Enter");
        controller.MouseDirection();
        controller.animator.SetFloat("Move", 0f);
        controller.agent.isStopped = true;
        controller.agent.updateRotation = false;
        controller.agent.ResetPath();

        Debug.Log("SetTrigger 호출됨");
        controller.animator.SetTrigger("BasicAttack");

        SkillData basicAttack = controller.skillSystem.GetBasicAttack();
        controller.skillSystem.TryCast(basicAttack);

        stateMachine.ChangeState(new IdleState(controller, stateMachine));
    }

    public void Tick() { }
    public void Exit() 
    {
        controller.agent.isStopped = false;
        controller.agent.updateRotation = true;
    }

}

