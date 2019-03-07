﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shower : MonoBehaviour
{
    public float health = 100;
    public float damage_power = 10.0f;
    public float speed_shoot = 1.0f;
    public float raduis_cover = 1.0f;
    Transform head;

    public float lookat_Speed;
    private Collider[] enemies;

    public float fireRate = 1.0f;
    private float nextFire;
    public Rigidbody bullet;
    private Transform fire_pos;
    public float bullet_speed = 2000.0f;

    string target_tag = "enemy";
    LayerMask mask;
    public Slider health_ui;
    bool dead = false;

    public AudioClip attack_sound;
    public AudioClip dead_sound;
    AudioSource audio;

    void Start()
    {
        head = transform.Find("head").transform;
        fire_pos = head.Find("fire_pos").transform;
        if (gameObject.tag == "enemy")
        {
            target_tag = "own";
            mask = LayerMask.GetMask("own");
            gameObject.layer = LayerMask.NameToLayer("enemy");
        }
        else
        {
            target_tag = "enemy";
            mask = LayerMask.GetMask("enemy");
            gameObject.layer = LayerMask.NameToLayer("own");
        }

        audio = gameObject.GetComponent<AudioSource>();
    }


    void Update()
    {
        if (dead == false)
        {
            enemies = Physics.OverlapSphere(transform.position, raduis_cover, mask);
            if (enemies.Length > 0)
            {
                Transform Target = enemies[0].transform;
                Quaternion OriginalRot = head.rotation;
                head.LookAt(Target);
                Quaternion NewRot = head.rotation;
                head.rotation = OriginalRot;
                head.rotation = Quaternion.Lerp(head.rotation, NewRot, lookat_Speed * Time.deltaTime);

                if (Time.time > nextFire)
                {
                    audio.clip = attack_sound;
                    audio.Play();

                    Rigidbody bullet_clone = Instantiate(bullet, fire_pos.position, fire_pos.rotation) as Rigidbody;
                    bullet_clone.GetComponent<bullet>().parnet_target_tag = target_tag;
                    bullet_clone.GetComponent<bullet>().damage_power = damage_power;
                    bullet_clone.AddForce(fire_pos.forward * bullet_speed);
                    nextFire = Time.time + fireRate;
                }

            }

            health_ui.value = health / 100.0f;
            if (health < 0)
            {
                dead = true;
                gameObject.tag = "dead";
                gameObject.layer = 0;
                Debug.Log("shower dead");
                Destroy(gameObject, 3);

                audio.clip = dead_sound;
                audio.Play();
            }
        }
        else
        {

        }

    }


    public void ApplyDamage(float damage)
    {
        this.health -= damage;
    }
}
