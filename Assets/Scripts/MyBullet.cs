using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyBullet : MonoBehaviour
{
    new Rigidbody rigidbody;
    public float bulletSpeed = 5f;
    public float life = 2f;

    private void Start() 
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddRelativeForce (0,0,bulletSpeed, ForceMode.Impulse);

        Destroy (gameObject, life);
    }
}
