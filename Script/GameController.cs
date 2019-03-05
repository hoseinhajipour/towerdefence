using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class GameController : MonoBehaviour
{
    public GameObject win;
    public GameObject Defeat;
    public Text player_name;
    public Text enemy_name;
    public GameObject find_player_match;
    public string i_am_a="own";
    public Text youare;


    public PlayerInfo own_info;
    public PlayerInfo enemy_info;

    public string room_name = "";

    void Start()
    {
        /*
        if(PlayerPrefs.GetString("username") != null)
        {
            string username_ = PlayerPrefs.GetString("username");
            player_name.text = username_;
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        youare.text = i_am_a;
    }

    public void showWinPanel()
    {
        win.SetActive(true);
    }
    public void showDefeatPanel()
    {
        Defeat.SetActive(true);
    }
    public void setEnemyInfo(PlayerInfo info)
    {
        enemy_name.text = info.name;
    }

    public void show_find_player_match_panel()
    {
        find_player_match.SetActive(true);
    }
    public void hide_find_player_match_panel()
    {
        find_player_match.SetActive(false);
    }

}
