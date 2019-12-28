using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake (float shakeDuration, float shakeMagnitude)
    {
        // stores the original position of the camera before shaking, so we can reset it when we are done.
        Vector3 originalCamPosition = transform.position;

        // how much time has pased since we started shaking
        float  elapsed = 0.0f;

        // keeps shaking until the time elapsed equals the shake duration.
        while (elapsed < shakeDuration)
        {
            // offset the camera by a random amount on the X and Y axis every frame multiplied by the shakeMagnitude.
            float x = Random.Range (-1f, 5f) * shakeMagnitude;
            float y = Random.Range (-1f, 5f) * shakeMagnitude;

            // apply the offset to the camera position, but retaining its position on the Z axis.
            transform.localPosition = new Vector3 (x, y, originalCamPosition.z);

            // increase elapsed time every frame.
            elapsed += Time.deltaTime;
            Debug.Log ("shaking");

            yield return null;
        }

        // reset camera position to its original point as soon as the while loop is over (elapsed = shakeDuration)
        transform.localPosition = originalCamPosition;
    }
}
