using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameMan : MonoBehaviour{
    //just some stats
    public int deaths;
    
    //important stuff
    private DeathScreen ds;
    public static GameMan GM=null;
    
    private void Awake(){
        ds = GameObject.FindGameObjectWithTag("DeathScreen").GetComponent<DeathScreen>();
        DontDestroyOnLoad(gameObject);
        
        if (GM == null){
            GM = this;
            
        }
        else{
            Destroy(gameObject);
        }
        
        
    }

    void OnEnable(){
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        ds = GameObject.FindGameObjectWithTag("DeathScreen").GetComponent<DeathScreen>();
        pausemenucg.alpha = 0;
        settingscg.alpha = 0;
    }

    public bool paused;
    public bool settingsOn;
    private float toA;
    public CanvasGroup pausemenucg;
    public CanvasGroup settingscg;
    private void Update(){
        if (Input.GetKeyDown(KeyCode.Escape)){
            paused = !paused;
            Time.timeScale = paused? 0:1;
            if (!paused)
                settingsOn = false;
        }
        
        toA= paused ? 1 : 0;
        pausemenucg.alpha = Mathf.Lerp(pausemenucg.alpha, toA, 8 * Time.unscaledDeltaTime);
        pausemenucg.interactable = paused;
        pausemenucg.blocksRaycasts = paused;
        
        
        settingscg.alpha = Mathf.Lerp(settingscg.alpha, settingsOn? 1: 0, 8 * Time.unscaledDeltaTime);
        settingscg.interactable = settingsOn;
        settingscg.blocksRaycasts = settingsOn;
    }

    public void unpause(){
        paused = false;
        settingsOn = false;
        Time.timeScale = paused? 0:1;
    }

    public void settings(){
        settingsOn = !settingsOn;
    }

    public void quit(){
        Application.Quit();
    }

    public void Die(){
        deaths++;
        ds.enabled = true;
        ds.deaths = deaths;

    }

    public void Restart(){
        SceneManager.LoadScene(1);
    }

    public void Win(){
        FadeToBlack.fb.toA = 1;
        StartCoroutine(ReturnToMenu());
    }

    IEnumerator ReturnToMenu(){
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
    }

    //Settings functions:
    public AudioMixer mixer;
    public void SetSoundVolume(float vol){
        mixer.SetFloat("Volume", vol);
    }

}