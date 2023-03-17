using System;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class EnemyClass : MonoBehaviour, IDamagable{
    public GameObject marker;

    public Image hpbar;

    public float Health = 0;
    public float maxHealth = 160;

    public bool marked;

    public void mark(){
        marked = true;
        marker.SetActive(true);
    }

    private void Update(){
        hpbar.enabled = Health < maxHealth;
        hpbar.fillAmount = Health / maxHealth;
    }


    public GameObject blood;
    public GameObject gibs;


    //Health and Stats

    [HideInInspector] public StateEnum State;
    [HideInInspector] public float Itime = 0; //invincibilty time

    //Other Stuff

    [HideInInspector] public GameObject _player;

    [HideInInspector] public bool aware; //whether or not the enemy is aware of the player.
    [HideInInspector] public Vector3 lastSeen;
    public Animator am;
    public LayerMask player;
    public LayerMask level;
    public LayerMask both;


    public void Awake(){
        taserFX.SetActive(false);
        marker.SetActive(false);
        State = StateEnum.Passive;
        Health = maxHealth;
        aware = false;
        _player = GameObject.FindGameObjectWithTag("Player");
    }


    //AUDIO STUFF:
    public AudioSource src;
    public AudioClip hit1;
    public AudioClip hit2;


    public void FixedUpdate(){
        am.SetInteger("State", (int)State);
        if (Itime >= 0){
            Itime -= Time.deltaTime;
        }
        Tick();
    }

    [HideInInspector] public int visibility;

    [HideInInspector] public float awarenesstime;

    public float range = 10;


    public void Tick(){
        taserFX.SetActive(tased && State == StateEnum.Stunned);


        //AI: start by raycasting:

        RaycastHit2D hit = Physics2D.Raycast(transform.position,
            (_player.transform.position - transform.position).normalized, range, both.value);

        if (hit.collider != null && State != StateEnum.Stunned){
            if (((1 << hit.collider.gameObject.layer) & player.value) != 0){
                if (visibility < 30){
                    visibility++;
                }

                lastSeen = hit.collider.transform.position; // set last seen to the hit.


                if (State != StateEnum.Shooting && visibility > 20 && State != StateEnum.ShootingDelay){
                    
                    if (_player.transform.position.x > transform.position.x){
                        transform.localScale = new Vector3(1, transform.localScale.y, 1);
                    }
                    else{
                        transform.localScale = new Vector3(-1,  transform.localScale.y, 1);
                    }

                    //can enter shooting from any state
                    DetectPlayer();
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
                break;
            }
            case (StateEnum.Shooting):{
                break;
            }
            case (StateEnum.Passive):{
                break;
            }
            case (StateEnum.Stunned):{
                break;
            }

            case (StateEnum.Searching):{
                break;
            }
        }
    }

    public GameObject taserFX;

    [HideInInspector] public float stunTime = 0;

    public void OnDrawGizmos(){
        Handles.Label(transform.position+(Vector3)(Vector2.up*1.5f),State.ToString());
    }

    public enum StateEnum{
        Passive = 1,
        Searching = 2,
        Shooting = 3,
        ShootingDelay = 4,
        Stunned = 5
    };

    [HideInInspector] public bool tased = false;

    public void takeDamage(int dmg, Vector3 hit, bool tazer, float stun, int owner){
        //If marked, double damage
        tased = tazer;
        dmg *= marked ? 2 : 1;
        State = StateEnum.Stunned;
        stunTime = stun + 0.05f;
        Instantiate(blood, hit, Quaternion.identity);
        if (Itime <= 0){
            am.SetTrigger("Hit");
            if (owner == 0){
                Itime = 0.06f;

                aware = true;
                src.PlayOneShot(hit1, 0.8f);
            }

            if (owner == -1){
                Itime = 0.06f;

                aware = true;
                src.PlayOneShot(hit2, 0.8f);
            }
            else{
                src.PlayOneShot(hit2, 0.8f);
            }

            if (Health > dmg){
                TimeMan.tm.TimeFreeze(0.11f);
                Health -= dmg;
            }
            else{
                TimeMan.tm.TimeFreeze(0.2f, 0.5f);
                Health = 0;
                Die();
            }

            State = StateEnum.Stunned;
        }
    }


    public void Die(){
        ScreenShake.camShake.Shake(0.3f, 0.3f);
        Instantiate(gibs, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public virtual void DetectPlayer(){
        State = StateEnum.Shooting;
    }

   
}