
using System;
using System.Collections;
using UnityEngine;

public class TimeMan : MonoBehaviour{
    public static TimeMan tm;

    private void Awake(){
        //Time.timeScale = 0.5f;
        tm = this;
    }

    public void TimeFreeze(float dur){
        StartCoroutine(TFHelper(dur,0.17f));
    }
    public void TimeFreeze(float dur, float tmult){
        StartCoroutine(TFHelper(dur, tmult));
    }
    
    private IEnumerator TFHelper(float dur, float tmult){
        Time.timeScale = tmult;
        yield return new WaitForSecondsRealtime(dur);
        Time.timeScale = 1f;
    }
}