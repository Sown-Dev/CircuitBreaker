using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

public class Enemy1 : EnemyClass{
    //Gradients
    public Gradient redCol;
    public Gradient shootingCol;

    //Prefabs
    public GameObject bullet;
    public Transform bulletSpawnPos;
    public GameObject arms;


    
    [HideInInspector] StateEnum State;
    private float xVel = 1600;
    private float jumpV = 900;
    private float Itime = 0; //invincibilty time


    float shootDelay;
    public LineRenderer lr;


    private void Awake(){
        taserFX.SetActive(false);
        marker.SetActive(false);
        State = StateEnum.Passive;
        Health = maxHealth;
        aware = false;
        _player = GameObject.FindGameObjectWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    [SerializeField] private Transform m_GroundCheck;
    private bool m_Grounded;

    [SerializeField] private Transform PitCheck;

    
    //AUDIO STUFF:
    public AudioClip shootsfx;
    public AudioClip walk;
    
    
    void FixedUpdate(){
        arms.transform.right = (shootpos - transform.position) * transform.localScale.x;

        //ground check
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        Collider2D[] colliders =
            Physics2D.OverlapBoxAll(m_GroundCheck.position, new Vector2(0.48f, 0.056f), 0, level.value);

        for (int i = 0; i < colliders.Length; i++){
            if (colliders[i].gameObject != gameObject){
                m_Grounded = true;
                if (!wasGrounded){
                    //landing sound
                }
            }
        }

        //Line Renderer lerp stuff:
        lr.SetPosition(1, Vector2.Lerp(lr.GetPosition(1), lrTopos, 10 * Time.deltaTime));


        am.SetInteger("State", (int)State);
        am.SetFloat("Speed", math.abs(rb.velocity.x));
        am.SetBool("Falling", math.abs(rb.velocity.y) > 0.05f);


        if (shootDelay > 0){
            shootDelay -= Time.deltaTime;
        }

        if (shootWait > 0){
            shootWait -= Time.deltaTime;
        }

        if (Itime > 0){
            Itime -= Time.deltaTime;
        }

    }

    private int visibility; //how many frames it has seen player. if above threshold, it shoots
    private int backvis; // same thing but for back

    private float
        awarenesstime; //how many frames since last saw player. if this is too high, it forgets about the player

    private float range = 10;
    private Vector3 shootpos;
    private float shootWait;
    private Vector2 lrTopos;

    void Tick(){
        lr.enabled = false;
        taserFX.SetActive(tased && State== StateEnum.Stunned);


        //AI: start by raycasting:

        RaycastHit2D hit = Physics2D.Raycast(transform.position,
            (_player.transform.position - transform.position).normalized, range, both.value);

        if (hit.collider != null && State !=StateEnum.Stunned){
            if (((1 << hit.collider.gameObject.layer) & player.value) != 0){
                if (visibility < 40){
                    visibility++;
                }

                lastSeen = hit.collider.transform.position; // set last seen to the hit.


                if (State != StateEnum.Shooting && visibility > 30 && State != StateEnum.ShootingDelay){
                    if (_player.transform.position.x > transform.position.x){
                        transform.localScale = new Vector3(1, 1, 1);
                    }
                    else{
                        transform.localScale = new Vector3(-1, 1, 1);
                    }
                    //can enter shooting from any state
                    shootDelay = 0.4f; //start with some delay so you don't instantly die
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

                if (shootWait <= 0){
                    State = StateEnum.Shooting;
                    am.SetTrigger("Shoot");
                    Shoot();
                    shootDelay = 1.1f;
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

                if (_player.transform.position.x > transform.position.x){
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else{
                    transform.localScale = new Vector3(-1, 1, 1);
                }


                arms.transform.right = (_player.transform.position - transform.position) * transform.localScale.x;

                if (Itime <= 0 && shootDelay <= 0){
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
            case(StateEnum.Stunned):{
                stunTime -= Time.deltaTime;
                if (stunTime <= 0){
                    tased = false;
                    State = StateEnum.Shooting;
                }
                break;
            }

            case (StateEnum.Searching):{
                //gives position of player
                if (!Physics2D.Linecast(transform.position + new Vector3(0, 0.4f, 0), _player.transform.position,
                        level.value)){ //we rais the origin so that we cast from the head
                    awarenesstime = 0;
                    lastSeen = _player.transform.position;
                }
                else{
                    awarenesstime++;
                }

                if (awarenesstime > 100){
                    aware = false;
                    State = StateEnum.Passive;
                }

                //makes enemy go towards player
                if (Vector2.Distance(lastSeen, transform.position) > 2f){ //keep your distance
                    //if we are far enough away keep searching, otherwise we go to passive
                    if (lastSeen.x > transform.position.x){
                        transform.localScale = new Vector3(1, 1, 1);
                    }
                    else{
                        transform.localScale = new Vector3(-1, 1, 1);
                    }

                    rb.AddForce(transform.right * (xVel * Time.deltaTime * transform.localScale.x));
                    //check for incline in front to jump over obstacles
                    RaycastHit2D stairhit = Physics2D.Raycast(transform.position - new Vector3(0, 0.4f, 0),
                        transform.right, 0.9f * transform.localScale.x, level.value);

                    if (stairhit.collider != null){
                        stairVal++;
                        if (stairVal > 5 && m_Grounded){
                            stairVal = 0;
                            //jump!
                            rb.AddForce(transform.up * jumpV);
                        }
                    }

                    //check for pit below, in case u need to jump over
                    RaycastHit2D pithit = Physics2D.BoxCast(PitCheck.position,
                        new Vector2(0.1f, 0.1f), 0, transform.up, 0, level.value);

                    if (pithit.collider == null){ //want to check if there is nothing below
                        stairVal++;
                        if (stairVal > 4 && m_Grounded){
                            stairVal = 0;
                            rb.AddForce(transform.up * jumpV * 1.3f); //higher jumps over pit
                        }
                    }
                }
                else{
                    if (!aware)
                        State = StateEnum.Passive;
                }


                break;
            }
            
        }
    }

  
    private int stairVal; // how long ive been looking at an incline

    void Shoot(){
        rb.AddForce(arms.transform.right * transform.localScale.x*-500);
        src.PlayOneShot(shootsfx,0.8f);
        GameObject bul = Instantiate(bullet, bulletSpawnPos.position, arms.transform.rotation);
        bul.transform.right = arms.transform.right * transform.localScale.x;
    }

   

    

    


   
}