using UnityEngine;

public class Interactable : MonoBehaviour{
    [HideInInspector]public GameObject player;
    public FadeIn eKey;
    [HideInInspector]public Material m;
    [HideInInspector]public float toTh;
    [HideInInspector]public bool interactable = true;

    [HideInInspector]public UpgradeScreen us;
    void Awake(){
        us = GameObject.FindGameObjectWithTag("UpgradeScreen").GetComponent<UpgradeScreen>();
        player = GameObject.FindGameObjectWithTag("Player");
        m = gameObject.GetComponent<SpriteRenderer>().material;
        toTh = 0;
        m.SetFloat("_Thickness", 0);
    }

    void Update(){
        //m.SetFloat("_Thickness",Mathf.Lerp(m.GetFloat("_Thickness"), toTh, 5*Time.deltaTime));
        m.SetFloat("_Thickness", toTh);
        if (interactable){
            if (Vector2.Distance(player.transform.position, transform.position) < 2.5f){
                //show E
                eKey.enable();
                toTh = 0.005f;

                if (Input.GetKeyDown("e")){
                    Interact();
                }
            }
            else{
                eKey.disable();
                toTh = 0;
            }
        }
        else{
            eKey.disable();
            toTh = 0;
        }
    }

    public virtual void Interact(){
        //Do nothing
    }
}