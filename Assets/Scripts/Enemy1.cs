using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

public class Enemy1 : EnemyClass, IDamagable{
    //Gradients
    public Gradient redCol;
    public Gradient shootingCol;

    //Prefabs
    public GameObject bullet;
    public GameObject blood;
    public GameObject gibs;
    public Transform bulletSpawnPos;
    public GameObject arms;

    //Health and Stats
    private float Health;
    private float maxHealth = 160;
    [HideInInspector] StateEnum State;
    private float xVel = 1600;
    private float jumpV = 900;
    private float Itime = 0; //invincibilty time

    //Other Stuff

    private GameObject _player;
    private Rigidbody2D rb;
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
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    [SerializeField] private Transform m_GroundCheck;
    private bool m_Grounded;

    [SerializeField] private Transform PitCheck;

    
    //AUDIO STUFF:
    public AudioSource src;
    public AudioClip hit1;
    public AudioClip hit2;
    public AudioClip shootsfx;
    public AudioClip walk;
    
    
    void FixedUpdate(){
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

    void Tick(){
        lr.enabled = false;
        


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

                arms.SetActive(true);
                arms.transform.right = (shootpos - transform.position) * transform.localScale.x;
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


                arms.SetActive(true);
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
                arms.SetActive(false);
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
                arms.SetActive(false);
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
        src.PlayOneShot(shootsfx,0.8f);
        GameObject bul = Instantiate(bullet, bulletSpawnPos.position, arms.transform.rotation);
        bul.transform.right = arms.transform.right * transform.localScale.x;
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
            am.SetTrigger("Hit");
            if (owner == 0){
                Itime = 0.09f;
                
                aware = true;
                State = StateEnum.Searching;
                src.PlayOneShot(hit1, 0.8f);
            }
            if (owner == -1){
                Itime = 0.09f;
                
                aware = true;
                State = StateEnum.Searching;
                src.PlayOneShot(hit2, 0.8f);
            }
            else{
                src.PlayOneShot(hit2, 0.8f);
            }

            shootDelay +=1f;
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
        Instantiate(gibs, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}