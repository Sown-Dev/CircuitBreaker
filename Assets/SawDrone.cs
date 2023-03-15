using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawDrone : MonoBehaviour{
    private Rigidbody2D rb;

    private void Awake(){
        rb = gameObject.GetComponent<Rigidbody2D>();
    } 

    void FixedUpdate()
    {
        Stabilization();
    }

    void Stabilization(){
        //rotation stabilization
        if (Mathf.Abs(rb.rotation) > 20){
            rb.AddTorque( ((rb.rotation>0) ? -1:1)*80f*Time.deltaTime );
            rb.AddTorque( ((rb.rotation>0) ? -1:1)*0.8f*Time.deltaTime*Mathf.Abs(rb.rotation) );
        }
        
        //hover
        if (rb.velocity.y < -0.2f){
            rb.AddForce(transform.up*900f*Time.deltaTime);
            rb.AddForce(Vector2.up*100f*Time.deltaTime);
        }

        float ud = 1.2f;
        float amt = 2000f;
        if (Physics2D.Raycast(transform.position,
                Vector2.down, ud).collider != null){
            rb.AddForce(Vector2.up * amt * Time.deltaTime);
            Debug.Log("psjgslkdjg");
        }

        if ( Physics2D.Raycast(transform.position,
                Vector2.up, ud).collider !=null)
            rb.AddForce(Vector2.down*amt*Time.deltaTime);
        if ( Physics2D.Raycast(transform.position,
                Vector2.left, ud).collider !=null)
            rb.AddForce(Vector2.right*amt*Time.deltaTime);
        if ( Physics2D.Raycast(transform.position,
                Vector2.right, ud).collider !=null)
            rb.AddForce(Vector2.left*amt*Time.deltaTime);
        
    }

    private void OnDrawGizmos(){
        float ud = 1.5f;
        Gizmos.DrawLine(transform.position, (Vector2) transform.position+(Vector2.left*ud));
        Gizmos.DrawLine(transform.position, (Vector2) transform.position+(Vector2.down*ud));
        if (Physics2D.Raycast(transform.position,
                Vector2.right, ud).collider != null)
            Gizmos.DrawLine(transform.position, (Physics2D.Raycast(transform.position,
                Vector2.right, ud).transform.position));

    }
}
