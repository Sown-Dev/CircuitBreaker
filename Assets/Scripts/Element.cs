using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour{
    public List<Node> Nodes;

    private void OnTriggerEnter2D(Collider2D other){
        Destroy(gameObject);
    }
}
