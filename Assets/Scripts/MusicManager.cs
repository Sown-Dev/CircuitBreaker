using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    //important stuff
    public AudioClip menu;
    public AudioClip game;
    public AudioSource src;
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

    private int prevScene = -1;
    void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        if (scene.buildIndex != prevScene){
            PlayMusic(scene.buildIndex);
        }

        prevScene = scene.buildIndex;
    }

    void PlayMusic(int scene){
        if (scene == 0){
            src.clip = menu;
            src.Play();
        }
        if (scene == 1){
            src.clip = game;
            src.Play();
        }
    }

}
