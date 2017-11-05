using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour {


	public List<GameObject> characterList;
	public int index=0;

	void Start () {
	
		GameObject[] characters = Resources.LoadAll<GameObject>("Prefab");
		foreach (GameObject c in characters) {

			GameObject _char = Instantiate(c) as GameObject;
			_char.name = c.name;
			_char.transform.SetParent(GameObject.Find("CharacterList").transform);

			characterList.Add(_char);
			_char.SetActive(false);
			GameObject.Find("CharName").GetComponent<Text>().text = characterList[index].name;
			characterList[index].SetActive(true);


		}
	}
	public void Next(){
		characterList [index].SetActive (false);
		if (index == characterList.Count - 1) {
			index = 0;
		} else {
			index++;
		}
		GameObject.Find("CharName").GetComponent<Text>().text = characterList[index].name;
		characterList [index].SetActive (true);
		
	}
	public void Previous(){
		characterList [index].SetActive (false);
		if (index == 0) {
			index = characterList.Count-1;
		} else {
			index--;
		}
		GameObject.Find("CharName").GetComponent<Text>().text = characterList[index].name;
		characterList [index].SetActive (true);
		
	}

	void Update(){
		characterList [index].transform.Rotate (0,0.5f,0);
	}
}
