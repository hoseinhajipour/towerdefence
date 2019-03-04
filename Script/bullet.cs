using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    // Start is called before the first frame update

    public string parnet_target_tag = "enemy";
    public float damage_power=10.0f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag== parnet_target_tag)
        {
            float rnd_dmg = Random.Range(damage_power / 1.2f, damage_power);
            collision.gameObject.SendMessage("ApplyDamage", damage_power);
        }

        Destroy(gameObject);
    }
}
