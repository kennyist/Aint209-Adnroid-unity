using UnityEngine;
using System.Collections;

public class ThirdPersonCameraCollision : MonoBehaviour {

    [System.Serializable]
    public class MeshHiding
    {
        //public bool enableMeshHiding = true;
        public float meshHideDistance = 0.5f;
        public GameObject meshObject;
        public bool hideAllChildren = true;
    }
     
    public float scrollSpeed = 1f;
    public float limitOuter = 15f;
    public MeshHiding meshHiding = new MeshHiding();

    private float setDistance;
    private ThirdPersonCamera tpc;
    private Component[] rends;

    void Start()
    {
        
        tpc = gameObject.GetComponent<ThirdPersonCamera>();
    }

	void Update () {
        bool scrollingCollisionCheck = false;
        float camDist;
        Vector3 castDirection;
        RaycastHit playerToCameraHit;
        float bufferDistance = limitOuter;

        for(int i = 0; i < tpc.cameraColliders.Length; i++){
            camDist = Vector3.Distance(tpc.rotationObject.transform.position, tpc.cameraColliders[i].transform.position);
            castDirection = tpc.cameraColliders[i].transform.position - tpc.rotationObject.transform.position;
            Debug.DrawRay(tpc.rotationObject.transform.position, castDirection, Color.red);
            if (Physics.Raycast(tpc.rotationObject.transform.position, castDirection,out playerToCameraHit, limitOuter, tpc.collisionLayerMask))
            {
                if (bufferDistance > playerToCameraHit.distance)
                {
                    bufferDistance = playerToCameraHit.distance;
                }
                scrollingCollisionCheck = true;
            }
        }

        if (scrollingCollisionCheck && bufferDistance < tpc.preferredDistance)
        {
            setDistance = bufferDistance;
        }
        else
        {
            setDistance = tpc.preferredDistance;
        }

        tpc.scroller.transform.localPosition = new Vector3(tpc.scroller.transform.localPosition.x,tpc.scroller.transform.localPosition.y,Mathf.Lerp(Mathf.Abs(tpc.scroller.transform.localPosition.z), setDistance, scrollSpeed) * -1);

        if (tpc.scroller.transform.localPosition.z > (meshHiding.meshHideDistance * -1))
        {
            if (meshHiding.hideAllChildren)
            {
                rends = meshHiding.meshObject.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < rends.Length; i++)
                {
                    rends[i].renderer.enabled = false;
                }
            }
            else
            {
                meshHiding.meshObject.GetComponent<Renderer>().enabled = false;
            }
        }
        else
        {
            if (meshHiding.hideAllChildren)
            {
                rends = meshHiding.meshObject.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < rends.Length; i++)
                {
                    rends[i].renderer.enabled = true;
                }
            }
            else
            {
                meshHiding.meshObject.GetComponent<Renderer>().enabled = true;
            }
        }
	}
}
