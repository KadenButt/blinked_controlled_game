using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{

    public float velocity;
    public Rigidbody2D rb;

    
    // Start is called before the first frame update
    void Start()
    {
        //sets the velocity, makes it experence no gravity.
        rb = transform.GetComponent<Rigidbody2D>();
        velocity = 1;
        rb.gravityScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //destroys the object if it goes out of bounds
        if(transform.position[0] < -12 )
        {
            Destroy(transform.gameObject);
        }
    }
    
    void FixedUpdate()
    {
        //Controls the velocity
        rb.transform.Translate(new Vector2(-1,0)  * velocity);
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.tag == "player")
        {
            Destroy(transform.gameObject);
        }
    }




}
