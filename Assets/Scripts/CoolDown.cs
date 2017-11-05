using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDown : MonoBehaviour 

{
	public float coolDown = 1;
	public float coolDownTimer;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (coolDownTimer > 0) 
		{
			coolDownTimer -= Time.deltaTime;
		
		}

		if (coolDownTimer < 0) 
		{
			coolDownTimer = 0;
		}

		if (Input.GetButton ("Start") && coolDownTimer == 0) 
		{
			Attack ();
			coolDownTimer = coolDown;
		}
	}

	public void Attack ()
	{
		Debug.Log ("Hit" + Time.time);
	}
}
