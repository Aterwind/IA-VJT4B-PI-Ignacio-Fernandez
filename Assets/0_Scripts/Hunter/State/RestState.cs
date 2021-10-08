using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestState : IStateHunter
{
    StateMachineHunter _fsm;
    Hunter _hunter;

    public RestState(StateMachineHunter fStateMachine, Hunter h)
    {
        _fsm = fStateMachine;
        _hunter = h;
    }

    public void OnStart()
    {
        Debug.Log("Entre a Rest");
    }

    public void OnUpdate()
    {
        Debug.Log("Estoy en Rest");
    }

    public void OnExit()
    {
        Debug.Log("Sali de Rest");
    }

}
