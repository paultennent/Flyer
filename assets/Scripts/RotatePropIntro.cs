using UnityEngine;
using System.Collections;

public class RotatePropIntro: MonoBehaviour {
	
	private float rotation;
	private float throttle = 0f;

	private bool engaged = false;
	// Use this for initialization
	void Start () {
		
	}

	public void engage(bool e){
		engaged = e;
	}
	
	// Update is called once per frame
	void Update () {
		if (engaged) {
			if (throttle < 0.5f) {
				throttle = throttle + 0.001f;
			}
		} else {
			if (throttle > 0.0f) {
				throttle = throttle - 0.01f;
			}
		}
		rotation += 1000 * throttle;
		transform.localEulerAngles = new Vector3 (0, 0, rotation);
	}
}