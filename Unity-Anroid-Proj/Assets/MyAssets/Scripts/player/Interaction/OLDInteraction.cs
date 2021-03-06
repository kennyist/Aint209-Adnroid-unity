﻿using UnityEngine;
using System.Collections;

/*        
        Player Interaction / Interactable
        Author: Tristan 'Kennyist' Cunningham - www.tristanjc.com
        Date: 31/01/2014
        License: Creative Commons ShareAlike 3.0 - https://creativecommons.org/licenses/by-sa/3.0/
*/


/* ------------------ In development ---------------- */
// Early version
/*
public class OLDInteraction : MonoBehaviour {
    public enum Type { Interactable, Caster }

    [System.Serializable]
    public class Caster
    {
        public LayerMask layerMask;
        public Transform rayStartLocation;
        public float castDistance = 1f;
    }

    [System.Serializable]
    public class Interactable
    {
        public enum Type { Press, HoldForTime, HoldForTimeSlowReset }
        public Type type;
        public float holdTime = 3f;
        public string InputButton = "f";
        public float slowResetMultiplier = 0.1f;
    }

    public Type type;
    public Caster caster = new Caster();
    public Interactable interactable = new Interactable();

    private GameObject hitOBJ;
    private GameObject lastOBJ;
    private bool isHit;
    private RaycastHit hit;
    private float holdTime;

    void start()
    {
        hitOBJ = null;
        holdTime = 0.0f;
    }
	
	void Update () {
        if (type == Type.Caster)
        {
            CastRay();
        }
        else
        {
            if ((interactable.type == Interactable.Type.HoldForTime || interactable.type == Interactable.Type.HoldForTimeSlowReset) && holdTime >= interactable.holdTime)
            {
                gameObject.SendMessage("InteractableComplete", SendMessageOptions.DontRequireReceiver);
            }
            else if (interactable.type == Interactable.Type.Press && isHit && Input.GetKeyDown(interactable.InputButton))
            {
                gameObject.SendMessage("InteractableComplete", SendMessageOptions.DontRequireReceiver);
            }

            if (isHit && Input.GetKey(interactable.InputButton))
            {
                holdTime += Time.deltaTime;

                if (holdTime > interactable.holdTime)
                {
                    holdTime = interactable.holdTime;
                }
            }
            else
            {
                if (interactable.type != Interactable.Type.HoldForTimeSlowReset)
                {
                    holdTime = 0.0f;
                }
                else
                {
                    if (holdTime > 0.0f)
                    {
                        holdTime -= Time.deltaTime * interactable.slowResetMultiplier;
                    }
                }
            }
        }
	}

    void InteractionHit(bool hit)
    {
        isHit = hit;
    }

    void CastRay()
    {

        if (Physics.Raycast(caster.rayStartLocation.position, caster.rayStartLocation.forward, out hit, caster.castDistance, caster.layerMask))
        {
            hitOBJ = hit.collider.gameObject;

            if (lastOBJ != null)
            {
                if (hitOBJ != lastOBJ)
                {
                    Debug.Log("not equal");
                    lastOBJ.SendMessage("InteractionHit", false, SendMessageOptions.DontRequireReceiver);
                    lastOBJ = null;
                }
            }

            if (hitOBJ.GetComponent<Interaction>() != null && hitOBJ != gameObject)
            {
                if (hitOBJ.GetComponent<Interaction>().type == Type.Interactable)
                {
                    hitOBJ.SendMessage("InteractionHit", true, SendMessageOptions.DontRequireReceiver);
                }
            }
            else
            {
                hitOBJ.SendMessage("InteractionHit", false, SendMessageOptions.DontRequireReceiver);
                hitOBJ = null;
            }
        }
        else
        {
            if (lastOBJ != null)
            {
                lastOBJ.SendMessage("InteractionHit", false, SendMessageOptions.DontRequireReceiver);
                lastOBJ = null;
            }
        }

        if (hitOBJ != null)
        {
            lastOBJ = hitOBJ;
        }
    }

    /// <summary>
    /// Get the time the button has been held for.
    /// </summary>
    /// <returns>Time button has been held for while on this object</returns>
    public float CurrentHeldTime()
    {
        return holdTime;
    }

    /// <summary>
    /// Get the total hold time needed
    /// </summary>
    /// <returns>Total hold time needed</returns>
    public float HoldTime()
    {
        return interactable.holdTime;
    }


    /// <summary>
    /// Get the input button needed
    /// </summary>
    /// <returns>Input button</returns>
    public string InputButton()
    {
        return interactable.InputButton;
    }


    /// <summary>
    /// Get the interaction type for this object
    /// </summary>
    /// <returns>Interaction type</returns>
    public Interactable.Type InteractionType()
    {
        return interactable.type;
    }

}
*/