using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
	void Update()
	{
		transform.Translate(0, 0.2f, 0);

		if (transform.position.y > 5)
		{
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		Destroy(coll.gameObject);
		Destroy(gameObject);
	}
}