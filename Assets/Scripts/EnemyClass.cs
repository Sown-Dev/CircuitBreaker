using System;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class EnemyClass : MonoBehaviour{
    public GameObject marker;
    
    public Image hpbar;
    
    public float Health=0;
    public  float maxHealth = 160;
    
    public bool marked;
    public void mark(){
        marked = true;
        marker.SetActive(true);
    }

    private void Update(){
        hpbar.enabled = Health > maxHealth;
        hpbar.fillAmount = Health / maxHealth;
    }
}