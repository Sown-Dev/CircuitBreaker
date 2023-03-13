using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Bullet : MonoBehaviour{
    public GameObject shrapnel;
    private int owner = 0; //0 = player, 1 enemey
    public Rigidbody2D rb;
    private void Start(){
        rb.AddForce(transform.right * 180 );
        Destroy(gameObject,8f);
        owner = 1;
    }

    private void OnCollisionEnter2D(Collision2D col){
        
        
        if (col.gameObject.layer == 13){
            if (col.gameObject.GetComponent<IDamagable>() != null){ //damage is done regardless
                col.gameObject.GetComponent<IDamagable>().takeDamage(50, transform.position, false, 0.3f, owner);
            }
            
            transform.right = col.contacts[0].normal;
            rb.velocity = Vector2.zero;
            rb.AddForce(transform.right * 180 );
            owner = -1;
        }
        else{
            if (col.gameObject.GetComponent<IDamagable>() != null){ 
                col.gameObject.GetComponent<IDamagable>().takeDamage(50, transform.position, false, 0.3f, owner);
            }

            Instantiate(shrapnel, transform.position, Quaternion.Euler(col.contacts[0].normal));
            Destroy(gameObject);
            
        }
    }
    
}

