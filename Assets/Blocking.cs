using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocking : MonoBehaviour,IDamagable{
    public Player p;


    public void takeDamage(int dmg, Vector3 hitpoint, bool tazer, float stun, int owner){
        dmg = (int)(dmg * 0.6f);
        if (p.blockHealth > dmg){
            p.blockHealth -= dmg;
        }
        else{
            p.takeDamage(dmg,hitpoint,tazer,stun,owner);
        }
    }
}
