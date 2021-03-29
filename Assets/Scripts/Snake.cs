using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Snake : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public float distance;
    public float aggroRange;
    Seeker seeker;
    Rigidbody2D rb;
    Path path;
    public float speed = 300f;
    public float nextWaypointDistance = 3f;
    public ForceMode2D fMode;
    public bool aggro = false;
    public bool dead = false;
    public bool attack = false;
    public bool moving = false;
    
    bool pathIsEnded = false;

    public Transform target;
    public Animator anim;
    int currentWaypoint = 0;
    //public GameObject deathEffect;
    

    void Start(){
        anim = gameObject.GetComponent<Animator>();
        if(dead == false){
            currentHealth = maxHealth;
            seeker = GetComponent<Seeker>();
            rb = GetComponent<Rigidbody2D>();
            InvokeRepeating("UpdatePath",0f,.5f);
        }
    }

    void Aggro(){
        anim = gameObject.GetComponent<Animator>();
    }

    public void TakeDamage(int damage){
        currentHealth -= damage;
        if(currentHealth <= 0){
            Die();
        }
    }

    void Die(){
        anim = gameObject.GetComponent<Animator>();
        dead = true;
        StartCoroutine(wait(2));
    }
    public IEnumerator wait(float counter){
        yield return new WaitForSeconds(counter);
        Destroy(gameObject);
    }

    void UpdatePath(){
        if(seeker.IsDone() && aggro == true){
            seeker.StartPath(transform.position, target.position, OnPathComplete);
        }
    }
    
    public void OnPathComplete(Path p){
        if(!p.error){
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate(){
        anim.SetBool("aggro", aggro);
        anim.SetBool("isDead", dead);
        anim.SetBool("isMoving", moving);
        anim.SetBool("attack", attack);
        if(dead == false){
            RangeCheck();
            Attack();
            if(path == null){
                return;
            }
            if(currentWaypoint >= path.vectorPath.Count){
                pathIsEnded = true;
                return;
            }else{
                pathIsEnded = false;
            }
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;
            rb.AddForce(force);
            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
            if(distance < nextWaypointDistance){
                currentWaypoint++;
            }
            if(target.transform.position.x < transform.position.x){
                transform.localScale = new Vector3(1, 1, 1);
            }else{
                transform.localScale = new Vector3(-1, 1, 1);
            }
            if(currentHealth <= 0){
                Die();
            }
        }
    }

     void RangeCheck(){
         distance = Vector3.Distance(transform.position, target.transform.position);
         if(distance < aggroRange){
             aggro = true;
             moving = true;
         }else{
             aggro = false;
             moving = false;
         }
     }

     public void Attack(){
         if(distance<=1){
             attack = true;
         }else{
             attack = false;
         }
     }
}
