using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrowdGenerator : MonoBehaviour
{
    public List<GameObject> gameobjects;
    public GameObject right_area;


    public float createRate = 4.0f;
    private float nextCreate;

    public int Create_count=0;
    public int master_level = 1;

    void Update()
    {

        if (Time.time > nextCreate)
        {
            GameObject gameobject = gameobjects[Random.Range(0, gameobjects.Capacity)];

            Vector3 rndPosWithin;
            rndPosWithin = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            rndPosWithin = right_area.transform.TransformPoint(rndPosWithin * .5f);



            GameObject clone = Instantiate(gameobject, rndPosWithin, Quaternion.identity);
            clone.tag = "enemy";
            clone.name = gameobject.name + "_" + "enemy_" + Create_count;
            if(gameobject.name == "Soldier")
            {
                clone.GetComponent<Soldier>().current_level = master_level;
            }
            else if (gameobject.name == "Shower")
            {
                clone.GetComponent<Shower>().current_level = master_level;
            }


            nextCreate = Time.time + createRate;
        }
        
    }
}
