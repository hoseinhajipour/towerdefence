using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Text.RegularExpressions;

public class SoldierGenerator : MonoBehaviour, IDragHandler,IEndDragHandler,IDropHandler
{

    public GameObject gameobject;

    CameraHandler cameraHandler;
    GameController gameController;
    newConnection netconnection;

    public GameObject Left_area;
    public GameObject right_area;

    public GameObject area_create;

    public int Soldier_count = 0;

    private UpdateController userInfo;
    private characterClass ch;
    private int current_level;

    public bool canCreate = false;
    public float createRate = 3.0f;
    private float nextCreate;

    private void Start()
    {
        cameraHandler = GameObject.Find("Main Camera").GetComponent<CameraHandler>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();

        if (GameObject.Find("SocketIO") != null)
        {
            netconnection = GameObject.Find("SocketIO").GetComponent<newConnection>();
        }

        userInfo = GameObject.Find("AllCharacterInfo").GetComponent<UpdateController>();
        ch = userInfo.findCharacterInfo("Soldier");
        current_level = PlayerPrefs.GetInt("Soldier_level");
        createRate = ch.levels[current_level].create_rate;

        Debug.Log("Soldier_level : " + current_level);
    }
    void Update()
    {
        if (Time.time > nextCreate)
        {
            canCreate = true;
            nextCreate = Time.time + createRate;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canCreate)
        {
            setCreate_side();
            transform.position = Input.mousePosition;
            cameraHandler.enabled = false;
            area_create.SetActive(true);
        }
        else
        {
            Debug.Log("you can't create");
        }
        
    }


    public void OnDrop(PointerEventData eventData)
    {
        
        if (Input.GetMouseButtonUp(0))
        {
            canCreate = false;
            Vector3 wordPos;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int layer_mask = LayerMask.GetMask("area_create");
            if (Physics.Raycast(ray, out hit, 1000f, layer_mask))
            {
                Debug.Log("hit");
                wordPos = hit.point;

                GameObject clone = Instantiate(gameobject, wordPos, Quaternion.identity);
                clone.tag = gameController.i_am_a;
                clone.name = "Soldier_"+gameController.i_am_a+"_"+ Soldier_count;
                clone.GetComponent<Soldier>().current_level = current_level;

                attack_info attack_Info_ = new attack_info();
                attack_Info_.name = "Soldier";
                attack_Info_.object_name = clone.name;
                attack_Info_.tag = gameController.i_am_a;
                attack_Info_.room_name = gameController.room_name;
                attack_Info_.level = current_level;


                Debug.Log("send : " + wordPos.x + "," + wordPos.y + "," + wordPos.z);
                attack_Info_.position = wordPos.x.ToString("F4") + "," + wordPos.y.ToString("F4") + "," + wordPos.z.ToString("F4");
                if (netconnection != null)
                {
                    netconnection.attackReq(attack_Info_);
                }
                Soldier_count++;
            }
        }
        cameraHandler.enabled = true;
        area_create.SetActive(false);
    }    

    public void directCreate(attack_info attack_Info_)
    {
        Vector3 new_pos= JsonToVec(attack_Info_.position);
        Debug.Log(new_pos);
        GameObject clone = Instantiate(gameobject, new_pos, Quaternion.identity);
        clone.name = attack_Info_.object_name;
        clone.tag = attack_Info_.tag;
        clone.GetComponent<Soldier>().current_level = attack_Info_.level;


    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;
    }
    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }

    public Vector3 JsonToVec(string target)
    {
        Vector3 newvector;
        string[] newS = target.Split(new string[] { ","}, StringSplitOptions.None);
        newvector = new Vector3(
             Single.Parse(newS[0]),
              Single.Parse(newS[1]),
               Single.Parse(newS[2])
            );
        return newvector;
    }
    
    public string slashcheck(string str)
    {
        return str.Replace('/','.');
    }

    public void setCreate_side()
    {
        if (gameController.i_am_a == "own")
        {
            area_create = Left_area;
        }
        else
        {
            area_create = right_area;
        }
    }
}
