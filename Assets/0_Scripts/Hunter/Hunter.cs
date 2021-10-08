using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : UnitHunter
{
    private StateMachineHunter _fsm;
    [Header("Objectives and radius")]
    public int detectionRadius;
    public GameObject target;
    public GameObject futurePosObject;
    public bool targetLive = false;

    void Start()
    {
        _fsm = GetComponent<StateMachineHunter>();
        _fsm.AddState(HunterStateEnum.Rest, new RestState(_fsm, this));
        _fsm.AddState(HunterStateEnum.Patrol, new PatrolState(_fsm,this));
        _fsm.AddState(HunterStateEnum.Hunting, new HuntingState(_fsm, this));
        _fsm.changeState(HunterStateEnum.Patrol);
    }

    void Update()
    {
        _fsm.OnUpdate();
    }

    public void CheckBoid()
    {
        foreach (BoidNew boid in BoidManager.instance.Boids)
        {
            float dist = Vector3.Distance(boid.transform.position, transform.position);

            if ((target == null || boid.gameObject != target))
            {
                if (dist <= detectionRadius)
                {
                    targetLive = true;
                    target = boid.gameObject;
                    futurePosObject = boid.transform.GetChild(1).gameObject;

                }
            }
            else if (dist >= detectionRadius)
            {
                targetLive = false;
                target = null;
                futurePosObject = null;
            }
        }
    }

    public void PatrolWayPoints()
    {
        if(target == false)
        {
            _dir = allWaypoints[_currentWaypoints].transform.position -transform.position;
            transform.forward = _dir;
            transform.position += transform.forward * maxSpeed * Time.deltaTime;

            if (_dir.magnitude < 0.3f)
            {
                _currentWaypoints++;

                if (_currentWaypoints > allWaypoints.Count-1)
                {
                    _currentWaypoints = 0;
                    allWaypoints.Reverse();
                }
            }
        }
    }


    public void Hunting()
    {
        if(targetLive == true)
        {
            transform.position += _velocity * Time.deltaTime;
            transform.forward = _velocity.normalized;

            var tar = target.gameObject.GetComponent<BoidNew>();
            futurePos = target.transform.position + tar.GetVelocity() * futureTime * Time.deltaTime;
            desired = futurePos - transform.position;
            desired.Normalize();
            desired *= maxSpeed;

            steering = desired - _velocity;
            steering = Vector3.ClampMagnitude(steering, maxForce);
            futurePosObject.transform.position = futurePos;
            ApplyForce(steering);
        }

    }

    public void ApplyForce(Vector3 Force)
    {
        _velocity += Force;
        _velocity = Vector3.ClampMagnitude(_velocity, maxSpeed);
    }

}

public enum HunterStateEnum
{
    Rest,
    Hunting,
    Patrol,
}
