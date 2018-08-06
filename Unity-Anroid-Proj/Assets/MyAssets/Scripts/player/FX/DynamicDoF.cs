using UnityEngine;
using System.Collections;

public class DynamicDoF : MonoBehaviour {

    iniParser parser = new iniParser();

    public LayerMask layerMask;
    public Transform rayCastObject;
    public GameObject dofFocusObject;
    public float maxDistance;

    DepthOfField34 DoF;

    float currentPos;
    float destination;
    float smoothTime;
    float addDistance;

    void Start()
    {
        DoF = gameObject.GetComponent<DepthOfField34>();
        Refresh();
    }

    void Refresh()
    {
        parser.Load(IniFiles.CONFIG);
        smoothTime = float.Parse(parser.Get("fx_dof_smoothtime"));
        addDistance = float.Parse(parser.Get("fx_dof_focaldistance"));
    }

	void Update () {
        RaycastHit hit;

        if (Input.GetKeyDown(KeyCode.F5)) { Refresh(); }

        if (DoF != null)
        {
            if (Physics.Raycast(rayCastObject.transform.position, rayCastObject.transform.forward, out hit, maxDistance, layerMask))
            {
                dofFocusObject.transform.position = hit.point;
                //Debug.DrawRay(rayCastObject.transform.position, hit.point, Color.green);
            }
            else
            {
                dofFocusObject.transform.position = rayCastObject.transform.forward * maxDistance;
                //Debug.DrawRay(rayCastObject.transform.position, rayCastObject.transform.forward * maxDistance, Color.green);
            }

            destination = Vector3.Distance(rayCastObject.transform.position, dofFocusObject.transform.position);
            currentPos = Mathf.Lerp(currentPos, destination + addDistance, smoothTime * Time.deltaTime);

            DoF.focalPoint = currentPos;
        }
	}
}
