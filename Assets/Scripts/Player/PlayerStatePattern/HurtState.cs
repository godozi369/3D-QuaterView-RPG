using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class HurtState : IPlayerState
{
    private readonly PlayerController controller;
    private readonly PlayerStateMachine stateMachine;
    private float timer;

    public HurtState(PlayerController controller, PlayerStateMachine stateMachine)
    {
        this.controller = controller;
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        timer = 0f;
        controller.agent.isStopped = true;
        controller.agent.updatePosition = false;
        controller.agent.updateRotation = false;
        controller.animator.SetTrigger("Get Hit");
    }
    public void Tick()
    {
        timer += Time.deltaTime;
        if (timer >= 0.5f)
        {
            stateMachine.ChangeState(new IdleState(controller, stateMachine));  
        }
    }
    public void Exit()
    {
        controller.agent.isStopped = false;
        controller.agent.updatePosition = true;
        controller.agent.updateRotation = true;
    }
}
