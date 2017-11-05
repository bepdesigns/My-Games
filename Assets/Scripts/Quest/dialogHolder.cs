using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialogHolder : MonoBehaviour {

	public string dialogue;
	private DialogueManager dMAn;

	public string[] dialogueLines;



	// Use this for initialization
	void Start () 
	{
		dMAn = FindObjectOfType<DialogueManager> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Player") 
		{
			//if (Input.GetKeyUp (KeyCode.K)) 
			//{
				//dMAn.ShowBox(dialogue);

				if (!dMAn.dialogActive)
				{
					dMAn.dialogLines = dialogueLines;
					dMAn.currentLines = 0;
					dMAn.ShowDialogue ();
				}

				//if(transform.parent.GetComponent<Player>() !=null)
				//{
					//transform.parent.GetComponent<Enemy_Chase> ().chaseTarget = false;
				//}
			//}
		}
	}
}
