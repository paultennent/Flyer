using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

	// Use this for initialization
	private float pitch;

	void Start () {
		pitch = 0f;
	}

	public float getPitch(){
		return pitch;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey("up")) {
			pitch += 0.1f;
		}

		if (Input.GetKey("down")) {
			pitch -= 0.1f;
		}

		if (Input.GetKeyDown(KeyCode.M)) {
				GameObject.Find("AircraftJet").GetComponent<AeroplaneUserControl2Axis>().fullcontrol = !GameObject.Find("AircraftJet").GetComponent<AeroplaneUserControl2Axis>().fullcontrol;
		}

		pitch = Mathf.Clamp (pitch, -1f, 1f);

		float val = pitch.Remap (-1f, 1f, 0f, 100f);
		val = Mathf.RoundToInt (val);
		GameObject.Find("Touch-o-matic").GetComponent<TextMesh>().text = "Touch-o-matic: "+val;
	}
}

public static class ExtensionMethods {
	
	public static float Remap (this float value, float from1, float to1, float from2, float to2) {
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}
	
}
