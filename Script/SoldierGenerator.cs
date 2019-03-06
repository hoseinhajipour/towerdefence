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
    private void Start()
    {
        cameraHandler = GameObject.Find("Main Camera").GetComponent<CameraHandler>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        netconnection = GameObject.Find("SocketIO").GetComponent<newConnection>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        cameraHandler.enabled = false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 wordPos;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                wordPos = hit.point;
            }
            else
            {
                wordPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            GameObject clone= Instantiate(gameobject, wordPos, Quaternion.identity);
            clone.tag = gameController.i_am_a;


            attack_info attack_Info_ = new attack_info();
            attack_Info_.name = "Soldier";
            attack_Info_.tag = gameController.i_am_a;
            attack_Info_.room_name = gameController.room_name;
            attack_Info_.position = slashcheck(Round(wordPos.x, 4) + "," + Round(wordPos.y, 4) + "," + Round(wordPos.z, 4));

            netconnection.attackReq(attack_Info_);
        }
        cameraHandler.enabled = true;
    }    

    public void directCreate(attack_info attack_Info_)
    {
        Vector2 new_pos= JsonToVec(attack_Info_.position);
        Debug.Log(new_pos);
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
        return str.Replace('/','.');
    }
}
