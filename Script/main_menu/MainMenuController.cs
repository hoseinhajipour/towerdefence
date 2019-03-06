using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    public InputField username;
    public Text coin_area;
    void Start()
    {
        if (username != null)
        {
            string username_ = PlayerPrefs.GetString("username");
            username.text = username_;
        }

        if(coin_area != null)
        {
            coin_area.text = PlayerPrefs.GetInt("coin").ToString();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void onPlayGame()
    {
        string username_ = username.text;
        PlayerPrefs.SetString("username", username_);
        PlayerPrefs.Save();

        SceneManager.LoadScene("main");
    }

    public void LoadShopBtn()
    {
        SceneManager.LoadScene("shop");
    }

    public void LoadupdatesBtn()
    {
        SceneManager.LoadScene("updates");
    }
    public void LoadMenuBtn()
    {
        SceneManager.LoadScene("menu");
    }
}
