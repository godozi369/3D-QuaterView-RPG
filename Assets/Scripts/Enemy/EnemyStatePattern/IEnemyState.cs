using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState 
{
    void Enter();
    void Tick();
    void Exit();
}
