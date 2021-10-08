using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IStateHunter
{
    StateMachineHunter _fsm;
    Hunter _hunter;

    public PatrolState(StateMachineHunter fStateMachine, Hunter h)
    {
        _fsm = fStateMachine;
        _hunter = h;
    }

    public void OnStart()
    {
    }

    public void OnUpdate()
    {
        Debug.Log("Estoy en Patrol");
        _hunter.CheckBoid();
        _hunter.PatrolWayPoints();

        if (_hunter.target == true)
        {
            _fsm.changeState(HunterStateEnum.Hunting);
        }
    }

    public void OnExit()
    {

    }
}
