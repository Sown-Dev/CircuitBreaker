using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.ForceSoftware;
    public Vector2 hotSpot = new Vector2(100,100);
    
    void Awake()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

}
