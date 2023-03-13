using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour{
    private float toA=0;
    public SpriteRenderer sr;

    private void Start(){
        if (GameMan.GM.deaths <= 0){
            toA = 1;
        }
        sr.color = new Color(1, 1, 1, toA);
    }

    private void Update(){
        sr.color = new Color(1, 1, 1, Mathf.Lerp(sr.color.a, toA , 3*Time.deltaTime));
    }

    private void OnTriggerExit2D(Collider2D other){
        toA = 0;
    }
}
