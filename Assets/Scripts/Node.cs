using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour{
    [HideInInspector] public bool Open = true;
    private SpriteRenderer sr;

    private void Awake(){
        Open = true;
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update(){

        sr.color = Open ? Color.red : Color.black;
    }
}
