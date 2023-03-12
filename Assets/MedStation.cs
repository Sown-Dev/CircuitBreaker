using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MedStation : Interactable{
    public override void Interact(){
        interactable = false;
        player.GetComponent<Player>().shields++;
    }
}
