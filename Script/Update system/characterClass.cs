using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class level
{
    public int num;
    public float create_rate;
    public int attack_power;
    public float move_speed;
    public float attack_rate_per_second;
    public int Health;
    public int price;
}

[System.Serializable]
public class characterClass
{
    public string name;
    public Sprite icon;
    public string description;
    public level[] levels;
}
