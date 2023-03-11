using UnityEngine;
using UnityEngine.Timeline;

public class EnemyClass : MonoBehaviour{
    public GameObject marker;
    public bool marked;
    public void mark(){
        marked = true;
        marker.SetActive(true);
    }
}