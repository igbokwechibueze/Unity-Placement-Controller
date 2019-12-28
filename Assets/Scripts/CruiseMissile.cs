using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CruiseMissile : MonoBehaviour
{
    public Transform target;
    public float force;
    public float rotationForce;
    public float secondsBeforeHoming;
    public float lauchForce;
    new Rigidbody rigidbody;
    private bool shouldFollow;

    private void Start() {
        rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(waithBeforeHoming());
    }

    private void FixedUpdate() 
    {
        if (shouldFollow)
        {
           // This will make the projectile travels straigth towards the target.
            Vector3 direction = target.position - rigidbody.position;
            direction.Normalize();
            
            // This will make the projectile smoothly rotate towards the target.
            Vector3 rotationAmount = Vector3.Cross(transform.forward, direction);
            rigidbody.angularVelocity = rotationAmount * rotationForce;
            rigidbody.velocity = transform.forward * force; 
        }
    }

    private IEnumerator waithBeforeHoming(){
        rigidbody.AddForce(Vector3.up * lauchForce, ForceMode.Impulse);
        yield return new WaitForSeconds(secondsBeforeHoming);
        shouldFollow = true;
    }

}
