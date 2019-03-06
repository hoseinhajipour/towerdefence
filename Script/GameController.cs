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
   


    public PlayerInfo own_info;
    public PlayerInfo enemy_info;

    public string room_name = "";

    public int total_kill = 0;
    public int total_Coin = 0;

    public Text total_kill_text;
    public Text total_Coin_text;

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
        total_kill_text.text = total_kill.ToString();
        total_Coin_text.text = total_Coin.ToString();
    }

    public void showWinPanel()
    {
        saveGameResult();
        win.SetActive(true);
    }
    public void showDefeatPanel()
    {
        saveGameResult();
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

    public void saveGameResult()
    {
        int coin = PlayerPrefs.GetInt("coin");
        coin += total_Coin;
        PlayerPrefs.SetInt("coin", coin);

        int total_kill_ = PlayerPrefs.GetInt("total_kill");
        total_kill_ += total_kill;
        PlayerPrefs.SetInt("total_kill", total_kill_);
    }
}
