using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntingState : IStateHunter
{
    StateMachineHunter _fsm;
    Hunter _hunter;

    public HuntingState(StateMachineHunter fStateMachine, Hunter h)
    {
        _fsm = fStateMachine;
        _hunter = h;
    }

    public void OnStart()
    {
    }

    public void OnUpdate()
    {
        Debug.Log("Estoy en h");
        _hunter.CheckBoid();
        _hunter.Hunting();

        if(_hunter.targetLive == false)
        {
            _fsm.changeState(HunterStateEnum.Patrol);
        }
    }

    public void OnExit()
    {
    }
}
