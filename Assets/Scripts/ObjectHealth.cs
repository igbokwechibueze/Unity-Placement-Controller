using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Assigns a health value to an object which can be reduced upon recieving damage. 
// It also determines how long an object can live before dieing even without recieving damage.
/// Depending of the kind of object, this script can trigger the explosion damage script on its parent object to cause damage when it dies.
/// </summary>
public class ObjectHealth : MonoBehaviour
{
    [Header ("Health")]

    /// <summary>
    /// The health point of the object.
    /// </summary>
    [Tooltip("The health point of the object.")]
    public float health = 100f;

    /// <summary>
    /// Used by MountedGun script to cause damage to the object. If isAlive == null, MountedGun will not damage the object.
    /// </summary>
    [Tooltip("Used by MountedGun script to cause damage to the object. If isAlive == null, MountedGun will not damage the object.")]    
    public bool isAlive = true;

    /// <summary>
    /// How long the object can live before self distruction even without recieving damage.
    /// </summary>
    [Tooltip("How long the object can live before self distruction even without recieving damage.")]    
    public float timeToLive = 10f;

    [Header ("Dead State")]

    /// <summary>
    /// The explosion prefab to be instantiated when this object dies.
    /// </summary>
    [Tooltip("The explosion prefab to be instantiated when this object dies.")]
    public GameObject deadEffect;  

    /// <summary>
    /// Should the object explode and cause damage upon death.
    /// </summary>
    [Tooltip("Should the object explode and cause damage upon death.")]
    public bool canExplode;

    ExplosionDamage explosionDamage;

    private void Start() 
    {
        explosionDamage = GetComponent<ExplosionDamage>();
    }  

    public void TakeDamage (float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Effect();
        Destroy(gameObject);
        isAlive = false;
        // Calls the Explosion damage script to cause a damage on nearby gameobjects when this object dies.
        if (canExplode == true && explosionDamage != null)
        {
            explosionDamage.Explode();
        }
    }
    private void Update() 
    {
        /// Slowly kills the object with each passing frame.
        timeToLive -= Time.deltaTime;
        if (timeToLive <= 0f)
        {
            Die();
        }
    }

    private void Effect()
    {
        // Instantatiates a prefab upon death.
        if (deadEffect != null)
            {
                var particle = GameObject.Instantiate(deadEffect, transform.position, Quaternion.identity, null);
                particle.SetActive(true);
            }
    }
}
