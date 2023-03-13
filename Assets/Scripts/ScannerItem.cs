using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerItem : ItemMan{
    public int amount;

    public LayerMask enemies;
    public AudioSource asrc;
    public AudioClip scan;
    public override void Add(){
        tElapsed = 0;
        amount++;
    }

    private float maxTime = 5;
    private float tElapsed;
    private void Update(){
        maxTime = (float)(5f * Math.Pow(1,(0.8*amount)));
        if (amount > 0){
            tElapsed += Time.deltaTime;
            if (tElapsed > maxTime){
                tElapsed = 0;
                Scan();
            }
        }
    }

    void Scan(){
        asrc.PlayOneShot(scan, 0.4f);
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(19, 11),0,  transform.up,1000, enemies.value);
        if (hit.collider != null){
            if(!hit.collider.gameObject.GetComponent<EnemyClass>().marked)
                hit.collider.gameObject.GetComponent<EnemyClass>().mark();
        }
    }
}
