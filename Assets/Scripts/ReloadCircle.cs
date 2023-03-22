using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadCircle : MonoBehaviour{
    private Image img;
    public float fillAmt = 0;
    private void Awake(){
        img = gameObject.GetComponent<Image>();
    }

    void Update(){
        
        img.fillAmount = fillAmt;
        transform.position =  Input.mousePosition;
    }
}
