using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowKeysMovement : MonoBehaviour{
    public float amt = 5;
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow)){
            transform.position += new Vector3(amt, 0, 0) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow)){
            transform.position += new Vector3(-amt, 0, 0) * Time.deltaTime;
        }
        
        if (Input.GetKey(KeyCode.UpArrow)){
            transform.position += new Vector3(0, amt, 0) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow)){
            transform.position += new Vector3(0, -amt, 0) * Time.deltaTime;
        }
    }
}
