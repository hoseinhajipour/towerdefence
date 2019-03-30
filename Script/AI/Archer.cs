using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Archer : MonoBehaviour
{
    public float health = 100;
    public Slider health_ui;
    public float damage_power = 10.0f;
    NavMeshAgent navMeshAgent;
    public int current_level;
    string target_castle_tag = "enemy_Castle";
    string target_tag = "enemy";
    GameObject enemyCastle;
    LayerMask mask;

    public float target_finder_raduis = 10.0f;
    public float near_enemy_distance = 1.0f;
    bool attack = false;
    bool dead = false;
    Collider[] enemies;

    public Animator anim;

    public float atackRate = 1.0f;
    private float nextAttack;
    GameObject current_traget;


    public Rigidbody arrow;
    public Transform fire_pos;
    public float arrow_speed = 2000.0f;

    void Start()
    {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        mask = LayerMask.GetMask("enemy");
        if (gameObject.tag == "enemy")
        {
            target_castle_tag = "own_Castle";
            mask = LayerMask.GetMask("own");
            target_tag = "own";
            gameObject.layer = LayerMask.NameToLayer("enemy");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("own");
        }
        enemyCastle = GameObject.Find(target_castle_tag);
    }

    void Update()
    {

        enemies = Physics.OverlapSphere(transform.position, target_finder_raduis, mask);
        if (enemies.Length > 0)
        {
            float dist = Vector3.Distance(transform.position, enemies[0].transform.position);
            current_traget = enemies[0].gameObject;
            if (dist <= near_enemy_distance)
            {
                attack = true;
                navMeshAgent.Stop();
       
            }
            else
            {
                attack = false;
                navMeshAgent.Resume();
                navMeshAgent.SetDestination(enemies[0].transform.position);
            }
            
        }
        else
        {
            attack = false;
            navMeshAgent.Resume();
            navMeshAgent.SetDestination(enemyCastle.transform.position);
            current_traget = enemyCastle;
        }



        if (attack == false)
        {
            //path finding
            anim.Play("Run");
        }
        else
        {
            if (Time.time > nextAttack)
            {
                anim.Play("ArrowAttack");

                Rigidbody bullet_clone = Instantiate(arrow, fire_pos.position, fire_pos.rotation) as Rigidbody;
                bullet_clone.GetComponent<bullet>().parnet_target_tag = target_tag;
                bullet_clone.GetComponent<bullet>().damage_power = damage_power;
                bullet_clone.AddForce(fire_pos.forward * arrow_speed);

                transform.LookAt(current_traget.transform);
                // float rnd_dmg = Random.Range(damage_power / 1.2f, damage_power);
                // current_traget.SendMessage("ApplyDamage", damage_power);
                nextAttack = Time.time + atackRate;
            }
        }


    }

    public void ApplyDamage(float damage)
    {
        this.health -= damage;
        health_ui.value = health / 100.0f;

        if (health < 0)
        {
            Destroy(gameObject, 3);
        }
    }

  
}
