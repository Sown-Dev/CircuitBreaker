using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour{
    public GameObject player;
    public bool CopyScale;
    void Update()
    {
        Vector3 playerPos = player.transform.position;
        transform.position =new Vector3(playerPos.x, playerPos.y, 0);//Vector3.Lerp(transform.position, new Vector3(Mathf.Round(playerPos.x), Mathf.Round(playerPos.y),-10), 5 * Time.deltaTime);
        //new Vector3(playerPos.x, playerPos.y, -10);
            //Vector3.Lerp(transform.position, new Vector3(playerPos.x, playerPos.y,-10), 5 * Time.deltaTime);
            transform.localScale = CopyScale ? player.transform.localScale : transform.localScale;
    }
}
