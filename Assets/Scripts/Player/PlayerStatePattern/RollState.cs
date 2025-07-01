using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class RollState : IPlayerState
{
    private readonly PlayerController controller;
    private readonly PlayerStateMachine stateMachine;
    private readonly PlayerStat stat;

    public RollState(PlayerController controller, PlayerStateMachine machine, PlayerStat stat)
    {
        this.controller = controller;
        this.stateMachine = machine;
        this.stat = stat;
    }
    public void Enter()
    {
        controller.MoveAnim(Vector3.zero);
        controller.StartCoroutine(RollCoroutine());
    }
    private IEnumerator RollCoroutine()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit))
        {
            stateMachine.ChangeState(new IdleState(controller, stateMachine));
            yield break;
        }

        Vector3 dir = (hit.point - controller.transform.position).normalized;
        dir.y = 0;
        controller.transform.rotation = Quaternion.LookRotation(dir);

        controller.RollAnim();
        yield return new WaitForEndOfFrame(); 

        controller.agent.enabled = false;

        float timer = 0f;
        while (timer < stat.rollDuration)
        {
            controller.transform.position += dir * stat.rollForce * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        controller.agent.enabled = true;

        if (NavMesh.SamplePosition(controller.transform.position, out NavMeshHit navHit, 1f, NavMesh.AllAreas))
        {
            controller.agent.Warp(navHit.position);
        }
        else
        {
            Debug.LogWarning("[RollState] NavMesh 위에 위치할 수 없음");
        }

        stateMachine.ChangeState(new IdleState(controller, stateMachine));
    }
    public void Tick() { }
    public void Exit()
    {
    }
}
