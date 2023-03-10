using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMan : MonoBehaviour{
    //just some stats
    private int deaths;
    
    //important stuff
    private DeathScreen ds;
    public static GameMan GM;
    private void Awake(){
        DontDestroyOnLoad(gameObject);
        GM = this;
        ds = GameObject.FindGameObjectWithTag("DeathScreen").GetComponent<DeathScreen>();
    }

    public void Die(){
        deaths++;
        ds.enabled = true;
        ds.deaths = deaths;

    }

    public void Restart(){
        SceneManager.LoadScene(0);
    }


}