using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Food : MonoBehaviour
{
    public Action<Food> BackStock;
    Vector3 dir;
    void Update()
    {
        for (int i = 0; i < BoidManager.instance.Boids.Count; i++)
        {
            dir = BoidManager.instance.Boids[i].transform.position - transform.position;

            if (dir.magnitude < 0.3)
            {
                Reset();
                BackStock.Invoke(this);
            }
               
        }
    }

    public void Reset()
    {
        transform.position = new Vector3(40, 0, 10);
    }

    public void TurnOff(Food b)
    {
        Reset();
    }
    public void TurnOn(Food b)
    {
        Reset();
    }
}
