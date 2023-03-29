using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{

    public float velocity;
    public Rigidbody2D rb2D;

    
    // Start is called before the first frame update
    void Start()
    {
        rb2D = transform.GetComponent<Rigidbody2D>();
        velocity = 1;
        rb2D.gravityScale = 0f;
        //transform.eulerAngles = new Vector2(0,90);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position[0] < -12 )
        {
            Destroy(transform.gameObject);
        }
    }
    
    void FixedUpdate()
    {
        rb2D.transform.Translate(new Vector2(-1,0)  * velocity);
    }



}
