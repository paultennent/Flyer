using UnityEngine;
using System.Collections;

public class RockDemoScript : MonoBehaviour {

	float speed = 1f;
	bool back = false;
	// Use this for initialization

	// Update is called once per frame
	void Update () {
		if (transform.localEulerAngles.z > 45f && !back && speed == 1f) {
			speed = -1f;
		}
		if ((!back) && transform.localEulerAngles.z > 315f) {
			back = true;
		}
		if ((back) && transform.localEulerAngles.z < 90f) {
			back = false;
		}

		if (transform.localEulerAngles.z < 315f && back && speed == -1f) {
			speed = +1f;
		}
		
		transform.Rotate (new Vector3 (0, 0, speed));
	}
}
