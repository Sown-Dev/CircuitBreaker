using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class Player : MonoBehaviour, IDamagable{

    public CinemachineVirtualCamera cm;
    public GameObject swordParticles;
    
     public ItemMan[] itemMans;
    [HideInInspector] public int shields;

    public int damage = 40;

    public GameObject dust;

    public GameObject dashAC;
    public GameObject attackAC;
    [SerializeField] private Transform m_GroundCheck; // A position marking where to check if the player is grounded.

    private int jumpCache = 1;
    public Animator am;
    Rigidbody2D rb;
    public GameObject gibs;
    public LayerMask level;
    public LayerMask[] avoid; // what layers to stop colliding with when dashing
    public LayerMask myLayer;
    public SpriteRenderer sr;

    private int maxJumps;
    void Awake(){
        dashAC.SetActive(false);
        attackAC.SetActive(false);
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private float jumpV = 1100;
    private float moveV = 1000;

    private bool m_Grounded;

    private void FixedUpdate(){
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(m_GroundCheck.position,new Vector2(0.48f,0.056f), 0,level.value);

        for (int i = 0; i < colliders.Length; i++){
            if (colliders[i].gameObject != gameObject){
                m_Grounded = true;
                jumpCache = maxJumps;
                if (!wasGrounded){
                    //landing sound
                }
            }
        }
    }

    private float dashCooldown;
    void Update(){
        if (dashCooldown > 0){
            dashCooldown -= Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(0)){
            Attack();
        }


        float xv = 0;
        float yv = 0;
        if (Input.GetKey(KeyCode.A)){
            xv -= 1;
            //transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.D)){
            xv += 1;
            //transform.localScale = new Vector3(1, 1, 1);
        }

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && m_Grounded){
            yv += 1;
            Instantiate(dust, transform.position, Quaternion.identity);
            am.SetTrigger("Jump");
        }

        if ((Input.GetKeyDown(KeyCode.W)) && !m_Grounded && jumpCache > 0){
            jumpCache--;
            VerticalDash();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !am.GetBool("Dash") && dashCooldown<=0){
            Dash(1);
        }

        float mult = m_Grounded ? 2f : 1.5f;


        rb.AddForce(transform.up * (yv * jumpV));
        rb.AddForce(transform.right * (xv * moveV * mult * Time.deltaTime));
        am.SetFloat("Speed", math.abs(rb.velocity.x));

        if(math.abs(rb.velocity.x)>0.3f)
            transform.localScale=  new Vector3(rb.velocity.x <0 ? -1: 1, 1,1);
            

        am.SetBool("Falling", math.abs(rb.velocity.y) > 0.1f);
        am.SetBool("Grounded", m_Grounded);
    }

    void Attack(){
        am.SetTrigger("Attack");
        StartCoroutine(AttackCR());
    }

    void Dash(float xv){
        dashCooldown += 1f;
        am.SetBool("Dash", true);
        am.SetTrigger("DashT");
        rb.AddForce(transform.right * (xv * moveV)*transform.localScale.x);
        StartCoroutine(DashCR());
    }

    void VerticalDash(){
        rb.velocity *= 0.1f; // reset force
        rb.AddForce(transform.up * (1.2f * jumpV));
        am.SetTrigger("UpDash");
    }

    IEnumerator DashCR(){
        rb.gravityScale = 0;
        dashAC.SetActive(true);
        foreach (LayerMask l in avoid){
            Physics2D.IgnoreLayerCollision((int)Mathf.Log(myLayer.value, 2), (int)Mathf.Log(l.value, 2), true);
        }


        sr.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(0.21f);
        
        dashAC.SetActive(false);
        am.SetBool("Dash", false);
        foreach (LayerMask l in avoid){
            Physics2D.IgnoreLayerCollision((int)Mathf.Log(myLayer.value, 2), (int)Mathf.Log(l.value, 2), false);
        }   
        rb.gravityScale = 5;

        sr.color = new Color(1, 1, 1, 1f);  
        rb.velocity *= 0.4f; 
    }

    IEnumerator AttackCR(){
        yield return new WaitForSeconds(0.02f);
        attackAC.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        attackAC.SetActive(false);
        am.SetBool("Dash", false);
    }

    public void takeDamage(int dmg, Vector3 hit){
        if (shields <= 0){
            Die();
        }
        else{
            shields--;
        }
    }

    void Die(){
        ScreenShake.camShake.Shake(0.3f,0.25f);
        swordParticles.SetActive(false);
        GameObject body = Instantiate(gibs, transform.position, Quaternion.identity);
        transform.position += new Vector3(0, 1000000, 0);
        cm.Follow = body.transform;
        StartCoroutine(DeathCleanup());
    }

    IEnumerator DeathCleanup(){
        yield return new WaitForSeconds(0.4f);
        GameMan.GM.Die();
    }

    private void OnCollisionEnter2D(Collision2D col){
        if (col.collider.gameObject.layer == LayerMask.NameToLayer("LaserGate")){
            Die();
        }
    }

    public void AddItem(int Index){
        if (Index > -1){ //-1 adds shield
            itemMans[Index].Add();
        }
        
        else{
            if (Index == -1){ //-1 adds shield
                shields++;
                
            }
            if (Index == -2){ //-1 adds shield
                maxJumps++;
            }
            
        }
    }
}