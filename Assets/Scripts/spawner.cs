
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    public GameObject enemy;
    public player script; 

    float spawnRate = 5f;
    public float nextSpawnRate = 0.0f;

    public float IncreaseSpawnRate = 0.0f;

    private Vector2 spawnLocation; 

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        //spawns enemy after given time
        if(Time.time > nextSpawnRate && script.isGameRunning == true)
        {
            Spawn_Enemy();
            nextSpawnRate = Time.time + spawnRate;
        }

        //Increases the rate of the enemy being spawned in
        if( Time.time > IncreaseSpawnRate && script.isGameRunning == true)
        {
            if(spawnRate > 0.5f){spawnRate -= 0.5f;}
            
            IncreaseSpawnRate = Time.time + 2.0f;
        }

        
    }

    //function that spawn an enemy at  a random y
    private void Spawn_Enemy()
    {
    
        spawnLocation = new Vector2(transform.position[0], Random.Range(-5.0f,5.0f));
        Instantiate(enemy, spawnLocation, transform.rotation);
    }


}
