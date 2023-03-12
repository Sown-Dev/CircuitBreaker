using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

public class Turret : EnemyClass, IDamagable{
    //Gradients
    public Gradient redCol;
    public Gradient shootingCol;

    //Prefabs
    public GameObject bullet;
    public GameObject blood;
    public GameObject scrapgibs;
    public Transform bulletSpawnPos;

    //Health and Stats
    private float Health;
    public float maxHealth = 200;
    [HideInInspector] StateEnum State;

    private float Itime = 0; //invincibilty time

    //Other Stuff

    private GameObject _player;
    private bool aware; //whether or not the enemy is aware of the player.
    private Vector3 lastSeen;
    public Animator am;
    public LayerMask player;
    public LayerMask level;
    public LayerMask both;
    float shootDelay;
    public LineRenderer lr;



    private void Awake(){
        marker.SetActive(false);
        State = StateEnum.Passive;
        Health = maxHealth;
        aware = false;
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    

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

    private float range = 10;
    private Vector3 shootpos;
    private float shootWait;
    private Vector2 lrTopos;

    int burst;
    public int maxBurst=4;
    public float shotWaitMax = 0.08f;
    void Tick(){
        lr.enabled = false;
        //Have Raycast behind to detect player if they are there for long enough:


        //AI: start by raycasting:

        RaycastHit2D hit = Physics2D.Raycast(transform.position,
            (_player.transform.position - transform.position).normalized, range, both.value);

        if (hit.collider != null){
            if (((1 << hit.collider.gameObject.layer) & player.value) != 0){
                if (visibility < 40){
                    visibility++;
                }

                lastSeen = hit.collider.transform.position; // set last seen to the hit.


                if (State != StateEnum.Shooting && visibility > 30 && State != StateEnum.ShootingDelay){
                    //can enter shooting from any state
                    shootDelay = 1f; //start with some delay so you don't instantly die
                    State = StateEnum.Shooting;
                }
            }
            else{
                visibility = 0;
            }
        }
        else{
            visibility = 0;
        }

       
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
        GameObject bul = Instantiate(bullet, bulletSpawnPos.position, turretHead.rotation);
        bul.transform.right = turretHead.transform.right * -transform.localScale.x;
    }

    private void OnDrawGizmos(){
        Gizmos.color = Color.blue;
        //Gizmos.DrawLine(transform.position, transform.position + ((_player.transform.position-transform.position).normalized * 10));
        Gizmos.DrawLine(transform.position - new Vector3(0, 0.4f, 0),
            transform.position - new Vector3(0, 0.4f, 0) + ((transform.right * transform.localScale.x) * 0.8f));

        Gizmos.DrawLine(transform.position, transform.position + ((transform.right * -transform.localScale.x) * 6f));

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right * transform.localScale.x, 10,
            player.value);

        if (hit.collider != null){
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, hit.collider.gameObject.transform.position);
        }

        if (State == StateEnum.Searching){
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position + new Vector3(0, 0.4f, 0), lastSeen);
        }
        //Handles.Label(transform.position+ new Vector3(0, 1, 0), State.ToString());
    }

    enum StateEnum{
        Passive = 1,
        Searching = 2,
        Shooting = 3,
        ShootingDelay = 4
    };

    public void takeDamage(int dmg, Vector3 hit, bool tazer, float stun, int owner){
        //If marked, double damage
        dmg *= marked ? 2 : 1;

        Instantiate(blood, hit, Quaternion.identity);
        if (Itime <= 0){
            if (owner == 0){
                Itime = 0.1f;
                am.SetTrigger("Hit");
                aware = true;
                State = StateEnum.Searching;
            }

            shootDelay += 1f;
            if (Health > dmg){
                TimeMan.tm.TimeFreeze(0.11f);
                Health -= dmg;
            }
            else{
                TimeMan.tm.TimeFreeze(0.2f, 0.5f);
                Health = 0;
                Die();
            }
        }
    }
    


    void Die(){
        ScreenShake.camShake.Shake(0.2f, 0.2f);
        Instantiate(scrapgibs, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}