using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent : MonoBehaviour{
    public GameObject parent;
    public bool copyTransform = true;
    public bool copyRot = true;
    private Vector3 offset;
    private void Awake(){
        offset = transform.position;
    }

    private void Update(){
        transform.position = parent.transform.position + offset;
        transform.rotation = parent.transform.rotation;
    }
}
