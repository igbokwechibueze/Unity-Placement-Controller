using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirDroneLauncher : MonoBehaviour
{
    [System.Serializable]
    public enum DefenceType
    {
        airDefence,
        surfaceDefence,
        allDefence
    }
    public DefenceType defenceType = DefenceType.airDefence;    
    AALauncher launcher;

    [HideInInspector]
    public Transform target;
    public float turnRate = 15.0f;
    public float speed = 50.0f;

    public RadarNew radarNew;
    
    /// <summary>
    /// Script that shoots out a Raycasted Laser to determine if the GameObject is aiming at an object with a health script.
    /// </summary>
    [Tooltip("Script that shoots out a Raycasted Laser to determine if the GameObject is aiming at an object with a health script.")]    
    LaserAim laserAim;    

    float wait;
    float startTime;

    void Start()
    {
        startTime = Time.time;
        wait = Random.Range(4.0f, 10.0f);

        launcher = GetComponentInChildren<AALauncher>(); 
        laserAim = GetComponentInChildren<LaserAim>();       
    }

    void FixedUpdate()
    {
        if (radarNew != null)
        {
            if (defenceType == DefenceType.airDefence)
            {
                target = radarNew.airTarget;
            }

            if (defenceType == DefenceType.surfaceDefence)
            {
                target = radarNew.surfaceTarget;
            }

            if (defenceType == DefenceType.allDefence)
            {
                target = radarNew.allTarget;
            }

            if (target)
           {
               if (Time.time - startTime > wait && laserAim.foundObject == true)
               {
                   // Fire weapon.
                   launcher.Launch(target);
               }
                // Look at the target.
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target.position - transform.position, transform.up), turnRate * Time.deltaTime);
            }   
        }else
            {
                target = null;
            }        
    }
}
