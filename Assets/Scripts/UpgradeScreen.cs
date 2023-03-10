
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class UpgradeScreen : MonoBehaviour{
    public UpgradeOption up1;
    public UpgradeOption up2;
    public UpgradeOption up3;
    public UpgradeDescription[] descs;
    
    
    public CanvasGroup cg;
    private float toA;
    private Player p;
    
    private void Awake(){
        cg.alpha = 0;
        p = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        //GetUpgrade();
    }

    private void Update(){
        cg.interactable = toA> 0.1f;
        cg.blocksRaycasts = (toA> 0.1f);
        cg.alpha = Mathf.Lerp(cg.alpha, toA, 10*Time.deltaTime);
    }

    public void GetUpgrade(){
        toA = 1;
        
        //eventually want to make sure random numbers dont repeat
        
        int rand1 = Random.Range(0, descs.Length);
        up1.Set(descs[rand1]);
        
        int rand2 = Random.Range(0, descs.Length);
        up2.Set(descs[rand2]);
        
        int rand3 = Random.Range(0, descs.Length);
        up3.Set(descs[rand3]);

    }

    public void Pick(int id){
        p.AddItem(id);
        toA = 0;
    }
}
