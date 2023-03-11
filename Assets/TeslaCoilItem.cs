using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TeslaCoilItem : ItemMan{
    private float cooldown = 2.5f;
    private int damage = 0;
    private int level=0;
    private float radius= 2.6f;

    public LayerMask enemies;
    public SpriteRenderer sr;
    public GameObject visuals;

    public LineRenderer lr;
    public CircleRenderer cr;
    private void Awake(){
        lr.enabled = false;
    }

    public override void Add(){
        level++;
        damage += 10;
        cooldown *= 0.8f;
        radius *= 1.2f;
        
        cr.xradius = radius;
        cr.yradius = radius;
        cr.CreatePoints();
    }

    private float toW = 0;
    private float tElapsed;
    private void Update(){
        
        lr.SetPosition(0, transform.position);
        lr.widthMultiplier = Mathf.Lerp(lr.widthMultiplier, toW, 11 * Time.deltaTime);
        sr.enabled = level > 0;
        visuals.SetActive(level>0);

        tElapsed += Time.deltaTime;
        if (level > 0 && tElapsed > cooldown){
            tElapsed = 0;
            Zap();
        }

    }

    public Animator am;
    void Zap(){
        am.SetTrigger("Shock");
        Collider2D[] colliders =
            Physics2D.OverlapCircleAll(transform.position, radius, enemies.value);

        for (int i = 0; i < colliders.Length; i++){
            if (colliders[i].gameObject != gameObject){
                Debug.Log("HIT!!!");
                //get first collider
                colliders[i].gameObject.GetComponent<IDamagable>().takeDamage(damage, colliders[i].transform.position, true, 0.1f, 0);
                StartCoroutine(ShockFX(colliders[i].gameObject.transform.position));
                break;
            }
        }
    }
    
    IEnumerator ShockFX(Vector3 pos){
        lr.enabled = true;
        lr.SetPosition(1, pos);
        lr.widthMultiplier = 0.17f;
        toW = 0;
        yield return new WaitForSeconds(0.2f);
        lr.enabled = false;
    }
}
