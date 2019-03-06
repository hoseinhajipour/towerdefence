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
    newConnection netconnection;

    public int coin_for_dead_me = 10;
    void Start()
    {
        //    gameController = GameObject.Find("GameController").GetComponent<GameController>();
        netconnection = GameObject.Find("SocketIO").GetComponent<newConnection>();
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
                end_game_save();
                netconnection.do_end_ballte();
            }
        }else{
            
        }
    }

    public void ApplyDamage(float damage)
    {
        this.health -= damage;
    }

    public void end_game_save()
    {
        GameController gb = GameObject.Find("GameController").GetComponent<GameController>();
        gb.total_kill += 1;

        if (gameObject.tag == gb.i_am_a)
        {
            gb.total_Coin += coin_for_dead_me;
        }

        gb.saveGameResult();
    }
}
