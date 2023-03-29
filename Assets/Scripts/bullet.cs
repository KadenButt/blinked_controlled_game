using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    
    public Rigidbody2D rb;
    public Transform player;

    private float horizontal_speed= 4f;
    private Vector2 dir;
    private double angle_to_player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player").transform;
        dir = (player.position - transform.position);
        rb.velocity = dir.normalized * horizontal_speed;
 
        
    }

    // Update is called once per frame
    void Update()
    {        
        //if(transform.position.y > 5|| transform.position.x > -10.3){Destroy(gameObject);}
    }

    void FixedUpdate()
    {
    }
}
