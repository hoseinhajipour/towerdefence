using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateController : MonoBehaviour
{
    public Text coin_text;
    public GameObject enoughPanel;
    int coin = 0;
    public string charcter_name;
    public int current_level = 0;
    public int next_price;

    public characterClass current_character_info;


    public Text text_level_number;
    public Text text_name;
    public Text text_description;
    public Text text_create_rate;
    public Text text_attack_power;
    public Text text_move_speed;
    public Text text_attack_rate_per_second;
    public Text Health;
    public Text update_btn;
    public Image character_icon;


    

    
    public characterClass[] characters;

    void Start()
    {
        coin = PlayerPrefs.GetInt("coin");
        coin_text.text = coin.ToString();

    }



    public void setCharacter(string name)
    {
        charcter_name = name;

       
        if (PlayerPrefs.GetInt(charcter_name + "_level") != null)
        {
            current_level = PlayerPrefs.GetInt(charcter_name + "_level");
        }
        else
        {
            current_level = 0;
        }
        Debug.Log(charcter_name + "_level = " + current_level);
        current_character_info = findCharacterInfo(charcter_name);
       
        
        text_level_number.text = current_level+"";
        text_name.text= current_character_info.name;
        text_description.text = current_character_info.description;
        text_create_rate.text = current_character_info.levels[current_level].create_rate.ToString();
        text_attack_power.text = current_character_info.levels[current_level].attack_power.ToString();
        text_move_speed.text = current_character_info.levels[current_level].move_speed.ToString();
        text_attack_rate_per_second.text = current_character_info.levels[current_level].attack_rate_per_second.ToString();
        Health.text = current_character_info.levels[current_level].Health.ToString();
        update_btn.text = "Update "+ current_character_info.levels[current_level].price.ToString();
        character_icon.overrideSprite = current_character_info.icon;
        next_price = current_character_info.levels[(current_level + 1)].price;
    }
    

    public void DoUpdateCharacter()
    {
        if(coin >= next_price)
        {
            current_level++;
            PlayerPrefs.SetInt(charcter_name + "_level", current_level);
            coin -= next_price;
            coin_text.text = coin.ToString();
            PlayerPrefs.SetInt("coin", coin);
        }
        else
        {
            Debug.Log("coin not enfoh");

            enoughPanel.SetActive(true);
        }
        

    }


    public characterClass findCharacterInfo(string name)
    {
        characterClass ch_ =new characterClass();
        for(int i = 0; i < characters.Length; i++)
        {
            if (characters[i].name == name)
            {
                ch_ = characters[i];
                break;
            }
        }

        return ch_;
    }
}
