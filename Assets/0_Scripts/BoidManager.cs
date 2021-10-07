using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public static BoidManager instance;

    public List<Boid> Boids = new List<Boid>();
    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }


    public void Addboid(Boid b)
    {
        if (Boids.Contains(b))
            return;
        Boids.Add(b);
    }
}
