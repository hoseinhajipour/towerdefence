﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Soldier : MonoBehaviour
{

    public float health = 100;
    public float speed_movment = 1.0f;
    public float damage_power = 10.0f;
    

    public GameObject enemyCastle;

    string target_castle_tag = "enemy_Castle";
    string target_tag = "enemy";
    LayerMask mask;

    public Slider health_ui;

    public float target_finder_raduis = 10.0f;
    public float lookat_Speed=10.0f;
    public Collider[] enemies;


    public float atackRate = 1.0f;
    private float nextAttack;


    private Vector3 directionOfCharacter;

    public float near_enemy_distance = 1.0f;
    bool attack = false;
    bool dead = false;

    GameObject current_traget;
    void Start()
    {
        mask = LayerMask.GetMask("enemy");
        if (gameObject.tag == "enemy")
        {
            target_castle_tag = "own_Castle";
            mask = LayerMask.GetMask("own");
            target_tag = "own";
            transform.eulerAngles = new Vector3(0, -90, 0);
            gameObject.layer = LayerMask.NameToLayer("enemy");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("own");
            transform.eulerAngles = new Vector3(0, 90, 0);
        }
        enemyCastle = GameObject.Find(target_castle_tag);
    }

    void Update()
    {
        if (dead == false)
        {
            enemies = Physics.OverlapSphere(transform.position, target_finder_raduis, mask);
            if (enemies.Length > 0)
            {
                current_traget = enemies[0].gameObject;
                transform.LookAt(enemies[0].transform);
                float dist = Vector3.Distance(transform.position, enemies[0].transform.position);
                if (dist <= near_enemy_distance)
                {
                    attack = true;
                }
                else
                {
                    attack = false;
                }
            }
            else
            {
                transform.LookAt(enemyCastle.transform);

                current_traget = enemyCastle;
                float dist = Vector3.Distance(transform.position, enemyCastle.transform.position);
                if (dist <= near_enemy_distance)
                {
                    attack = true;
                }
                else
                {
                    attack = false;
                }
            }
            if (attack == false)
            {
                //path finding
                transform.position += transform.forward * speed_movment * Time.deltaTime;
            }
            else
            {
                if (Time.time > nextAttack)
                {
                   float rnd_dmg=  Random.Range(damage_power/1.2f, damage_power);
                    current_traget.SendMessage("ApplyDamage", damage_power);
                    Debug.Log("attack");
                    nextAttack = Time.time + atackRate;
                }
            }


            health_ui.value = health / 100.0f;


            if (health < 0)
            {
                dead = true;
                Debug.Log("Soldier dead");

                gameObject.tag = "dead";
                gameObject.layer = 0;
                Destroy(gameObject, 3);
            }
        }
        else
        {
            //play dead animation
        }
    }

    public void ApplyDamage(float damage)
    {
        this.health -= damage;
    }

}
