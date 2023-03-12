using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour{

    public float pMult = 0.08f;
    // Update is called once per frame
    void Update(){
        Vector3 mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition) * pMult;
        transform.position = new Vector3(mpos.x, mpos.y, 10) ;

    }
}
