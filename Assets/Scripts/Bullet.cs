using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Bullet : MonoBehaviour{
    public Rigidbody2D rb;

    private void Start(){
        rb.AddForce(transform.right * 180 );
        Destroy(gameObject,8f);
    }

    private void OnCollisionEnter2D(Collision2D col){
        Destroy(gameObject);
        if (col.gameObject.GetComponent<IDamagable>() != null){
            col.gameObject.GetComponent<IDamagable>().takeDamage(50, transform.position);
        }
    }
}

