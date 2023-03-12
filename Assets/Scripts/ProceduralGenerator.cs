using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class ProceduralGenerator : MonoBehaviour{
    public List<GameObject> DeadEnds;
    public List<GameObject> Levels;
    public List<GameObject> Transitions;
    public GameObject end;
    public List<Node> Nodes;

    public int genAmt;

    public LayerMask levels;
    private void Start(){
        Generate();
    }

    private Element e2;
    private Node n2;
    
    void Generate(){
        
        int generated = 0;
        while (OpenNodes() && generated < genAmt){
            foreach (Node n in Nodes){
                if (n.Open){
                    
                    Node.RoomType rt = n.roomtype;
                    Node.NodeEnum rot = n.nodeRot;

                    GameObject GO = GetRandomElement(rt, rot);//Instantiate(GetRandomElement(rt,rot), n.transform.position, n.transform.rotation);
                    Element e = GO.GetComponent<Element>();
                    
                    //DELETE leater
                    n2 = n;
                    e2 = e;

                    //boxcast to check for overlap:
                    Vector3 castPos = n.transform.position + (Vector3) e.bc.offset;
                    RaycastHit2D hit = Physics2D.BoxCast(castPos,
                        e.bc.size, 0, transform.up, 0 , levels);

                    int attempts = 0;
                    while (hit.collider != null ){
                        Debug.Log("attempt" + attempts);
                        GO = GetRandomElement(rt, rot);//Instantiate(GetRandomElement(rt,rot), n.transform.position, n.transform.rotation);
                        e = GO.GetComponent<Element>();
                        //boxcast to check for overlap:
                        castPos = n.transform.position + (Vector3) e.bc.offset;
                        hit = Physics2D.BoxCast(castPos,
                            e.bc.size, 0, transform.up, 0 , levels);
                        attempts++;
                        if (attempts > 100){
                            if (rt == Node.RoomType.Level){
                                rt = Node.RoomType.Transition;
                                while (hit.collider] != null ){
                                    Debug.Log("attempt" + attempts);
                                    GO = GetRandomElement(rt, rot);//Instantiate(GetRandomElement(rt,rot), n.transform.position, n.transform.rotation);
                                    e = GO.GetComponent<Element>();
                                    //boxcast to check for overlap:
                                    castPos = n.transform.position + (Vector3) e.bc.offset;
                                    hit = Physics2D.BoxCast(castPos,
                                        e.bc.size, 0, transform.up, 0 , levels);
                                    attempts++;
                                    if (attempts > 100){
                                        break;
                                    }
                                }
                            } //will attempt to try this loop again with transitions if it is a level
                            
                            //breaks afterwards
                            break;
                        } 
                    }
                
                    //convert to transition otherwise;
                    
                    
                    GameObject instGO= Instantiate(GO, n.transform.position, n.transform.rotation);
                    e = instGO.GetComponent<Element>();
                    n.Open = false;
                    
                    foreach (Node tn in e.Nodes){
                        Nodes.Add(tn);
                    }

                    generated++;
                    break;
                }
            }
        }

        foreach (Node n in Nodes){
            if (n.Open){
                GameObject GO = Instantiate(end, n.transform.position, n.transform.rotation);
                break;
            }
        }
    }

    private void OnDrawGizmos(){
        if(n2)
            Gizmos.DrawCube( n2.transform.position + (Vector3) e2.bc.offset, e2.bc.size);
    }

    GameObject GetRandomElement(Node.RoomType rt, Node.NodeEnum rot){
        List<GameObject> Elements = Levels;
        switch (rt){
            case(Node.RoomType.Level):{
                Elements = Levels ;
                break;
            }
            case(Node.RoomType.DeadEnd):{
                Elements = DeadEnds;
                break;
            }
            case(Node.RoomType.Transition):{
                Elements = Transitions;
                break;
            }
        }
        int rand = Random.Range(0, Elements.Count);
        int attempts = 0; //prevent breaking
        while (Elements[rand].GetComponent<Element>().myRot != rot){
            attempts++;
            rand = Random.Range(0, Elements.Count);
            if(attempts>40)
                break;
        }
        return Elements[rand];
    }

    bool OpenNodes(){
        foreach (Node n in Nodes){
            if (n.Open){
                return true;
            }
        }

        return false;
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.R)){
            Generate();
        }
    }
    
}