using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBarController : MonoBehaviour {

	public Image imageCooldown;
	public float cooldown = 5;
	bool IsCooldown;

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Alpha1)) 
		{
			IsCooldown = true;
		}

		if (IsCooldown) 
		{
			imageCooldown.fillAmount += 1 / cooldown * Time.deltaTime;
		}

		if (imageCooldown.fillAmount >= 1) 
		{
			imageCooldown.fillAmount = 0;
			IsCooldown = false;
		}
	}
		
}
