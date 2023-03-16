using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SawDrone : EnemyClass{
    private Rigidbody2D rb;
    public Light2D light;

    private void Awake(){
        base.Awake();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate(){
        Stabilization();

        Tick();
    }

    void Stabilization(){
        light.color = Color.green;
        //rotation stabilization
        if (Mathf.Abs(rb.rotation) > 20){
            rb.AddTorque(((rb.rotation > 0) ? -1 : 1) * 80f * Time.deltaTime);
            rb.AddTorque(((rb.rotation > 0) ? -1 : 1) * 0.8f * Time.deltaTime * Mathf.Abs(rb.rotation));
        }

        //hover
        if (rb.velocity.y < -0.2f){
            rb.AddForce(transform.up*900f*Time.deltaTime);
            rb.AddForce(Vector2.up*100f*Time.deltaTime);
        }

        float ud = 1.5f;
        float amt = 500f;
        if (Physics2D.Raycast(transform.position,
                Vector2.down, ud, level).collider != null){
            rb.AddForce(Vector2.up * amt * Time.deltaTime);
            light.color = Color.red;
            Debug.Log("psjgslkdjg");
        }

        if (Physics2D.Raycast(transform.position,
                Vector2.up, ud, level).collider != null){
            light.color = Color.red;
            rb.AddForce(Vector2.down * amt * Time.deltaTime);
        }

        if (Physics2D.Raycast(transform.position,
                Vector2.left, ud, level).collider != null){
            light.color = Color.red;
            rb.AddForce(Vector2.right * amt * Time.deltaTime);
        }

        if (Physics2D.Raycast(transform.position,
                Vector2.right, ud, level).collider != null){
            light.color=Color.red;
            rb.AddForce(Vector2.left * amt * Time.deltaTime);
        }
    }

    private void OnDrawGizmos(){
        float ud = 1.5f;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + (Vector2.left * ud));
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + (Vector2.down * ud));
        if (Physics2D.Raycast(transform.position,
                Vector2.right, ud).collider != null)
            Gizmos.DrawLine(transform.position, (Physics2D.Raycast(transform.position,
                Vector2.right, ud).transform.position));
    }

    void Tick(){
        base.Tick();
        switch (State){
            case (StateEnum.ShootingDelay):{
                break;
            }
            case (StateEnum.Shooting):{
                if (_player.transform.position.x > transform.position.x){
                    transform.localScale = new Vector3(1, transform.localScale.y, 1);
                }
                else{
                    transform.localScale = new Vector3(-1, transform.localScale.y, 1);
                }
                rb.AddForce((_player.transform.position - transform.position) * 100);
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
}