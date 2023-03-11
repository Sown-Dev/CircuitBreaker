using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Bullet : MonoBehaviour{
    private int owner = 0; //0 = player, 1 enemey
    public Rigidbody2D rb;
    private void Start(){
        rb.AddForce(transform.right * 180 );
        Destroy(gameObject,8f);
        owner = 1;
    }

    private void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.GetComponent<IDamagable>() != null){ //damage is done regardless
            col.gameObject.GetComponent<IDamagable>().takeDamage(50, transform.position, false, 0, 0);
        }
        
        Debug.Log(col.gameObject.layer);
        if (col.gameObject.layer == 13){
            
            transform.right = col.contacts[0].normal;
            rb.velocity = Vector2.zero;
            rb.AddForce(transform.right * 180 );
            owner = 0;
        }
        else{

            Destroy(gameObject);
            
        }
    }
    
}

