using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detects enemies within a certain radius and assigns the closest to target transforms (air, surface, all). This is done using the KindOfTarget script.
/// Also plays a siren audio and a rotation animation on choice when enemies are detected.
/// </summary>
public class RadarNew : MonoBehaviour
{
    [System.Serializable]
    public enum TargetsToTrack
    {
        Enemies,
        PlayerAndAlies
    }

    public TargetsToTrack targetsToTrack = TargetsToTrack.Enemies;

    [Header ("Targets")]

    /// <summary>
    /// How far the Radar can track enemies. Beyond this point tracking stops.
    /// </summary>
    [Tooltip("How far the Radar can track enemies. Beyond this point tracking stops.")] 
    public float covarageDistance = 1000f;

    /// <summary>
    /// The closest enemy in the air.
    /// </summary>
    [Tooltip("The closest enemy in the air")]     
    [HideInInspector]
    public Transform airTarget;

    /// <summary>
    /// Distance to the closest AirTarget.
    /// </summary>
    [Tooltip("Distance to the closest AirTarget.")]     
    [HideInInspector]
    public float airTargetDistance;

    /// <summary>
    /// The closest enemy on the surface.
    /// </summary>
    [Tooltip("The closest enemy on the surface.")] 
    [HideInInspector]
    public Transform surfaceTarget;

    /// <summary>
    /// Distance to the closest SurfaceTarget.
    /// </summary>
    [Tooltip("Distance to the closest SurfaceTarget.")]     
    [HideInInspector]
    public float surfaceTargetDistance;    

    /// <summary>
    /// The closest enemy from both air and surface.
    /// </summary>
    [Tooltip("The closest enemy from both air and surface.")]     
    [HideInInspector]    
    public Transform allTarget;

    /// <summary>
    /// Distance to the closest Target.
    /// </summary>
    [Tooltip("Distance to the closest Target.")]     
    [HideInInspector]
    public float allTargetDistance; 

    /// <summary>
    /// Is true if Radar finds a target
    /// </summary>
    [Tooltip("Is true if Radar finds a target.")]  
    [HideInInspector]
    public bool wasTriggered;   

    [Header("-Animation-")]

    /// <summary>
    /// The Animator controlling the radar rotation.
    /// </summary>
    [Tooltip("The Animator controlling the radar rotation.")]
    Animator radarRotationAnimator;

    /// <summary>
    /// The name of the animation to play when shooting.
    /// </summary>
    [Tooltip("The name of the animation to play when shooting.")]
    public string radarRotationAnim = "RadarRotation";

    /// <summary>
    /// Decides if the radar needs to rotate or not.
    /// </summary>
    [Tooltip("Decides if the radar needs to rotate or not.")]
    public bool useAnim;  

    [Header("-Timing-")]

    /// <summary>
    /// A random time to wait before assigning targets.
    /// </summary>
    [Tooltip("A random time to wait before assigning targets.")] 
    float wait;

    /// <summary>
    /// Time at which targets would be assigned.
    /// </summary>
    [Tooltip("Time at which targets would be assigned.")] 
    float startTime;

    [Space]
    /// <summary>
    /// The Audio Source to set active when Radar detects enemies.
    /// </summary>
    [Tooltip("The Audio Source to set active when Radar detects enemies.")]
    public GameObject sirenAudioSource;

    private void Start() 
    {
        //Time at which AI can start shooting.
        startTime = Time.time;
        // A random time at which AI can start shooting.
        wait = Random.Range(3.0f, 6.0f); 

        // AudioPlayer GameObject is deactivated at start.
        if (sirenAudioSource != null)
        {
            sirenAudioSource.SetActive (false);
        }
        
        //Get the Animation component attached to the Radar GameObject
        radarRotationAnimator = GetComponentInChildren<Animator>();               
    }

	// Update is called once per frame
	void Update () 
    {
		FindClosestAirEnemy ();
        FindClosestSurfaceEnemy ();
        FindClosestAllEnemy ();

        // This would rotate the radar, if an animation is provided.
        if (useAnim == true && radarRotationAnimator != null)
        {
            radarRotationAnimator.Play(radarRotationAnim);             
        }

        // This would sound a siren when targets are detected.
        if (airTarget != null || surfaceTarget != null || allTarget != null)
        {
            wasTriggered = true;
            
            if (sirenAudioSource != null)
            {
               sirenAudioSource.SetActive (true); 
            }
        }

        if (airTarget == null && surfaceTarget == null && allTarget == null)
        {
            wasTriggered = false;
            
            if (sirenAudioSource != null)
            {
               sirenAudioSource.SetActive (false); 
            }            
        }                
	}

//***** The methods below assigns targets into their various kinds (Air or Surface), it also puts all the targets into another array called 'AllEnemy'*****
//***** It does this by first checking for the 'KindOfTarget' script on any object within its coverage.*****
//***** It the checks if the targets are eneimes or not and what their target type is (Air or Surface). *****
//***** With this targets are grouped accordingly to their 'TargetType', with the closess begining assigned first. *****
	void FindClosestAirEnemy()
	{
		float distanceToClosestEnemy = covarageDistance * 1000f; //Mathf.Infinity;
		KindOfTarget closestEnemy = null;
		KindOfTarget[] allEnemies = GameObject.FindObjectsOfType<KindOfTarget>();

		foreach (KindOfTarget currentEnemy in allEnemies) 
        {
            if (targetsToTrack == TargetsToTrack.Enemies)
            {
                if (currentEnemy.isEnemy == true && currentEnemy.targetType == KindOfTarget.TargetType.Air)
                {
    			    float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
			        if (distanceToEnemy < distanceToClosestEnemy) 
                    {
				        distanceToClosestEnemy = distanceToEnemy;
				        closestEnemy = currentEnemy;
			        }
                }            
            }else
                {
                    if (currentEnemy.isEnemy == false && currentEnemy.targetType == KindOfTarget.TargetType.Air)
                    {
    			        float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
			            if (distanceToEnemy < distanceToClosestEnemy) 
                        {
				            distanceToClosestEnemy = distanceToEnemy;
				            closestEnemy = currentEnemy;
			            }
                    }            
                }

		}

        if (closestEnemy != null && Time.time - startTime > wait)
        {
            airTarget = closestEnemy.transform;
            airTargetDistance = distanceToClosestEnemy;
            Debug.DrawLine (this.transform.position, closestEnemy.transform.position);
           // Debug.Log("DistanceToEnemy: " + distanceToClosestEnemy + "km");
        }else
        {
            airTarget = null;
        }        
	} 

	void FindClosestSurfaceEnemy()
	{
		float distanceToClosestEnemy = covarageDistance * 1000f; //Mathf.Infinity;
		KindOfTarget closestEnemy = null;
		KindOfTarget[] allEnemies = GameObject.FindObjectsOfType<KindOfTarget>();

		foreach (KindOfTarget currentEnemy in allEnemies) 
        {
            if (targetsToTrack == TargetsToTrack.Enemies)
            {
                if (currentEnemy.isEnemy == true && currentEnemy.targetType == KindOfTarget.TargetType.Land)
                {
    			    float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
			        if (distanceToEnemy < distanceToClosestEnemy) 
                    {
				        distanceToClosestEnemy = distanceToEnemy;
				        closestEnemy = currentEnemy;
			        }
                }            
            }else
                {
                    if (currentEnemy.isEnemy == false && currentEnemy.targetType == KindOfTarget.TargetType.Land)
                    {
    			        float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
			            if (distanceToEnemy < distanceToClosestEnemy) 
                        {
				            distanceToClosestEnemy = distanceToEnemy;
				            closestEnemy = currentEnemy;
			            }
                    }            
                }

		}

        if (closestEnemy != null && Time.time - startTime > wait)
        {
            surfaceTarget = closestEnemy.transform;
            surfaceTargetDistance = distanceToClosestEnemy;
            Debug.DrawLine (this.transform.position, closestEnemy.transform.position);
           // Debug.Log("DistanceToEnemy: " + distanceToClosestEnemy + "km");
        }else
        {
            surfaceTarget = null;
        }        
	}

	void FindClosestAllEnemy()
	{
		float distanceToClosestEnemy = covarageDistance * 1000f; //Mathf.Infinity;
		KindOfTarget closestEnemy = null;
		KindOfTarget[] allEnemies = GameObject.FindObjectsOfType<KindOfTarget>();

		foreach (KindOfTarget currentEnemy in allEnemies) 
        {
            if (targetsToTrack == TargetsToTrack.Enemies)
            {
                if (currentEnemy.isEnemy == true)
                {
    			    float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
			        if (distanceToEnemy < distanceToClosestEnemy) 
                    {
				        distanceToClosestEnemy = distanceToEnemy;
				        closestEnemy = currentEnemy;
			        }
                }            
            }else
                {
                    if (currentEnemy.isEnemy == false)
                    {
    			        float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
			            if (distanceToEnemy < distanceToClosestEnemy) 
                        {
				            distanceToClosestEnemy = distanceToEnemy;
				            closestEnemy = currentEnemy;
			            }
                    }            
                }

		}

        if (closestEnemy != null && Time.time - startTime > wait)
        {
            allTarget = closestEnemy.transform;
            allTargetDistance = distanceToClosestEnemy;
            Debug.DrawLine (this.transform.position, closestEnemy.transform.position);
           // Debug.Log("DistanceToEnemy: " + distanceToClosestEnemy + "km");
        }else
        {
            allTarget = null;
        }        
	}     
}
