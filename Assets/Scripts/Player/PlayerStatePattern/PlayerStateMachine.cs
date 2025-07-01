using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public IPlayerState CurrentState { get; private set; }

    public void ChangeState(IPlayerState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }
    private void Update()
    {
        CurrentState?.Tick();
    }
}
