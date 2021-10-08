using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitHunter : MonoBehaviour
{
    [Header("Hunter Behaviour")]
    [SerializeField] protected float maxSpeed = 0;
    [SerializeField] protected float maxForce = 0;
    [SerializeField] protected float futureTime = 0;

    [Header(" List waypoints")]
    [SerializeField] protected List<GameObject> allWaypoints = new List<GameObject>();
    protected Vector3 _dir;
    protected Vector3 futurePos;
    protected int _currentWaypoints = 0;

    protected Vector3 _velocity;
    protected Vector3 steering;
    protected Vector3 desired;
}
