using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shower : MonoBehaviour
{
    public int health = 100;
    public float damage_power = 10.0f;
    public float speed_shoot = 1.0f;
    public float raduis_cover = 1.0f;
    public LayerMask mask;


    Transform head;

    public float lookat_Speed;
    private Collider[] enemies;

    public float fireRate = 1.0f;
    private float nextFire;
    public Rigidbody bullet;
    private Transform fire_pos;
    public float bullet_speed = 2000.0f;
    void Start()
    {
        head = transform.Find("head").transform;
        fire_pos = head.Find("fire_pos").transform;
    }


    void Update()
    {
        enemies = Physics.OverlapSphere(transform.position, raduis_cover, mask);
        foreach (Collider enemy in enemies)
        {
            Debug.Log(enemy.name);
        }
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
                Rigidbody bullet_clone =Instantiate(bullet, fire_pos.position, fire_pos.rotation) as Rigidbody;
                bullet_clone.AddForce(fire_pos.forward * bullet_speed);
                nextFire = Time.time + fireRate;
            }

        }


    }
}
