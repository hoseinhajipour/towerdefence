using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    public InputField username;

    void Start()
    {
        string username_ = PlayerPrefs.GetString("username");
        username.text= username_;
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
}
