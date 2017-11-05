using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour 
{
	public Texture2D abilityIcon;
	public GameObject shield;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		bool AB1 = Input.GetKeyDown (KeyCode.Alpha1);

		if (AB1) 
		{
			Ability1 ();
		}
		
	}
	public void OnGUI()
	{
		GUI.DrawTexture(new Rect(10,150, abilityIcon.width, abilityIcon.height),abilityIcon);
	}

	public void Ability1()
	{
		GameObject myShield = (GameObject)Instantiate (shield, transform.position, shield.transform.rotation);
		Shield shieldScript = myShield.GetComponent<Shield> ();
		shieldScript.myOwner = this.gameObject;
	
	}

}
