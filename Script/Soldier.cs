using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{

    public int health = 100;
    public float speed_movment = 1.0f;
    public float damage_power = 10.0f;


    GameObject enemyCastle;
    void Start()
    {
        enemyCastle = GameObject.FindGameObjectWithTag("enemy_Castle");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed_movment * Time.deltaTime, 0, 0);
    }
}
