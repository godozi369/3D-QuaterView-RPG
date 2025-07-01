using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : IPlayerState
{
    private readonly PlayerController controller;
    private readonly PlayerStateMachine stateMachine;

    public DieState(PlayerController controller, PlayerStateMachine stateMachine)
    {
        this.controller = controller;
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        controller.animator.SetTrigger("Die");
    }
    public void Tick()
    {
    }
    public void Exit()
    {

    }
}
