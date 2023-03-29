
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    public GameObject enemy;
    public bool spawn;

    public float spawnRate = 0.5f;
    public float nextSpawnRate = 0.0f;

    public float IncreaseSpawnRate = 0.0f;

    private Vector2 spawn_location; 

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {


        if(Time.time > nextSpawnRate)
        {
            Spawn_Enemy();
            nextSpawnRate = Time.time + spawnRate;
        }

        
        if( Time.time > IncreaseSpawnRate)
        {
            spawnRate -= 0.1f;
            IncreaseSpawnRate = Time.time + 1.0f;
        }

        
    }

    private void Spawn_Enemy()
    {
    
        spawn_location = new Vector2(transform.position[0], Random.Range(-5.0f,5.0f));
        Instantiate(enemy, spawn_location, transform.rotation);
    }


}
