using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillState : IPlayerState
{
    private readonly PlayerController controller;
    private readonly PlayerStateMachine stateMachine;
    private readonly SkillData skill;

    public SkillState(PlayerController controller, PlayerStateMachine stateMachine, SkillData skill)
    {
        this.controller = controller;
        this.stateMachine = stateMachine;
        this.skill = skill;
    }

    public void Enter()
    {
        Debug.Log("SkillState Enter");

        controller.MouseDirection();
        controller.agent.isStopped = true;
        controller.animator.SetFloat("Move", 0f);
        controller.agent.ResetPath();

        if (!string.IsNullOrEmpty(skill.animationTrigger))
            controller.animator.SetTrigger(skill.animationTrigger);

        controller.skillSystem.TryCast(skill);
        stateMachine.ChangeState(new IdleState(controller, stateMachine));
    }

    public void Tick() { }

    public void Exit()
    {
        controller.agent.isStopped = false;
    }
}
