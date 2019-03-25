using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class characterInfo : MonoBehaviour
{

    public string charcter_name;
    public Text level_text;
    public int current_level = 0;

    UpdateController UpdateController_;

    void Start()
    {
        UpdateController_ = GameObject.Find("MainMenuController").GetComponent<UpdateController>();

        characterClass ch = UpdateController_.findCharacterInfo(name);
        current_level = PlayerPrefs.GetInt(charcter_name + "_level");
        level_text.text = current_level + " / " + ch.levels.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
