using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeToBlack : MonoBehaviour{
    public static FadeToBlack fb;
    public CanvasGroup cg;
    private void Awake(){
        fb = this;
    }

    public float toA = 0;
    private void Update(){
        cg.alpha = Mathf.Lerp(cg.alpha, toA, 3 * Time.deltaTime);
    }
}
