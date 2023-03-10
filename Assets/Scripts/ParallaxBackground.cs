using System;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour{
    [SerializeField] private Vector2 parallaxMult;
    public Transform camTransform;
    private Vector3 lastPos;
    
    private float textureUnitSizeX;
    private float textureUnitSizeY;
    
    void Start(){
        lastPos = camTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
    }

    private void LateUpdate(){
        Vector3 MovementChange = camTransform.position - lastPos;
        transform.position += new Vector3(MovementChange.x * parallaxMult.x, MovementChange.y * parallaxMult.y);
        lastPos = camTransform.position;

        if (Mathf.Abs(camTransform.position.x - transform.position.x )>= textureUnitSizeX){
            float offsetPositionX = (camTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(camTransform.position.x +offsetPositionX, transform.position.y);
        }
        
        if (Mathf.Abs(camTransform.position.y - transform.position.y) >= textureUnitSizeY){
            float offsetPositionY = (camTransform.position.y - transform.position.y) % textureUnitSizeY;
            transform.position = new Vector3( transform.position.x, camTransform.position.y +offsetPositionY);
        }
    }
}