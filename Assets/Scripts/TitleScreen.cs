using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{

    public void playGame(){
        GameMan.GM.Start();
    }

    public void settings(){
        
    }

    public void quit(){
        Application.Quit();
    }
}
