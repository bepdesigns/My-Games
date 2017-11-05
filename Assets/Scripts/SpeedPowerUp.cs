using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : PowerUpEffect {

	//Inspector Variables
	public float speedMultiplier = 2.0f;


	new void OnTriggerEnter(Collider other)
	{
		Rigidbody rb = other.transform.root.GetComponent<Rigidbody> ();
		rb.velocity *= speedMultiplier;
		base.OnTriggerEnter (null);
	}
}
