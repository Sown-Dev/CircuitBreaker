using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

public class Turret : EnemyClass{
    //Gradients
    public Gradient redCol;
    public Gradient shootingCol;

    //Prefabs
    public GameObject bullet;
    public Transform bulletSpawnPos;

    //Health and Stats
    

    private float Itime = 0; //invincibilty time

    //Other Stuff

 
    float shootDelay;
    public LineRenderer lr;

    //Audio
    public AudioClip shoot;
    public AudioClip lockon;

   
    

    void FixedUpdate(){
       

        //Line Renderer lerp stuff:
        lr.SetPosition(1, Vector2.Lerp(lr.GetPosition(1), lrTopos, 10 * Time.deltaTime));


        


        if (shootDelay > 0){
            shootDelay -= Time.deltaTime;
        }

        if (shootWait > 0){
            shootWait -= Time.deltaTime;
        }

        if (Itime > 0){
            Itime -= Time.deltaTime;
        }

        Tick();
    }

    private int visibility; //how many frames it has seen player. if above threshold, it shoots
    private int backvis; // same thing but for back

    private float
        awarenesstime; //how many frames since last saw player. if this is too high, it forgets about the player

    private Vector3 shootpos;
    private float shootWait;
    private Vector2 lrTopos;

    int burst;
    public int maxBurst=4;
    public float shotWaitMax = 0.08f;
    public void Tick(){
        base.Tick();
        lr.enabled = false;
        //Have Raycast behind to detect player if they are there for long enough:


        //AI: start by raycasting:

        
       
        switch (State){
            case (StateEnum.ShootingDelay):{
                
                lr.colorGradient = shootingCol;
                RaycastHit2D shothit = Physics2D.Raycast(transform.position,
                    (shootpos - transform.position).normalized, range, level.value);

                if (shothit.collider != null){
                    lr.enabled = true;
                    lr.SetPosition(0, lr.transform.position);
                    lrTopos = shothit.point;
                }
                else{
                    lr.enabled = true;
                    lr.SetPosition(0, lr.transform.position);
                    lrTopos = (_player.transform.position - transform.position).normalized * range + transform.position;
                }
                turretHead.transform.right = (shootpos - transform.position) * -transform.localScale.x;
                if (shootWait <= 0){
                    if (burst < maxBurst){
                        am.SetTrigger("Shoot");
                        Shoot();
                        shootWait = shotWaitMax;
                    }
                    burst++;
                    shootDelay = 2f;
                    if (burst >= maxBurst+5){ //add 5 frames to not instantly transition
                        State = StateEnum.Shooting;
                    }
                }

                break;
            }
            case(StateEnum.Stunned):{
                stunTime -= Time.deltaTime;
                if (stunTime <= 0){
                    tased = false;
                    State = StateEnum.Shooting;
                }
                break;
            }
            case (StateEnum.Shooting):{
                lr.colorGradient = redCol;

                RaycastHit2D shothit = Physics2D.Raycast(transform.position,
                    (_player.transform.position - transform.position).normalized, range, level.value);

                if (shothit.collider != null){
                    lr.enabled = true;
                    lr.SetPosition(0, lr.transform.position);
                    lrTopos = shothit.point;
                }
                else{
                    lr.enabled = true;
                    lr.SetPosition(0, lr.transform.position);
                    lrTopos = (_player.transform.position - transform.position).normalized * range + transform.position;
                }

                if (_player.transform.position.x < transform.position.x){
                    transform.localScale = new Vector3(1, transform.localScale.y, 1);
                }
                else{
                    transform.localScale = new Vector3(-1,  transform.localScale.y, 1);
                }

                turretHead.transform.right = (_player.transform.position - transform.position) * -transform.localScale.x;

                if (Itime <= 0 && shootDelay <= 0){
                    burst = 0;
                    State = StateEnum.ShootingDelay;
                    shootWait = 0.3f;
                    aware = true;
                    shootpos = _player.transform.position;
                }

                if (visibility <= 0){
                    State = StateEnum.Searching;
                }


                break;
            }
            case (StateEnum.Passive):{
                RaycastHit2D bcheck = Physics2D.Raycast(transform.position, transform.right * -transform.localScale.x,
                    6, player.value);


                if (bcheck.collider != null){
                    backvis++;
                    if (backvis > 18){
                        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
                        State = StateEnum.Searching;
                        lastSeen = bcheck.collider.transform.position; // set last seen to the hit.
                        aware = true;
                    }
                }
                else{
                    if (backvis > 0)
                        backvis--;
                }

                break;
            }

            case (StateEnum.Searching):{
                State = StateEnum.Passive;
                break;
            }
        }
    }


    public Transform turretHead;
    void Shoot(){
        src.PlayOneShot(shoot,0.8f);
        GameObject bul = Instantiate(bullet, bulletSpawnPos.position, turretHead.rotation);
        bul.transform.right = turretHead.transform.right * -transform.localScale.x;
    }

   
   
}