using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IPlayerState
{
    private readonly PlayerController controller;
    private readonly PlayerStateMachine stateMachine;

    public IdleState(PlayerController controller, PlayerStateMachine stateMachine)
    {
        this.controller = controller;
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        controller.MoveAnim(Vector3.zero);
    }
    public void Tick()
    {
        
    }
    public void Exit() { }

}
