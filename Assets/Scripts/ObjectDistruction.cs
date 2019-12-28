using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDistruction : MonoBehaviour
{
    public float timeToDestroy = 10f;


    // Update is called once per frame
    void Update()
    {
        Destroy (gameObject, timeToDestroy );
    }
}
