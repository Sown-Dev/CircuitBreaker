using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProceduralGenerator : MonoBehaviour{
    public List<GameObject> DeadEnds;
    public List<GameObject> Levels;
    public List<GameObject> Transitions;
    public GameObject end;
    public List<Node> Nodes;

    public int genAmt;

    private void Start(){
        Generate();
    }

    void Generate(){
        int generated = 0;
        while (OpenNodes() && generated < genAmt){
            foreach (Node n in Nodes){
                if (n.Open){
                    n.Open = false;
                    Node.RoomType rt = n.roomtype;
                    Node.NodeEnum rot = n.nodeRot;
                    
                    GameObject GO = Instantiate(GetRandomElement(rt,rot), n.transform.position, n.transform.rotation);
                    Element e = GO.GetComponent<Element>();
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
            }
        }
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