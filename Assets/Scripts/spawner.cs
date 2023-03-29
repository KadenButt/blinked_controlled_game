
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    public GameObject enemy;
    public bool spawn;

    private Vector2 spawn_location; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(spawn)
        {
            Spawn_Enemy();
        }


    }

    private void Spawn_Enemy()
    {
    
        spawn_location = new Vector2(transform.position[0], Random.Range(-5.0f,5.0f));
        Instantiate(enemy, spawn_location, transform.rotation);
    }


}
