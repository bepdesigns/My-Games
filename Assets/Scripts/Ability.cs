using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour {

	public GameObject immortal;
	public Texture2D ab1;
	public Texture2D ab1CD;
	private  float ab1Timer = 0;
	public float ab1CDTime;

	
	// Update is called once per frame
	void Update () 
	{
		ab1Timer -= Time.deltaTime;
	}

	void OnGUI()
	{
		bool ab1Key = Input.GetKeyDown (KeyCode.Q);
		if (ab1Timer <= 0) 
		{
			GUI.Label (new Rect (10, 10, 50, 50), ab1);

			if (ab1Key) 
			{
				AbilityOne ();
			}
			else
			{
				GUI.Label (new Rect (10, 10, 50, 50), ab1CD);
			}

		}
	}
	public void AbilityOne()
	{
		Instantiate (immortal, Vector3.zero, Quaternion.identity);
		ab1Timer = ab1CDTime;
	}
}
