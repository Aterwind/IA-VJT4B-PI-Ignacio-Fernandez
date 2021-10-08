using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBoid : MonoBehaviour
{
    [SerializeField] private GameObject _targetHunter = null;
    [SerializeField] private float magDestroy = 0;
    void Update()
    {
        if ((_targetHunter.transform.position - transform.position).magnitude < magDestroy)
        {
            this.gameObject.SetActive(false);
            transform.position = new Vector3(40, 0, 10);
        }
    }
}
