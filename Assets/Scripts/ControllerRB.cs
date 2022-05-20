using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerRB : MonoBehaviour
{
    public float speed;
    public float max_speed;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");

        if(rb.velocity.magnitude < max_speed)
        {
            Vector2 movement = new Vector2(horizontal, 0);
            rb.AddForce(movement * speed);
        }
        
    }
}
