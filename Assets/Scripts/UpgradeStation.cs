using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeStation : Interactable{
    private bool used = false;

    public Animator am;

    public override void Interact(){
        used = true;
        interactable = false;
        us.GetUpgrade();
        am.SetTrigger("Open");
    }
}
