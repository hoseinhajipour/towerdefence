using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject win;
    public GameObject Defeat;


    void Start()
    {
        
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
}
