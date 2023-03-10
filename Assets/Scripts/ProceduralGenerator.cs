using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProceduralGenerator : MonoBehaviour{
    public List<GameObject> Elements;
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
                    GameObject GO = Instantiate(GetRandomElement(), n.transform.position, n.transform.rotation);
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

    GameObject GetRandomElement(){
        int rand = Random.Range(0, Elements.Count);

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