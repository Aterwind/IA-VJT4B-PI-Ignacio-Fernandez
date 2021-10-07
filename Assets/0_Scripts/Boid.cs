using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    private Vector3 _velocity;
    private Vector3 randomVector;
    private Vector3 steering;
    private Vector3 desired;

    private float randomStartX = 0, randomStartY;
    private int nearAnother = 0;

    [SerializeField] private float maxSpeed = 0;
    [SerializeField] private float maxForce = 0;
    [SerializeField] private int separationRadius = 0;
    [SerializeField] private int separationCohesion = 0;
    [SerializeField] private int separtionAlign = 0;

    [SerializeField] private float separationWeight=0;
    [SerializeField] private float cohesionWeight=0;
    [SerializeField] private float alignWeight=0;
    [SerializeField] private float fleeWeight = 0;


    [SerializeField] private GameObject _target = null;
    
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

        if ((_target.transform.position - transform.position).magnitude < 4)
        {
            ApplyForce(Flee(_target) * fleeWeight);
        }
        else
        {
            ApplyForce(Separation() * separationWeight);
            ApplyForce(Cohesion() * cohesionWeight);
            ApplyForce(Align() * alignWeight);
        }

        transform.position += _velocity * Time.deltaTime;
        transform.forward = _velocity.normalized;
    }

    //Flocking
    Vector3 Separation()
    {
        desired = Vector3.zero;
        nearAnother = 0;
        foreach (var boid in BoidManager.instance.Boids)
        {
            Vector3 distance = boid.transform.position - transform.position;
            if(boid != this && distance.magnitude < separationRadius)
            {
                desired += distance;
                nearAnother++;
            }
        }

        if(nearAnother == 0)
        {
            return Vector3.zero;
        }

        desired /= nearAnother;
        desired.Normalize();
        desired.y = 0;
        desired = -desired;
        desired *= maxSpeed;

        return SteeringFun();
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
        desired.y=0;
        desired *= maxSpeed;

        return SteeringFun();
    }

    Vector3 Align()
    {
        desired = Vector3.zero;
        nearAnother = 0;
        foreach (var boid in BoidManager.instance.Boids)
        {
            if(boid !=this && Vector3.Distance(boid.transform.position, transform.position) < separtionAlign)
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

        return SteeringFun();
    }
    //

    //Evade

    Vector3 Flee(GameObject _tar)
    {
        desired = _tar.transform.position - transform.position;
        desired.Normalize();
        desired.y = 0;
        desired *= maxSpeed;
        desired = -desired;

        return SteeringFun();
    }

   
    void CheckEdge()
    {
        if(transform.position.z > 16)
            transform.position = new Vector3(transform.position.x, transform.position.y, -16);
        if(transform.position.z < -16)
            transform.position = new Vector3(transform.position.x, transform.position.y, 16);

        if(transform.position.x > 27)
            transform.position = new Vector3(-27, transform.position.y, transform.position.z);
        if (transform.position.x < -27)
            transform.position = new Vector3(27, transform.position.y, transform.position.z);
    }

    Vector3 SteeringFun()
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
}
