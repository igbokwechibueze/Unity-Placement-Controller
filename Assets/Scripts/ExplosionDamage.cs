using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When called sends out and explosion force that moves and damages objects within a certain raduis, as well as shaking the camera.
/// </summary>
public class ExplosionDamage : MonoBehaviour
{
    [Header("-Object Damaging-")]
    /// <summary>
    /// Objects within this raduis would be affected by the blast.
    /// </summary>
    [Tooltip("Objects within this raduis would be affected by the blast.")]
    public float blastRadius = 5f;

    /// <summary>
    /// The force the blast exerts on near by objects with rigidbodies.
    /// </summary>
    [Tooltip("The force the blast exerts on near by objects with rigidbodies.")]    
    public float blastForce = 5f;

    /// <summary>
    /// The damage the blast does to objects within the blast radius. Only objects with colliders are affected
    /// </summary>
    [Tooltip("The damage the blast does to objects within the blast radius. Only objects with colliders are affected")]    
    public float blastDamage = 5f;

    [Space]
    [Header("-Camera Shake-")]

    /// <summary>
    /// Script on the camera that handles shaking.
    /// </summary>
    [Tooltip("Script on the camera that handles shaking.")]   
    public CameraShake cameraShake;

    /// <summary>
    /// How long should the camera keep on shaking.
    /// </summary>
    [Tooltip("How long should the camera keep on shaking.")] 
    public float shakeDuration = .15f;

    /// <summary>
    /// The strenght of the camera shake.
    /// </summary>
    [Tooltip("The strenght of the camera shake.")]     
    public float shakeMagnitude = .4f;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void Explode ()
    {
        // call the shake function on the cameraShake script to shake the camera.
     //   StartCoroutine(cameraShake.Shake (shakeDuration, shakeMagnitude));

        // an array of colliders within the blast radius.
        Collider [] collidersToDamage = Physics.OverlapSphere(transform.position, blastRadius);

        // For each collider within the blast radius get the health script and apply damage.
        foreach (Collider nearObjects in collidersToDamage)
        {
           ObjectHealth objectHealth = nearObjects.GetComponent<ObjectHealth>();
           if (objectHealth != null)
           {
               objectHealth.TakeDamage(blastDamage);
           }
        }

        // an array of colliders within the blast radius.
        Collider [] collidersToMove = Physics.OverlapSphere(transform.position, blastRadius);

        // For each collider within the blast radius get their rigibody and apply force.
        foreach (Collider nearObjects in collidersToMove)
        {
           Rigidbody rigidbody = nearObjects.GetComponent<Rigidbody>();
           if (rigidbody != null)
           {
               rigidbody.AddExplosionForce(blastForce, transform.position, blastRadius, 1, ForceMode.Impulse);
           }
        }        
    }
}
