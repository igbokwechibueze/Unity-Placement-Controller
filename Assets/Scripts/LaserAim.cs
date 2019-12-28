using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that shoots out a Raycasted Laser to determine if the GameObject is aiming at an object with a health script.
/// </summary>
public class LaserAim : MonoBehaviour
{
    public float laserRange;

    [HideInInspector]
    public bool foundObject = false;

    public Transform rayOrigin;

    public bool useLineRenderer;
    LineRenderer lineRenderer;
    public Gradient redColor;
    public Gradient greenColor;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForObject();
        if (useLineRenderer == false)
        {
            gameObject.GetComponentInChildren<LineRenderer>().enabled = false;
        }else
        {
            gameObject.GetComponentInChildren<LineRenderer>().enabled = true;
        }
    }

    void CheckForObject()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out hitInfo, laserRange))
        {
            if (hitInfo.collider != null)
            {
             //  Debug.Log(hitInfo.transform.name);
               Debug.DrawLine(rayOrigin.position, hitInfo.point, Color.red);
               lineRenderer.SetPosition(1, hitInfo.point);
               lineRenderer.colorGradient = redColor;

               ObjectHealth objectHealth = hitInfo.transform.GetComponent<ObjectHealth>();
              // CharacterHealth characterHealth = hitInfo.transform.GetComponent<CharacterHealth>();

               if (objectHealth != null /* || characterHealth != null */)
               {
                  //  Debug.Log("ObjectSeen");
                    foundObject = true;                
               }    else
                    {
                        foundObject = false;
                     //   Debug.Log("WrongObject");
                    }
            }
        }else
            {
                Debug.DrawLine(rayOrigin.position, rayOrigin.position + rayOrigin.forward * laserRange, Color.green);
                foundObject = false;
               // Debug.Log("No hit");
                lineRenderer.SetPosition(1, rayOrigin.position + rayOrigin.forward * laserRange);
                lineRenderer.colorGradient = greenColor;                
            }

        lineRenderer.SetPosition(0, rayOrigin.transform.position);
    }    
}
