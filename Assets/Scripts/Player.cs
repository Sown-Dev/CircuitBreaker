using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamagable{
    public CinemachineVirtualCamera cm;
    public GameObject swordParticles;

    public ItemMan[] itemMans;
    [HideInInspector] public int shields=1;

    public int damage = 40;
    public bool platforms;
    public GameObject dust;

    public GameObject dashAC;
    public GameObject attackAC;
    public GameObject blockC;
    [SerializeField] private Transform m_GroundCheck; // A position marking where to check if the player is grounded.

    private int jumpCache = 1;
    public Animator am;
    public Animator armam;
    Rigidbody2D rb;
    public GameObject gibs;
    public LayerMask level;
    public LayerMask[] avoid; // what layers to stop colliding with when dashing
    public LayerMask myLayer;
    public SpriteRenderer sr;

    public Image blockAMT;

    private int maxJumps = 1;

    public ParticleSystem shatter;

    //audio:
    public AudioSource src;
    public AudioClip electricsword;
    public AudioClip runtiles;
    public AudioClip runactuator;
    public AudioClip discharge;
    void Awake(){
        taserOBJ.SetActive(false);
        dashAC.SetActive(false);
        attackAC.SetActive(false);
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private float jumpV = 1230;
    private float moveV = 1000;

    private bool m_Grounded;
    public int jumpType = 0; //0 normal jump, 1 = dash


    private void FixedUpdate(){
        
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        Collider2D[] colliders =
            Physics2D.OverlapBoxAll(m_GroundCheck.position, new Vector2(0.48f, 0.056f), 0, level.value);

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

    //blocking vars:
    private bool blocking;
    bool prevblocking;

    public float blockHealth;

    //blocking params:
    public float blockRegen = 0.8f;
    public float maxBlock = 210;

    public Vector3 mouseDir;
    
    //audio stuff:
    private Vector3 startPos; //picks new pos every 3 seconds, moving from this position will produce footsteps
    private float elapsed;
    void Update(){
        if (GameMan.GM.paused){
            return;
        }
        
        if (m_Grounded){
            elapsed += Time.deltaTime;
            if (elapsed > 2f){
                elapsed = 0;
                startPos = transform.position;
            }

            if (Vector2.Distance(startPos, transform.position) > 1.1f){
                startPos = transform.position;
                src.PlayOneShot(runtiles, 0.7f);
            }
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        mouseDir = (mousePos - transform.position).normalized;
        mouseDir.z = 0; //just to be sure
            
            
        if (dashCooldown > 0){
            dashCooldown -= Time.deltaTime;
        }

        //if (Input.GetMouseButtonDown(0)){
            //Attack();
        //}

        if (Input.GetMouseButtonDown(1) && !prevblocking && blockHealth > 30){
            blocking = true;
        }
        else{
            blocking = Input.GetMouseButton(1) && blockHealth > 0 && prevblocking;
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

        platforms = Input.GetKey(KeyCode.S);

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && m_Grounded){
            yv += 1;
            Instantiate(dust, transform.position, Quaternion.identity);
            am.SetTrigger("Jump");
        }

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && !m_Grounded && jumpCache > 0){
            jumpCache--;
            if (jumpType == 1){
                VerticalDash();
            }
            else if (jumpType == 0){
                rb.velocity *= 0.5f;
                yv += 0.65f; //slightly weaker double jump
                Instantiate(dust, transform.position, Quaternion.identity);
                am.SetTrigger("Jump");
            }
        }

        if (Input.GetMouseButtonDown(0) && !am.GetBool("Dash") && dashCooldown <= 0){
            
            Dash(mouseDir);
        }

        float mult = m_Grounded ? 2f : 1.5f;


        rb.AddForce(transform.up * (yv * jumpV));
        rb.AddForce(transform.right * (xv * moveV * mult * Time.deltaTime));
        am.SetFloat("Speed", math.abs(rb.velocity.x));
        armam.SetFloat("Speed", math.abs(rb.velocity.x));


        //scale:
        ah.trackMouse = blocking;
        if (blocking){
            
            transform.localScale = new Vector3(mousePos.x < transform.position.x ? -1 : 1, 1, 1);
        }
        else if (math.abs(rb.velocity.x) > 0.3f)
            transform.localScale = new Vector3(rb.velocity.x < 0 ? -1 : 1, 1, 1);


        am.SetBool("Falling", math.abs(rb.velocity.y) > 1f);
        am.SetBool("Grounded", m_Grounded);

        //block code
        blockHealth += blocking ? -0.5f : 0;
        if (!blocking && blockHealth < maxBlock){
            blockHealth += blockRegen;
        }

        blockHealth = Mathf.Clamp(blockHealth, 0, maxBlock); //clamp it so that ui doesnt bug out

        blockC.SetActive(blocking);
        armam.SetBool("Blocking", blocking);
        armam.SetBool("WasBlocking", prevblocking);
        prevblocking = blocking;

        blockAMT.fillAmount = blockHealth / maxBlock;
        blockAMT.enabled = blocking || blockHealth < maxBlock;
    }

    void Attack(){
        am.SetTrigger("Attack");
        StartCoroutine(AttackCR());
    }

    public ArmsHolder ah;
    void Dash(Vector3 direction){
        src.PlayOneShot(electricsword,0.06f);
        rb.velocity *= 0.16f;
        dashCooldown += 0.6f;
        am.SetBool("Dash", true);
        armam.SetBool("Dash", true);
        armam.SetTrigger("DashT");
        rb.AddForce(direction * (1f * moveV) );
        StartCoroutine(DashCR());
    }

    
    void VerticalDash(){
        src.PlayOneShot(electricsword,0.06f);
        rb.velocity *= 0.1f; // reset force
        rb.AddForce(transform.up * (1.1f * jumpV));
        am.SetTrigger("UpDash");
    }

    IEnumerator DashCR(){
        rb.gravityScale = 0;
        ah.trackPhys = true;
        dashAC.SetActive(true);
        foreach (LayerMask l in avoid){
            Physics2D.IgnoreLayerCollision((int)Mathf.Log(myLayer.value, 2), (int)Mathf.Log(l.value, 2), true);
        }


        sr.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(0.21f);
        ah.trackPhys = false;
        dashAC.SetActive(false);
        am.SetBool("Dash", false);
        armam.SetBool("Dash", false);
        foreach (LayerMask l in avoid){
            Physics2D.IgnoreLayerCollision((int)Mathf.Log(myLayer.value, 2), (int)Mathf.Log(l.value, 2), false);
        }

        rb.gravityScale = 5;

        sr.color = new Color(1, 1, 1, 1f);
        rb.velocity *= 0.4f;
    }

    IEnumerator AttackCR(){
        yield return new WaitForSeconds(0.03f);
        attackAC.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        attackAC.SetActive(false);
        am.SetBool("Dash", false);
    }

    public void takeDamage(int dmg, Vector3 hitpoint, bool tazer, float stun, int owner){
        ScreenShake.camShake.Shake(0.3f, 0.35f);
        if (shields <= 0){
            Die();
        }
        else{
            shields--;
            shatter.Play();
        }
    }

    void Die(){
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

    public GameObject taserOBJ;
    public void AddItem(int Index){
        if (Index > -1){ //positive indices have an item manager
            itemMans[Index].Add();
        }

        else{
            if (Index == -1){ //-1 adds shield
                shields += 2;
            }

            if (Index == -2){ //-2 adds jump
                maxJumps++;
            }

            if (Index == -3){ // -3 adds taser
                taser = true;
                taserOBJ.SetActive(true);
                extrastun += 0.4f;
            }

            if (Index == -4){
                moveV += 170f;
                jumpV += 140f;
            }

            if (Index == -5){
                blockRegen += 1f;
                maxBlock += 50f;
            }

            if (Index == -6){
                jumpType = 1;
            }
        }
    }

    public bool taser;
    public float extrastun;
    //base stun is in the AC class
}