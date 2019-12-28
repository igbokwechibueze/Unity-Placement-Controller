using UnityEngine;
using System.Collections;

public class BombFragment : MonoBehaviour
{
	public float speed = 5.0f;					// The speed at which this bomb fragment is propelled away from the initial explosion

	[Tooltip("The explosion prefab to be instantiated when this bomb fragment hits something")]
    public ParticleSystem explosionFXPrefab;
	

	// Use this for initialization
	void Start()
	{
		transform.Rotate(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
	}
	
	// Update is called once per frame
	void FixedUpdate()
	{
		transform.Translate(0, speed * Time.deltaTime, 0);
	}

	void OnCollisionEnter(Collision col)
	{
		// Make the projectile explode
		if (col.collider.gameObject.GetComponent<BombFragment>() == null)		// Explode only if the collision is not with another bombfragment
		{
			Explode(col.contacts[0].point);
		}
	}

	void Explode(Vector3 position)
	{
		// Instantiate the explosion
		if (explosionFXPrefab != null)
		{
			Instantiate(explosionFXPrefab, position, Quaternion.identity);
		}

		// Destroy this projectile
		Destroy(gameObject);
	}
}
