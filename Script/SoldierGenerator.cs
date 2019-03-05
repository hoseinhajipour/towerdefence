using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
            attack_Info_.pos_x = wordPos.x.ToString();
            attack_Info_.pos_y = wordPos.y.ToString();
            attack_Info_.pos_z = wordPos.z.ToString();
            netconnection.attackReq(attack_Info_);
        }
        cameraHandler.enabled = true;
    }    

    public void directCreate(attack_info attack_Info_)
    {
        
        Vector3 wordPos=new Vector3(
            float.Parse(attack_Info_.pos_x),
            float.Parse(attack_Info_.pos_y),
            float.Parse(attack_Info_.pos_z)
            );
        GameObject clone = Instantiate(gameobject, wordPos, Quaternion.identity);
        clone.tag = attack_Info_.tag;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;
    }
}
