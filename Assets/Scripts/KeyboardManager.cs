using UnityEngine;
using System.Collections;
using System;

public class KeyboardManager : MonoBehaviour {
    public GameObject GameCamera;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CamManager cameraManager = GameCamera.GetComponent<CamManager>();

            Transform cameraTarget = cameraManager.Target;

            int index;
            spider[] insects = GameObject.FindObjectsOfType<spider>();

            if (cameraTarget == null)
            {
                index = 0;
            }
            else
            {
                index = Array.FindIndex(insects, insect => insect.transform == cameraTarget);
                index++;
                if (index == insects.Length)
                    index = 0;
            }

            cameraManager.SetTarget(insects[index].transform);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleColliderVisible();
        }
	}

    void ToggleColliderVisible()
    {
        spider selectedSpider = GetSelectedSpider();
        if (selectedSpider != null)
        {
            Renderer[] childRenderers = selectedSpider.GetComponentsInChildren<Renderer>(true);

            foreach (var renderer in childRenderers)
            {
                if (renderer.name == "AttackRadius")
                    renderer.enabled = !renderer.enabled;

                if (renderer.name == "VisibilityRadius")
                    renderer.enabled = !renderer.enabled;
            }
        }
    }

    spider GetSelectedSpider()
    {
        spider[] spiders = GameObject.FindObjectsOfType<spider>();
        foreach (spider thisSpider in spiders)
        {
            if (thisSpider.Selected)
            {
                return thisSpider;
            }
        }
        return null;
    }

}
