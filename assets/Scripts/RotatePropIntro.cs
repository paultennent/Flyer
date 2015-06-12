using UnityEngine;
using System.Collections;

public class RotatePropIntro: MonoBehaviour {
	
	private float rotation;
	private float throttle = 0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (throttle < 0.5f) {
			throttle = throttle + 0.001f;
		}
		rotation += 1000*throttle;
		transform.localEulerAngles = new Vector3 (0, 0, rotation);
	}
}