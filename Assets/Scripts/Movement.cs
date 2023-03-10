using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour{
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A)){
            rb.AddForce(transform.right *-20);
        }
        if (Input.GetKey(KeyCode.D)){
            rb.AddForce(transform.right *20);
        }
        if (Input.GetKey(KeyCode.W)){
            rb.AddForce(transform.up *20);
        }
        if (Input.GetKey(KeyCode.S)){
            rb.AddForce(transform.up *-20);
        }
    }
}
