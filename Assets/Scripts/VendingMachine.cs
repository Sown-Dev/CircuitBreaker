using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : Interactable{
    public AudioSource src;
    public AudioClip clip;
    public override void Interact(){
        interactable = false;
        src.PlayOneShot(clip,0.8f);
    }
}
