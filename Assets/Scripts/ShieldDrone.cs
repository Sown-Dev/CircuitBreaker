using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDrone : MonoBehaviour{
    public float rotRate = 20f;
    void Update()
    {
        transform.Rotate(Vector3.forward, rotRate*Time.deltaTime);
    }
}
