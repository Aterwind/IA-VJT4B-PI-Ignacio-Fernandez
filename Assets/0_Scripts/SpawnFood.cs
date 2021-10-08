using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFood : MonoBehaviour
{
    public SpawnFood spawnFood;
    public int stock = 0;
    private int limiteList;
    private int _RandomListFood;
    private int _MaxRandomListEnemy;

    [SerializeField] private float waveRate = 2;
    private float nextWaveTime = 2;

    public List<Food> food = new List<Food>();
    public List<GameObject> spawnList = new List<GameObject>();
    public ObjectPool<Food> pool;

    void Start()
    {
        limiteList = food.Count;
        _MaxRandomListEnemy = spawnList.Count;

        pool = new ObjectPool<Food>(FoodReturn, food[_RandomListFood].TurnOn, food[_RandomListFood].TurnOff, stock);
    }

    void Update()
    {
        if (Time.time >= nextWaveTime)
        {
            Spawn();
            nextWaveTime = Time.time + 1 / waveRate * 30;
        }
    }

    void Spawn()
    {
        int spawnRandomList = Random.Range(0, _MaxRandomListEnemy);

        food[_RandomListFood].transform.position = spawnList[spawnRandomList].transform.position;
        food[_RandomListFood].transform.forward = spawnList[spawnRandomList].transform.forward;
        food[_RandomListFood].BackStock = pool.ReturnObject;
    }

    public Food FoodReturn()
    {
        _RandomListFood = Random.Range(0, limiteList);
        return Instantiate(food[_RandomListFood]);
    }
}
