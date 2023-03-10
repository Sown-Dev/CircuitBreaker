using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHover : MonoBehaviour{
    public float hoverAmt;
    public float hoverFreq;
    public float lerpAmt = 10;
    private bool up;
    private float eTime;
    private Vector3 toPos;
    private Vector3 originalPos;

    private RectTransform rectTransform;
    private void Start(){
        rectTransform = GetComponent<RectTransform> ();
        originalPos = rectTransform.position;
    }

    void Update(){
        eTime += Time.deltaTime;
        if (eTime > hoverFreq){
            eTime = 0;
            up = !up;
        }


        rectTransform.position = originalPos;
    }
}