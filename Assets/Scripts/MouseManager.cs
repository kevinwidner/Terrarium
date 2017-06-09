using UnityEngine;
using System.Collections;
using System;

public class MouseManager : MonoBehaviour {

    spider selectedSpider;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetMouseButtonUp(0)) // Left Button
        {
            SelectInsect();
        }
        else if (Input.GetMouseButtonUp(1)) // Right Button
        {
            SelectTargetPosition();
        }
	}

    void SelectInsect()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        LayerMask mask = 1 << 8;  //use this to determine what you want/don't want to collide with.

        Debug.DrawLine(ray.origin, ray.GetPoint(200), Color.blue);

        if (Physics.Raycast(ray, out hit, 200, mask))
        {
            spider collidedSpider = hit.collider.GetComponent<spider>();
            if (collidedSpider != null && collidedSpider.Alive)
            {
                spider[] spiders = GameObject.FindObjectsOfType<spider>();

                foreach (spider thisSpider in spiders)
                {
                    thisSpider.Selected = (thisSpider == collidedSpider) && !collidedSpider.Selected;
                }

                selectedSpider = Array.Find<spider>(spiders, s => s.Selected);
            }
        }
    }

    void SelectTargetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        LayerMask mask = 1 << 9;  //use this to determine what you want/don't want to collide with.

        Debug.DrawLine(ray.origin, ray.GetPoint(200), Color.green);

        if (Physics.Raycast(ray, out hit, 200, mask) && selectedSpider != null)
        {
            selectedSpider.SetTargetPosition(hit.point);
        }
    }

}
