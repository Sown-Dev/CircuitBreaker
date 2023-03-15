using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour{
    public float hoverAmt;
    public float hoverFreq;
    public float lerpAmt = 10;
    private bool up;
    private float eTime;
    private Vector3 toPos;
    private Vector3 originalPos;
    public float offset;

    private void Start(){
        originalPos = transform.localPosition;
    }

    void Update(){
        eTime += Time.deltaTime;
        if (eTime > hoverFreq){
            eTime = 0;
            up = !up;
        }

        toPos = new Vector3(originalPos.x, originalPos.y + (up ? hoverAmt : 0) +offset, originalPos.z);

        transform.localPosition = Vector3.Lerp(transform.localPosition, toPos, lerpAmt * Time.deltaTime);
    }
}