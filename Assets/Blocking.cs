using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocking : MonoBehaviour,IDamagable{
    private Player p;


    public void takeDamage(int dmg, Vector3 hitpoint, bool tazer, float stun, int owner){
        p.blockHealth -= dmg;
    }
}
