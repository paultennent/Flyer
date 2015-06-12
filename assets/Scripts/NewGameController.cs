﻿using UnityEngine;
using System.Collections;

public class NewGameController : MonoBehaviour {

	private bool itsenabled = false;
	// Use this for initialization
	void Start () {
	
	}

	public void setEnabled(bool e){
		itsenabled = e;
	}

	// Update is called once per frame
	void Update () {
		if (itsenabled) {
			if (Input.GetKeyDown ("space")) {
				Application.LoadLevel ("intro");
			}
			GameObject.Find ("Main Camera").transform.RotateAround (GameObject.Find ("PlayerDeathEffect").transform.position, Vector3.up, 10 * Time.deltaTime);
		}
	}
}
