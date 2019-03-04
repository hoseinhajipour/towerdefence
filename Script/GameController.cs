using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject win;
    public GameObject Defeat;
    public Text player_name;
    public Text enemy_name;

    void Start()
    {
        if(PlayerPrefs.GetString("username") != null)
        {
            string username_ = PlayerPrefs.GetString("username");
            player_name.text = username_;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
