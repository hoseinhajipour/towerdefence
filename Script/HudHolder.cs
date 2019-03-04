using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudHolder : MonoBehaviour
{
	Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;    
    }
	public GameObject ui;
	
    // Update is called once per frame
    void Update()
    {
        Vector3 namepos= cam.WorldToScreenPoint(this.transform.position);
		ui.transform.position=namepos;
    }
}
