using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreen : MonoBehaviour{
    public bool show;
    private CanvasGroup cg;
    private float toA;
    private void Awake(){
        cg = GetComponent<CanvasGroup>();
        cg.alpha = 0;
    }

    private void Update(){
        toA= show ? 1 : 0;
        cg.alpha = Mathf.Lerp(cg.alpha, toA, 10 * Time.deltaTime);
        cg.interactable = show;
        cg.blocksRaycasts = show;
      
        if (show){
            if (Input.GetKeyDown(KeyCode.Space)){
                show = false;
                GameMan.GM.Win();
            
            }
        }
    }
}
