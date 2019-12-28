using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turrets;

namespace TurretDemo
{
    /// <summary>
    /// For Missile Launchers this scripts calls the lauch function in the 'AALauncher Script', towards a set target.
    /// Targets are gotten from the DefenceControl Script on the Radar.
    /// </summary>  
    public class DroneWeaponLauncher : MonoBehaviour
    {
        [System.Serializable]
        public enum DefenceType
        {
            airDefence,
            surfaceDefence,
            allDefence
        }
        public DefenceType defenceType = DefenceType.airDefence;

        [Space]

        AALauncher launcher;
        Transform launchTransform;

        /// <summary>
        /// Script that shoots out a Raycasted Laser to determine if the GameObject is aiming at an object with a health script.
        /// </summary>
        [Tooltip("Script that shoots out a Raycasted Laser to determine if the GameObject is aiming at an object with a health script.")]    
        LaserAim laserAim;    
            
        public DefenceControl defence;

        [HideInInspector]
        public Transform target;

        [HideInInspector]
        public bool targetLocked;

        /// <summary>
        /// A random time to wait before launching missiles at targets.
        /// </summary>
        float wait;

        /// <summary>
        /// Minimum time to wait before launching missiles at targets.
        /// </summary>
        [Tooltip("Minimum time to wait before launching missiles at targets. ")]         
        public float minWaitTime = 2f;

        /// <summary>
        /// Maximum time to wait before launching missiles at targets.
        /// </summary>
        [Tooltip("Maximum time to wait before launching missiles at targets. ")]        
        public float maxWaitTime = 10f;

        /// <summary>
        /// Time to launch missiles at targets.
        /// </summary>       
        float startTime;

        private void Update()
        {
            if (defence != null && defence.turretsIdle == false)
            {
                if (defenceType == DefenceType.airDefence)
                {
                    target = defence.airtargetTransform;
                }

                if (defenceType == DefenceType.surfaceDefence)
                {
                    target = defence.surfacetargetTransform;
                }

                if (defenceType == DefenceType.allDefence)
                {
                    target = defence.allTargetTransform;
                }

                if (target && Time.time - startTime > wait && laserAim.foundObject == true)
                {
                    launcher.Launch(target);
                }   
            }else
            {
                target = null;
            }


            if (target == null)
            {
                targetLocked = false;
            }else
            {
                targetLocked = true;
            }  
        }

        void Start()
        {
            startTime = Time.time;
            wait = Random.Range(minWaitTime, maxWaitTime);

            launcher = GetComponentInChildren<AALauncher>();
            launchTransform = launcher.GetComponent<Transform>(); 

            laserAim = GetComponentInChildren<LaserAim>(); 
        }
    } 
}

