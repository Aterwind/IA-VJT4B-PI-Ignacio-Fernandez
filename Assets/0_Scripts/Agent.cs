using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private Vector3 _velocity;
    private float speed = 0;
    [SerializeField] private GameObject _target = null, _target2 = null;
    [SerializeField] private int maxSpeed = 0;
    [SerializeField] private float maxForce = 0;
    [SerializeField] private float arriveRadius = 0;

    [SerializeField] private float SeekWeight = 0; //Dar fuerza, haci conseiguie una aceleracion
    [SerializeField] private float FleeWeight = 0;
    //[SerializeField] private float ArriveWeight = 0;

    private void Update()
    {
        //Arrive(_target);

        //if((Vector3.Distance(transform.position, _target.transform.position) < 3) es lo mismo pero de otra manera
        if ((_target.transform.position - transform.position).magnitude < 3)
        {
            ApplyForce(Flee(_target) * FleeWeight);
        }
        else
        {
            ApplyForce(Seek(_target2) * SeekWeight);
        }

        transform.position += _velocity * Time.deltaTime;
        transform.forward = _velocity;
    }

    Vector3 Seek(GameObject _tar) // o void Seek(GameObject _tar)
    {
        Vector3 desired = _tar.transform.position - transform.position;
        desired.Normalize();
        desired *= maxSpeed;

        Vector3 sterring = desired - _velocity;
        sterring = Vector3.ClampMagnitude(sterring, maxForce);

        return sterring; // o ApplyForce(sterring);
        //A comparcion de del otro, directamente pasas un sterring como vector al Applyforce()
    }

    Vector3 Arrive(GameObject _tar) //Los mismo que Seek, pero este pregunte si esta dentro de su magnitud para desacelerar
    {                            // o void Seek(GameObject _tar)
        Vector3 desired = _tar.transform.position - transform.position;

        if(desired.magnitude < arriveRadius)
        {
            //speed = maxSpeed * (desired.magnitude / arriveRadius); //Comparte la mismo funcion, pero la de abajo un mapa de "Noise"
            speed = Map(desired.magnitude, 0, arriveRadius, 0, maxSpeed);
            desired.Normalize();
            desired *= maxSpeed;
        }
        else
        {
            desired.Normalize();
            desired *= maxSpeed;
        }

        Vector3 sterring = desired - _velocity;
        sterring = Vector3.ClampMagnitude(sterring, maxForce);

        return sterring; // o ApplyForce(sterring);
    }

    Vector3 Flee(GameObject _tar)
    {
        Vector3 desired = _tar.transform.position - transform.position;
        desired.Normalize();
        desired *= maxSpeed;
        desired = -desired;

        Vector3 sterring = desired - _velocity;
        sterring = Vector3.ClampMagnitude(sterring, maxForce);

        return sterring;
    }

    void ApplyForce(Vector3 Force)
    {
        _velocity += Force;
    }

    float Map(float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        return (from - toMin) / (fromMax - fromMin) * (toMax - toMin) + fromMin;
    }
}
