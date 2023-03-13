using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    //important stuff
    public static MusicManager MM=null;
    private void Awake(){
        DontDestroyOnLoad(gameObject);
        
        if (MM == null){
            MM = this;
            
        }
        else{
            Destroy(gameObject);
        }
        
        
    }
    void OnEnable(){
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        
    }

}
