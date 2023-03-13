using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDoor : MonoBehaviour{
    public Animator am;
    
    public Element e;
    private bool open;
    public LayerMask enemyMask;
    private int alternate = 0; // don't want to be boxcasting every frame so we check every 3 frames
    private void FixedUpdate(){
        alternate++;
        if (alternate > 3){
            
            alternate = 0;
            RaycastHit2D hit = Physics2D.BoxCast(e.bc.offset+ (Vector2)e.transform.position, e.bc.size, 0, Vector2.up, 0, enemyMask );
            if (hit.collider == null){
                open = true;
            }
            else{
                open = false;
            }
            am.SetBool("Open",open);
        }
    }
    
}
