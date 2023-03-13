using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MedStation : Interactable{
    public AudioSource src;
    public override void Interact(){
        src.Play();
        interactable = false;
        player.GetComponent<Player>().shields++;
    }
}
