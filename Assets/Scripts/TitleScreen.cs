using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{

    public void playGame(){
        SceneManager.LoadScene(1);
    }

    public void settings(){
        
    }

    public void quit(){
        Application.Quit();
    }
}
