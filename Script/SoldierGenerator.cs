using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SoldierGenerator : MonoBehaviour, IDragHandler,IEndDragHandler,IDropHandler
{

    public GameObject gameobject;

    CameraHandler cameraHandler;
    private void Start()
    {
        cameraHandler = GameObject.Find("Main Camera").GetComponent<CameraHandler>();
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
            Instantiate(gameobject, wordPos, Quaternion.identity);
        }
        cameraHandler.enabled = true;
    }    

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;
    }
}
