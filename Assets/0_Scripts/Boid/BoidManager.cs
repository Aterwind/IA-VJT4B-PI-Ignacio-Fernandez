using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public static BoidManager instance;

    public List<BoidNew> Boids = new List<BoidNew>();
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

    public void Addboid(BoidNew b)
    {
        if (Boids.Contains(b))
            return;
        Boids.Add(b);
    }
}
