using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldHUD : MonoBehaviour{
    public Image img;
    public RectTransform rt;
    private Player p;
    private void Awake(){
        p = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update(){
        rt.sizeDelta = new Vector2(100 * p.shields, 100);
    }
}
