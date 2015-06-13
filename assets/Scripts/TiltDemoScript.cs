using UnityEngine;
using System.Collections;

public class TiltDemoScript : MonoBehaviour {

	// Use this for initialization
	float speed = 1f;
	bool back = false;
	// Use this for initialization

	// Update is called once per frame
	void Update () {
		if (transform.localEulerAngles.x > 45f && !back && speed == 1f) {
			speed = -1f;
		}
		if ((!back) && transform.localEulerAngles.x > 315f) {
			back = true;
		}
		if ((back) && transform.localEulerAngles.x < 90f) {
			back = false;
		}
		
		if (transform.localEulerAngles.x < 315f && back && speed == -1f) {
			speed = +1f;
		}

		transform.Rotate (new Vector3 (speed, 0, 0));
	}
}
