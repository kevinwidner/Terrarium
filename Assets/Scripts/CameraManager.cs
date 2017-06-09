using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

    private Transform m_target;
    public Vector3 cameraOffset;

    public Transform Target
    {
        get
        {
            return m_target;
        }
    }

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (m_target == null)
            return;

        this.transform.position = m_target.position;
        this.transform.rotation = Quaternion.LookRotation(m_target.forward);

        Transform cameraTransform = GetComponentInChildren<Camera>().transform;
//        Debug.Log(string.Format("cameraRig position: {0}", this.transform.position));
//        Debug.Log(string.Format("cameraTransform position: {0}", cameraTransform.position));

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            cameraTransform.localPosition = new Vector3(0, cameraTransform.position.y - .5f, cameraTransform.position.z + .5f);
            Debug.Log("ScrollWheel went UP");
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            //cameraTransform.localPosition = new Vector3(0, cameraTransform.position.y + .5f, cameraTransform.position.z - .5f);
            Debug.Log("ScrollWheel went DOWN");
        }
	}

    public void SetTarget(Transform target)
    {
        m_target = target;
    }
}
