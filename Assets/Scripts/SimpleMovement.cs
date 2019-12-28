using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    public float moveStrength = 5.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(Mathf.Sin(Time.time), 0.0f, 0.0f) * moveStrength * Time.deltaTime);
    }
}
