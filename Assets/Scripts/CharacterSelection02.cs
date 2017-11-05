using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection02 : MonoBehaviour {

	private GameObject[] characterList;
	private int index;


	// Use this for initialization
	private void Start () 
	{
		index = PlayerPrefs.GetInt ("characterSelected");

		characterList = new GameObject[transform.childCount];

		//Fill the array with our models
		for (int i = 0; i < transform.childCount; i++) 
			characterList[i] = transform.GetChild(i).gameObject;

		//We toggle off their renderer
		foreach (GameObject go in characterList) 
			go.SetActive (false);

		//we toggle on the selected character
		if (characterList[index])
			characterList[index].SetActive (true);
		
	}

	public void ToggleLeft()
	{
		//toggle off the current model
		characterList[index].SetActive(false);


		index--;//index -= 1; index = index -1;
		if (index < 0)
			index = characterList.Length - 1;

		//toggle on the new model
		characterList[index].SetActive(true);

	}

	public void ToggleRight()
	{
		//toggle off the current model
		characterList[index].SetActive(false);


		index++;//index -= 1; index = index -1;
		if (index == characterList.Length)
			index = 0;

		//toggle on the new model
		characterList[index].SetActive(true);

	}

	public void ConfirmButton()
	{
		PlayerPrefs.SetInt ("characterSelected", index);
		SceneManager.LoadScene ("CharacterSelector");
	}
}
