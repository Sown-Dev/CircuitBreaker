using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Element : MonoBehaviour{
    public List<Node> Nodes;
    public GameObject myObj;
    public Node.NodeEnum myRot;
    
    private void OnTriggerEnter2D(Collider2D other){
        Destroy(gameObject);
    }
}
