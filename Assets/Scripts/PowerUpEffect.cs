using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpEffect : MonoBehaviour 
{
	//private Variables
	private Vector3 orignalScale;
	new protected Collider collider;


	//Inspector Variables
	public float rotationSpeed = 25.0f;
	public float bobbleAmplitutde = 0.25f;
	public float scaleSpeed = 2.0f;
	public float minSpawnTime = 5.0f;
	public float maxSpawnTime = 15.0f;



	// Use this for initialization
	protected void Start () 
	{
		orignalScale = transform.localScale;
		collider = GetComponent<Collider> ();
	}
	
	// Update is called once per frame
	protected void Update () 
	{
		transform.Rotate (new Vector3 (0, Time.deltaTime * rotationSpeed, 0));
		Vector3 position = transform.position;
		position.y += Mathf.Sin (Time.timeSinceLevelLoad) * Time.deltaTime * bobbleAmplitutde;
		transform.position = position;
	}

	protected void OnTriggerEnter(Collider other)
	{
		collider.enabled = false;
		StartCoroutine (ScaleObject (Vector3.zero));
	}

	private IEnumerator ScaleObject(Vector3 traget)
	{
		Vector3 scale = transform.localScale;
		while (scale != traget) 
		{
			scale = Vector3.MoveTowards (scale, traget, Time.deltaTime * scaleSpeed);
			transform.localScale = scale;
			yield return new WaitForEndOfFrame ();
		}
		if (scale == Vector3.zero) {
			StartCoroutine (RespawnObject ());
		} else if (scale == orignalScale) 
		{
			collider.enabled = true;
		}
	}

	private IEnumerator RespawnObject()
	{
		yield return new WaitForSeconds (Random.Range (minSpawnTime, maxSpawnTime));
		StartCoroutine (ScaleObject (orignalScale));
	}

}
