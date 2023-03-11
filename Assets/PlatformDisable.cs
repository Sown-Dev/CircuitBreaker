using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDisable : MonoBehaviour{
    private Player p;
    public PlatformEffector2D pe;
    private void Awake(){
        p = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update(){
        pe.rotationalOffset = p.platforms ? 180 : 0;
    }
}
