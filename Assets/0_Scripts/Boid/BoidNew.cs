using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidNew : MonoBehaviour
{
    private Vector3 _velocity;
    private Vector3 randomVector;
    private Vector3 steering;
    private Vector3 desired;

    private float randomStartX = 0, randomStartY;
    private int nearAnother = 0;
    private float speedMap = 0;

    [Header("Objectives")]
    [SerializeField] private GameObject _targetHunter = null;
    [SerializeField] private GameObject _targetFood = null;

    [Header("Boid Behaviour")]
    [SerializeField] private float maxSpeed = 0;
    [SerializeField] private float maxForce = 0;
    [SerializeField] private float hunterDistance = 0;

    [Header("Flocking and weights")]
    [SerializeField] private int separationRadius = 0;
    [SerializeField] private int separationCohesion = 0;
    [SerializeField] private int separtionAlign = 0;

    [SerializeField] private float separationWeight = 0;
    [SerializeField] private float cohesionWeight = 0;
    [SerializeField] private float alignWeight = 0;

    [Header("Arrive/Flee and weights")]
    [SerializeField] private float fleeWeight = 0;
    [SerializeField] private float arriveWeight = 0;
    [SerializeField] private float arriveRadius = 0;

    private void Start()
    {
        BoidManager.instance.Addboid(this);

        randomStartX = Random.Range(-1f, 1f);
        randomStartY = Random.Range(-1f, 1f);

        randomVector = new Vector3(randomStartX, 0, randomStartY);
        randomVector.Normalize();
        randomVector *= maxSpeed;

        ApplyForce(randomVector);
    }

    void Update()
    {
        CheckEdge();

        transform.position += _velocity * Time.deltaTime;
        transform.forward = _velocity;

        if ((_targetHunter.transform.position - transform.position).magnitude < hunterDistance)
        {
            ApplyForce(Flee(_targetHunter) * fleeWeight);
        }
        else if ((_targetFood.transform.position - transform.position).magnitude < arriveRadius && (_targetHunter.transform.position - transform.position).magnitude > hunterDistance)
        {
            ApplyForce(Arrive(_targetFood) * arriveWeight);
        }
        else
        {
            ApplyForce(Separation() * separationWeight);
            ApplyForce(Cohesion() * cohesionWeight);
            ApplyForce(Align() * alignWeight);
        }

    }

    //Flocking
    Vector3 Separation()
    {
        desired = Vector3.zero;
        nearAnother = 0;
        foreach (var boid in BoidManager.instance.Boids)
        {
            Vector3 distance = boid.transform.position - transform.position;
            if (boid != this && distance.magnitude < separationRadius)
            {
                desired += distance;
                nearAnother++;
            }
        }

        if (nearAnother == 0)
        {
            return Vector3.zero;
        }

        desired /= nearAnother;
        desired.Normalize();
        desired.y = 0;
        desired = -desired;
        desired *= maxSpeed;

        return SteeringFunc();
    }

    Vector3 Cohesion()
    {
        desired = Vector3.zero;
        nearAnother = 0;
        foreach (var boid in BoidManager.instance.Boids)
        {
            if (boid != this && Vector3.Distance(boid.transform.position, transform.position) < separationCohesion)
            {
                desired.x += boid.transform.position.x;
                desired.z += boid.transform.position.z;
                nearAnother++;
            }
        }
        if (nearAnother == 0)
        {
            return Vector3.zero;
        }
        desired /= nearAnother;
        desired = desired - transform.position;

        desired.Normalize();
        desired.y = 0;
        desired *= maxSpeed;

        return SteeringFunc();
    }

    Vector3 Align()
    {
        desired = Vector3.zero;
        nearAnother = 0;
        foreach (var boid in BoidManager.instance.Boids)
        {
            if (boid != this && Vector3.Distance(boid.transform.position, transform.position) < separtionAlign)
            {
                desired.x += boid._velocity.x;
                desired.z += boid._velocity.z;
                nearAnother++;
            }
        }

        if (nearAnother == 0)
        {
            return Vector3.zero;
        }
        desired /= nearAnother;
        desired.y = 0;
        desired.Normalize();
        desired *= maxSpeed;

        return SteeringFunc();
    }

    //Evade al cazador

    Vector3 Flee(GameObject _tarHunter)
    {
        desired = _tarHunter.transform.position - transform.position;
        desired.Normalize();
        desired.y = 0;
        desired *= maxSpeed;
        desired = -desired;

        return SteeringFunc();
    }

    //Arrive para la comida

    Vector3 Arrive(GameObject _tarFood)
    {
        desired = _tarFood.transform.position - transform.position;

        if (desired.magnitude < arriveRadius)
        {
            speedMap = Map(desired.magnitude, 0, arriveRadius, 0, maxSpeed);
            desired.Normalize();
            desired.y = 0;
            desired *= maxSpeed;
        }
        else
        {
            desired.Normalize();
            desired.y = 0;
            desired *= maxSpeed;
        }

        return SteeringFunc();
    }

    void CheckEdge()
    {
        if (transform.position.z > 16)
            transform.position = new Vector3(transform.position.x, transform.position.y, -16);
        if (transform.position.z < -16)
            transform.position = new Vector3(transform.position.x, transform.position.y, 16);

        if (transform.position.x > 27)
            transform.position = new Vector3(-27, transform.position.y, transform.position.z);
        if (transform.position.x < -27)
            transform.position = new Vector3(27, transform.position.y, transform.position.z);
    }

    Vector3 SteeringFunc()
    {
        steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);

        return steering;
    }

    void ApplyForce(Vector3 Force)
    {
        _velocity += Force;
        _velocity = Vector3.ClampMagnitude(_velocity, maxSpeed);
    }

    public Vector3 GetVelocity()
    {
        return _velocity;
    }

    float Map(float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        return (from - toMin) / (fromMax - fromMin) * (toMax - toMin) + fromMin;
    }
}
