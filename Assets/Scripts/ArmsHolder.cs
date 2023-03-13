using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public class ArmsHolder : MonoBehaviour{
    public bool trackMouse;
    public bool trackPhys;
    public Player p;
    public Rigidbody2D rb;
    private void Update(){
        
        if (trackMouse){
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Vector3 mouseDir = (mousePos - transform.position).normalized;
            mouseDir.z = 0; //just to be sure
            transform.right = Vector3.Lerp(mouseDir,Vector2.right, 0.3f );
            transform.right *= p.transform.localScale.x;
        }else if (trackPhys){
            Vector3 toDir = rb.velocity.normalized;
            transform.right = Vector3.Lerp(toDir,Vector2.right, 0.7f );
        }
        else{
            transform.right = Vector2.right;
        }

        
    }

    
}
