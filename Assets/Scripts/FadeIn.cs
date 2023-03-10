using System;
using UnityEngine;

public class FadeIn : MonoBehaviour{
    public bool enabled=false;
    private SpriteRenderer rend;
    private Color toBe;
    private Vector3 basePos;
    private void Awake(){
        rend = GetComponent<SpriteRenderer>();
        basePos = transform.position;
    }

    private void Update(){
        rend.color = Color.Lerp(rend.color, toBe, Time.deltaTime * 8);
        transform.position = basePos + new Vector3(0, 0.4f * rend.color.a, 0); //makes button pop up
    }

    public void enable(){
        enabled = true;
        toBe = new Color(1, 1, 1, 1);
    }

    public void disable(){
        enabled = false;
        toBe= new Color(1, 1, 1, 0);
    }
}