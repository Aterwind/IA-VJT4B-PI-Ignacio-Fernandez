using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateHunter
{
    void OnStart();
    void OnUpdate();
    void OnExit();
}
