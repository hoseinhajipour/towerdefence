﻿using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowerGenarator : MonoBehaviour, IDragHandler, IEndDragHandler, IDropHandler
{

    public GameObject gameobject;

    CameraHandler cameraHandler;
    GameController gameController;
    newConnection netconnection;

    public GameObject Left_area;
    public GameObject right_area;

    public GameObject area_create;


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

        if(GameObject.Find("SocketIO") != null)
        {
            netconnection = GameObject.Find("SocketIO").GetComponent<newConnection>();
        }
        


        userInfo = GameObject.Find("AllCharacterInfo").GetComponent<UpdateController>();
        ch = userInfo.findCharacterInfo("Shower");
        current_level = PlayerPrefs.GetInt("Shower_level");
        createRate = ch.levels[current_level].create_rate;

        Debug.Log("Shower_level : "+current_level);
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
                wordPos = hit.point;

                GameObject clone = Instantiate(gameobject, wordPos, Quaternion.identity);
                clone.tag = gameController.i_am_a;

                if(netconnection != null)
                {
                    attack_info attack_Info_ = new attack_info();
                    attack_Info_.name = "Shower";
                    attack_Info_.tag = gameController.i_am_a;
                    attack_Info_.room_name = gameController.room_name;
                    attack_Info_.position = slashcheck(Round(wordPos.x, 4) + "," + Round(wordPos.y, 4) + "," + Round(wordPos.z, 4));
                    netconnection.attackReq(attack_Info_);
                }
            }
            
           
        }
        cameraHandler.enabled = true;
        area_create.SetActive(false);
    }
    public void directCreate(attack_info attack_Info_)
    {
        Vector3 new_pos = JsonToVec(attack_Info_.position);
        GameObject clone = Instantiate(gameobject, new_pos, Quaternion.identity);
        clone.tag = attack_Info_.tag;
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
        string[] newS = Regex.Split(target, ",");
        newvector = new Vector3(float.Parse(newS[0]), float.Parse(newS[1]), float.Parse(newS[2]));
        return newvector;
    }
    public string slashcheck(string str)
    {
        return str.Replace('/', '.');
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
