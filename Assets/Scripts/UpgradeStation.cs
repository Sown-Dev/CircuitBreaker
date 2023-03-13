using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeStation : Interactable{
    private bool used = false;

    public Animator am;

    public AudioSource src;
    public override void Interact(){
        src.Play();
        used = true;
        interactable = false;
        us.GetUpgrade();
        am.SetTrigger("Open");
    }
}
