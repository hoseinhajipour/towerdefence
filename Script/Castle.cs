using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
    public float health = 100;

    bool dead = false;
    public Slider health_ui;
    public GameController gameController;

    void Start()
    {
    //    gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void Update()
    {
        if (dead == false)
        {

            health_ui.value = health / 100.0f;
            if (health < 0)
            {
                dead = true;
                Debug.Log("Castle dead");
                //   Destroy(gameObject, 3);

                if (gameObject.tag == "enemy")
                {
                    gameController.showWinPanel();
                }
                else
                {
                    gameController.showDefeatPanel();
                }
            }
        }else{

        }
    }

    public void ApplyDamage(float damage)
    {
        this.health -= damage;
    }
}
