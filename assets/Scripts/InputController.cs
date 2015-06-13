using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputController : MonoBehaviour {

	// Use this for initialization
	private float pitch;
	private float roll;
	Vector2 startPosition;
	float startTime;
	Vector3 startTilt;

	void Start () {
		pitch = 0f;
		roll = 0f;
		startTilt = Input.acceleration;
	}

	public float getPitch(){
		return pitch;
	}

	public float getRoll(){
		return roll;
	}

	void workFromTilt(){
		float diff = startTilt.y - Input.acceleration.y;
		pitch -= diff;
		diff = startTilt.x - Input.acceleration.x;
		roll -= diff;
		startTilt = Input.acceleration;
	}
	

	// Update is called once per frame
	void Update () {
		if (Input.GetKey("up")) {
			pitch += 0.1f;
		}

		if (Input.GetKey("down")) {
			pitch -= 0.1f;
		}

		workFromTilt ();

		if (Input.GetKeyDown(KeyCode.M)) {
				GameObject.Find("AircraftJet").GetComponent<AeroplaneUserControl2Axis>().fullcontrol = !GameObject.Find("AircraftJet").GetComponent<AeroplaneUserControl2Axis>().fullcontrol;
		}

		pitch = Mathf.Clamp (pitch, -1f, 1f);

		float val = pitch.Remap (-1f, 1f, 0f, 100f);
		val = Mathf.RoundToInt (val);
		try{
			GameObject.Find("Touch-o-matic").GetComponent<Text>().text = "Touch-o-matic: "+val;
		}
		catch{}//we're on ios so it ain't there
	}
}

public static class ExtensionMethods {
	
	public static float Remap (this float value, float from1, float to1, float from2, float to2) {
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}
	
}
