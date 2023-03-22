using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DeathScreen : MonoBehaviour{
   public bool enabled=false;
   private CanvasGroup cg;
   private float toA;
   public int deaths;

   private void Awake(){
      cg = GetComponent<CanvasGroup>();
      cg.alpha = 0;
   }

   private void Update(){
      toA= enabled ? 1 : 0;
      cg.alpha = Mathf.Lerp(cg.alpha, toA, 10 * Time.deltaTime);
      cg.interactable = enabled;
      cg.blocksRaycasts = !enabled;
      
      if (enabled){
         if (Input.GetKeyDown(KeyCode.R)){
            enabled = false;
            GameMan.GM.Start();
            
         }
      }
   }
}
