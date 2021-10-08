using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineHunter : MonoBehaviour
{
    Dictionary<HunterStateEnum, IStateHunter> _allStates = new Dictionary<HunterStateEnum, IStateHunter>();
    IStateHunter _currentState;

    public void OnUpdate()
    {
        if(_currentState != null)
        {
            _currentState.OnUpdate();
        }
    }

    public void changeState(HunterStateEnum id)
    {
        if (_allStates.ContainsKey(id))
        {
            if(_currentState != null)
            {
                _currentState.OnExit();
            }

            _currentState = _allStates[id];
            _currentState.OnStart();
        }
    }

    public void AddState(HunterStateEnum id, IStateHunter state)
    {
        _allStates.Add(id, state);
    }
}
