﻿using UnityEngine;
using System.Collections;

public class collectable_script : MonoBehaviour {

	// Use this for initialization
	float speed = 1.0f;


	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		pos.y = pos.y + speed * Time.deltaTime;
		transform.position = pos;
	}
	
	void OnCollisionEnter (Collision col)
	{	
			if (col.gameObject.name == "AircraftJet") {
				//print("collected");
				gameObject.GetComponent<AudioSource> ().Play ();
				GameObject.Find ("Controller").GetComponent<CollectableGenerator> ().removeCollectable (gameObject);
				Destroy (gameObject, 0.1f);
				col.gameObject.GetComponent<ScoreController> ().addPoints (200);
				col.gameObject.GetComponent<ScoreController> ().modFuel (10);
			}
		}

}
