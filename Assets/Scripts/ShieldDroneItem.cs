using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDroneItem : ItemMan{
    public GameObject DronePref;

    private int level;
    
    
    public override void Add(){
        level++;
        Instantiate(DronePref, transform);
    }
}
