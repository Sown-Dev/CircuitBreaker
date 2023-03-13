using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class White : MonoBehaviour{
    public float whiteAMT;
    private Material m;
    public void Awake(){
        m = gameObject.GetComponent<SpriteRenderer>().material;
    }

    private void Update(){
        m.SetFloat("_Instensity", whiteAMT);
    }
}
