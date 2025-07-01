using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : IPlayerState
{
    private readonly PlayerController controller;
    private readonly PlayerStateMachine stateMachine;

    public MoveState(PlayerController controller, PlayerStateMachine machine)
    {
        this.controller = controller;
        this.stateMachine = machine;
    }
    public void Enter()
    {
        controller.MoveAnim(Vector3.forward); 
    }
    public void Tick()
    {
        controller.MoveAnim(controller.agent.velocity);

        if (controller.IsArrived())
            stateMachine.ChangeState(new IdleState(controller, stateMachine));
    }
    public void Exit() { }
}
