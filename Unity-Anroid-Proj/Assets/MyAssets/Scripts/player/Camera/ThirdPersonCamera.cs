using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour {

    private iniParser parser = new iniParser();

    public LayerMask collisionLayerMask;
    public GameObject player;
    public GameObject locationAdjustment;
    public GameObject scroller;
    public GameObject cam;
    public GameObject rotationObject;
    public GameObject straferObject;
    public GameObject[] cameraColliders;
    
    [System.Serializable]
    public class TPcameraColliders{
        public GameObject castUpLocation;
        public GameObject castDownLocation;
        public GameObject castLeftLocation;
        public GameObject castRightLocation;
    }

    [System.Serializable]
    public class TPsmooth
    {
        public float Location = 15f;
        public float Rotation = 5f;
    }

    [System.Serializable]
    public class TPhorizontal{
        public float mouseSensitivity = 15f;
        public float minimumAngle = -360;
        public float maximumAngle = 360;
    }

    [System.Serializable]
    public class TPvertical
    {
        public float mouseSensitivity = 15f;
        public float minimumAngle = -60;
        public float maximumAngle = 60;
    }

    [HideInInspector]
    TPcameraColliders CamColliders = new TPcameraColliders();

    public TPsmooth smooth = new TPsmooth();
    public TPhorizontal horizontal = new TPhorizontal();
    public TPvertical vertical = new TPvertical();
    public float preferredDistance = 2;

    private float localRotationZ = 0;
    private float vRotation;
    private float hRotation;
    private bool cameraCollisionCheck;
    private bool activateFPS = false;
    private float smoothDampVelocity;

	// Use this for initialization
    void Start()
    {
        if (!networkView.isMine)
        {
            gameObject.SetActive(false);
            enabled = false;
        }
        parser.Load(IniFiles.CONFIG);

        // Initialize camera rotation 

        vRotation = rotationObject.transform.eulerAngles.x;
        hRotation = rotationObject.transform.eulerAngles.y;

        // Set the vertical rotation of the camera to the average of the vertical limits.
        rotationObject.transform.localEulerAngles = new Vector3((vertical.maximumAngle + vertical.maximumAngle) / 2, (horizontal.minimumAngle + horizontal.maximumAngle) / 2, rotationObject.transform.localEulerAngles.z);

        // Set the initial distance from the camera based on the preferred distance
        scroller.transform.localPosition = new Vector3(scroller.transform.localPosition.x, scroller.transform.localPosition.y, preferredDistance * -1);
    }

    void LateUpdate()
    {
        //scroller.transform.localPosition = new Vector3(scroller.transform.localPosition.x, scroller.transform.localPosition.y, preferredDistance * -1);
        // Follow the Character position with smoothing.
        transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime * smooth.Location);
    }

    void Update()
    {
        //Determine Camera Rotation
        AdjustVertical();
        AdjustHorizontal();

        straferObject.transform.localPosition = Vector3.Lerp(straferObject.transform.localPosition, player.transform.TransformDirection(0.4f, -0f, 0), Time.deltaTime * 5);

        if (Input.GetMouseButtonDown(1) || Input.GetButtonDown("R3"))
        {
            preferredDistance = preferredDistance == 0.6f ? 2 : 0.6f;
        }
    }

    float AngleClamp(float rotation, float min, float max)
    {
        if (rotation < -360)
        {
            rotation = rotation + 360;
        }
        if (rotation > 360)
        {
            rotation = rotation - 360;
        }
        rotation = Mathf.Clamp(rotation, min, max);
        return rotation;
    }

    void AdjustHorizontal()
    {
        if (bool.Parse(parser.Get("input_controller")))
        {
            hRotation += -Input.GetAxis("win R horizontal") * horizontal.mouseSensitivity;
        }
        else
        {
            hRotation += Input.GetAxis("Mouse X") * horizontal.mouseSensitivity;
        }
        hRotation = AngleClamp(hRotation, horizontal.minimumAngle, horizontal.maximumAngle);
        rotationObject.transform.localEulerAngles = new Vector3(rotationObject.transform.localEulerAngles.x, hRotation, rotationObject.transform.localEulerAngles.z);
    }

    void AdjustVertical()
    {
        if (bool.Parse(parser.Get("input_controller")))
        {
            vRotation = vRotation - Input.GetAxis("win R vertical") * vertical.mouseSensitivity;
        }
        else
        {
            vRotation = vRotation - -Input.GetAxis("Mouse Y") * vertical.mouseSensitivity;
        }
        vRotation = AngleClamp(vRotation, vertical.minimumAngle, vertical.maximumAngle);

        rotationObject.transform.localEulerAngles = new Vector3(vRotation, rotationObject.transform.localEulerAngles.y, rotationObject.transform.localEulerAngles.z);
    }

    bool TestLOS(LayerMask LOSLayer)
    {
        Vector3 LOSCastDirection = rotationObject.transform.position - cam.transform.position;
        if (Physics.Raycast(cam.transform.position, LOSCastDirection, LOSCastDirection.magnitude, LOSLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    float GetLocalRotationZ()
    {
        return localRotationZ;
    }

    void SetLocalRotationZ(float rotation)
    {
        localRotationZ = rotation;
    }

    void SetHorizontalRotation(float rotation)
    {
        hRotation = rotation;
    }

    void SetVeritcalRotation(float rotation)
    {
        vRotation = rotation;
    }

}
