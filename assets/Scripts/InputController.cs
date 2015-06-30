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
	private bool running = true;
	private double fuel_burn_rate = 50.0;

	void Start () {
		pitch = 0f;
		roll = 0f;
		startTilt = Input.acceleration;
	}

	public void setRunning(bool r){
		running = r;
	}

	public float getPitch(){
		return 0; //pitch;
	}

	public float getRoll(){
		return roll;
	}

	private void updateFBR(){
		float r = GameObject.Find ("AircraftJet").transform.position.y;
		r = r + 50.0f;
		if (r <= 0.0f) {
			r = 0f;
		}
		if (r >= 99.0f) {
			r = 99f;
		}
		fuel_burn_rate = 100-(double)r;
		print (fuel_burn_rate);
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
		GameObject tm = GameObject.Find ("Touchomatic");
		if (tm != null) {
			TouchReader tr = tm.GetComponent<TouchReader> ();
			pitch=tr.connectionStdev/256.0f-1.0f;
		} else {
			if (Input.GetKey ("up")) {
				pitch += 0.1f;
			}

			if (Input.GetKey ("down")) {
				pitch -= 0.1f;
			}
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

		//check if we're running here
		if(running){
			GameObject.Find("AircraftJet").GetComponent<Rigidbody>().AddForce(GameObject.Find("AircraftJet").transform.up * ((float)(val-50)));
			updateFBR();
			double fueltoKill = ((double)val) / fuel_burn_rate * Time.deltaTime;
			GameObject.Find ("AircraftJet").GetComponent<ScoreController> ().modFuel (-fueltoKill);
		}

	}
}

public static class ExtensionMethods {
	
	public static float Remap (this float value, float from1, float to1, float from2, float to2) {
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}
	
}
