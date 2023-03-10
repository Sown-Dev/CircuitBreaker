using System;
using UnityEngine;

public class AttackCollider : MonoBehaviour{
    public Player pl;
    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.GetComponent<IDamagable>() !=null){
            col.gameObject.GetComponent<Rigidbody2D>().AddForce(200*(col.gameObject.transform.position - transform.position));
            col.gameObject.GetComponent<IDamagable>().takeDamage(pl.damage, col.ClosestPoint(transform.position));
        }
    }

   
}