using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputController : MonoBehaviour {

	// Use this for initialization
	private float pitch = 0;
	private float roll = 0;
	private float throttle = 0;
	Vector2 startPosition;
	float startTime;
	Vector3 startTilt;
	private bool running = true;
	private double fuel_burn_rate = 20.0;
	private float ceiling = 500f;
	private float teleportPos = -70;
	private float upperTeleportPos = 500;
	private float teleportRecoveryPos = -30;

	void Start () {
		pitch = 0f;
		roll = 0f;
		startTilt = Input.acceleration;
		GameObject.Find ("AircraftJet").GetComponent<AeroplaneController> ().Immobilize ();
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

	public float getThrottle(){
		return throttle;
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
		//print (fuel_burn_rate);
	}

	void workFromTilt(){
		float diff = startTilt.y - Input.acceleration.y;
		pitch -= diff;
		diff = startTilt.x - Input.acceleration.x;
		roll -= diff;
		startTilt = Input.acceleration;
	}

	void checkCeiling(){
		if (running) {
			if (GameObject.Find ("AircraftJet").transform.position.y > ceiling) {
				pitch = -1;
			}
		}
	}

	void checkForTeleport(){
		if (running) {
			GameObject obj = GameObject.Find ("AircraftJet");
			if (obj.transform.position.y < teleportPos || obj.transform.position.y > upperTeleportPos) {
				obj.transform.position = new Vector3 (obj.transform.position.x,teleportRecoveryPos,obj.transform.position.z);
			}
		}
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
		checkCeiling ();
		checkForTeleport ();

		if (Input.GetKeyDown(KeyCode.M)) {
				GameObject.Find("AircraftJet").GetComponent<AeroplaneUserControl2Axis>().fullcontrol = !GameObject.Find("AircraftJet").GetComponent<AeroplaneUserControl2Axis>().fullcontrol;
		}

		pitch = Mathf.Clamp (pitch, -1f, 1f);
		throttle = pitch.Remap (-1f, 1f, -0.0f, 0.5f);

		float val = pitch.Remap (-1f, 1f, 0f, 100f);
		val = Mathf.RoundToInt (val);

		//check if we're running here
		if (running) {
			double f = GameObject.Find ("AircraftJet").GetComponent<ScoreController> ().getFuel ();
			//print ("Fuel:" + f);
			if (f > 0) {
				if (GameObject.Find ("AircraftJet").GetComponent<ScoreController> ().getLevel () > 0) {
					GameObject.Find ("AircraftJet").GetComponent<Rigidbody> ().AddForce (GameObject.Find ("AircraftJet").transform.up * ((float)((val * 5f) - 200f)));
				} else {
					GameObject.Find ("AircraftJet").GetComponent<Rigidbody> ().AddForce (GameObject.Find ("AircraftJet").transform.up * ((float)((val) - 50)));
				}
				GameObject.Find ("SimpleFlame(Red)").GetComponent<ParticleSystem> ().startSize = throttle.Remap (-0.1f, 0.5f, 0f, 0.5f);
				//updateFBR();
				double fueltoKill = ((double)val) / fuel_burn_rate * Time.deltaTime;
				if (GameObject.Find ("AircraftJet").GetComponent<ScoreController> ().getLevel () > 0) {
					GameObject.Find ("AircraftJet").GetComponent<ScoreController> ().modFuel (-fueltoKill);
				}
			} else {
				GameObject.Find ("AircraftJet").GetComponent<Rigidbody> ().AddForce (GameObject.Find ("AircraftJet").transform.up * (-100));
				GameObject.Find ("SimpleFlame(Red)").GetComponent<ParticleSystem> ().startSize = 0f;
			}
		} 
	}
}

public static class ExtensionMethods {
	
	public static float Remap (this float value, float from1, float to1, float from2, float to2) {
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}
	
}
