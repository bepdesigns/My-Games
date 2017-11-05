using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour 
{

	private float timer;
	public float duration;
	public GameObject myOwner;


	// Use this for initialization
	void Start () 
	{
		timer = duration;
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer -= Time.deltaTime;

		transform.position = myOwner.transform.position;

		if (timer <= 0) 
		{
			Death ();
		}
	}
	public void Death()
	{
		Destroy (gameObject);
	}
}
