using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turrets;

/// <summary>
/// This scripts calls the 'TurretRotation' script on objects placed into specific arrays (airDefence, surfaceDefence, allDefence). Causing the game objects to turn and face a target.
/// The target transforms are gotten from the 'RadarNew' script.
/// </summary>
public class DefenceControl : MonoBehaviour
{
    /// <summary>
    /// GameObjects with the TurretRotation Script that would target only air threats.
    /// </summary>
    [Tooltip("GameObjects with the TurretRotation Script that would target only air threats.")] 
    public TurretRotation[] airDefence;

    /// <summary>
    /// GameObjects with the TurretRotation Script that would target only surface threats.
    /// </summary>
    [Tooltip("GameObjects with the TurretRotation Script that would target only surface threats.")]     
    public TurretRotation[] surfaceDefence;
    
    /// <summary>
    /// GameObjects with the TurretRotation Script that would target all threats.
    /// </summary>
    [Tooltip("GameObjects with the TurretRotation Script that would target all threats.")]     
    public TurretRotation[] allDefence;

    [HideInInspector]

    public Vector3 targetPos;

    [HideInInspector]
    /// <summary>
    /// Closest air threat.
    /// </summary>
    public Transform airtargetTransform;

    [HideInInspector]
    /// <summary>
    /// Closest surface threat.
    /// </summary>
    public Transform surfacetargetTransform;

    [HideInInspector]
    /// <summary>
    /// Closest threat.
    /// </summary>    
    public Transform allTargetTransform;

    [Space]
    [HideInInspector]
    /// <summary>
    /// Is gameobject presently tracking a target or not. If not sets gameobject to a resting position.
    /// </summary>    
    public bool turretsIdle = false;

    /// <summary>
    /// Script that finds and assigns targets.
    /// </summary>  
    RadarNew radarNew;

    private void Start() 
    {
        radarNew = GetComponentInChildren<RadarNew>();
        
    }

    private void Update()
    {
        // Set turret to idle when radar is lost.
       if (radarNew == null)
       {
            turretsIdle = true;
            airtargetTransform = null;
            surfacetargetTransform = null;
            allTargetTransform = null;
       }else
            {
               airtargetTransform = radarNew.airTarget;
               surfacetargetTransform = radarNew.surfaceTarget;
               allTargetTransform = radarNew.allTarget;
            }

       // When a transform is assigned, pass that to the turret. If not,
       // just pass in whatever this is looking at.
       targetPos = transform.TransformPoint(Vector3.forward * 200.0f);
       foreach (TurretRotation tur in airDefence)
       {
            if (airtargetTransform == null)
               tur.SetAimpoint(targetPos);
            else
               tur.SetAimpoint(airtargetTransform.position);

            tur.SetIdle(turretsIdle);
       }

       foreach (TurretRotation tur in surfaceDefence)
       {
            if (surfacetargetTransform == null)
               tur.SetAimpoint(targetPos);
            else
               tur.SetAimpoint(surfacetargetTransform.position);

            tur.SetIdle(turretsIdle);
       }

       foreach (TurretRotation tur in allDefence)
       {
            if (allTargetTransform == null)
               tur.SetAimpoint(targetPos);
            else
               tur.SetAimpoint(allTargetTransform.position);

            tur.SetIdle(turretsIdle);
       }       
    }

   // Draws a line towards the target.
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetPos, 1.0f);
    }

}
