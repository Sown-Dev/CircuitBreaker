using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour{
    [HideInInspector] public bool Open = true;
    public RoomType roomtype;
    public NodeEnum nodeRot;
    
    private SpriteRenderer sr;

    private void Awake(){
        Open = true;
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.color = new Color(0, 0, 0, 0);
    }

    
    
    public enum NodeEnum{
        Right =0,
        Up =1,
        Down = 2,
        Left=3,
    };
    public enum RoomType{
        Level=0,
        DeadEnd=1,
        Transition=2
    };
}
