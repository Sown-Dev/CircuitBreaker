using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler{
    public UpgradeScreen screen;
    public TMP_Text name;
    public Image icon;
    public TMP_Text desc;
    private UpgradeDescription myDesc;

    public Hover myHover;
    public Image myImg;
    

    private Color toCol;

    private void Awake(){
        toCol = new Color(0.8f, 0.8f, 0.8f);
    }

    public void Set(UpgradeDescription de){
        name.text = de.name;
        icon.sprite = de.icon;
        desc.text = de.description;
        myDesc = de;
    }

    private void Update(){
        myImg.color = Color.Lerp(myImg.color, toCol, 15 * Time.deltaTime);
    }

    public void OnPointerEnter(PointerEventData eventData){
        myHover.hoverAmt = 3;
        myHover.offset = 4;
        toCol = new Color(1, 1, 1);
    }

    public void OnPointerExit(PointerEventData eventData){
        myHover.offset = 0;
        myHover.hoverAmt = 1;
        toCol = new Color(0.8f, 0.8f, 0.8f);

    }

    public void OnPointerClick(PointerEventData eventData){
        screen.Pick(myDesc.ID);
    }
}
