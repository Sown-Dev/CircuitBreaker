using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent : MonoBehaviour{
    public GameObject parent;
    public bool copyTransform = true;
    public bool copyRot = true;
    private Vector3 offset;
   //public Vector3 offset2;
    private void Awake(){
        offset = transform.localPosition;
    }

    private void Update(){
        if (copyTransform)
            transform.position = parent.transform.position + offset;
        if(copyRot)
            transform.rotation = parent.transform.rotation;
    }
}
