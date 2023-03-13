using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour{
    public float rotAmt;
    void Update()
    {
        transform.Rotate(Vector3.forward, rotAmt*Time.deltaTime);
    }
}
