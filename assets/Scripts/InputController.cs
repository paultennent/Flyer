using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputController : MonoBehaviour {


	private float keyPitch=0;// special pitch for keyboard override
	private bool keyOverride=false;
	private float pitch = 0;
	private float roll = 0;
	private float throttle = 0;
	Vector2 startPosition;
	float startTime;
	Vector3 startTilt;
	private bool running = true;
	private double fuel_burn_rate = 20.0;
	private float ceiling = 600f;
	private float teleportPos = -70;
	private float upperTeleportPos = 1200;
	private float teleportRecoveryPos = 0;

	private float timout = 60f;
	private float timeouttime = 0f;

	private GameObject aircraftJet;

	void Start () {
		timeouttime = Time.time;
		aircraftJet = GameObject.Find ("AircraftJet");
		pitch = 0f;
		roll = 0f;
		startTilt = Input.acceleration;
		aircraftJet.GetComponent<AeroplaneController> ().Immobilize ();
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
		float r = aircraftJet.transform.position.y;
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



	float ceilingMult=1.0f;

	void checkFuelLow()
	{
		bool showFuelLow = false;
		if (running) {
			double f = aircraftJet.GetComponent<ScoreController> ().getFuel ();
			showFuelLow=(f<20);
		}
		if (showFuelLow) {
			GameObject.Find ("FuelLow").GetComponent<Text> ().color = Color.white;
		} else {
			GameObject.Find ("FuelLow").GetComponent<Text> ().color = Color.clear;
		}
	}

	void checkCeiling(ScoreController sc){
		bool showTooHigh = false;
		bool showFlyLower = sc.showScoreTimeout();
		if (running) {
			float height=aircraftJet.transform.position.y;
			if (height > ceiling) {
				ceilingMult=1.0f-(height-ceiling)*0.01f;
				if(ceilingMult<0)ceilingMult=0;
				pitch=(pitch+1f) * ceilingMult - 1f;
				//ceilingMult-=0.025f;
				//if(ceilingMult<0)ceilingMult=0;
				//pitch = -1;//
				showTooHigh=true;
			}else
			{
				pitch=(pitch+1f) * ceilingMult - 1f;
				ceilingMult+=0.05f;
				if(ceilingMult>1)ceilingMult=1;

			}
		}
		if (showTooHigh) {
			GameObject.Find ("TooHigh").GetComponent<Text> ().color = Color.white;
		} else {
			GameObject.Find ("TooHigh").GetComponent<Text> ().color = Color.clear;
		}
		if (showFlyLower) {
			GameObject.Find ("FlyLower").GetComponent<Text> ().color = Color.white;

		} else {
			GameObject.Find ("FlyLower").GetComponent<Text> ().color = Color.clear;
		}
        GameObject.Find ("Altitude").GetComponent<Text> ().text = "Height: "+(int)sc.heightFromGround();

	}

	void checkForTeleport(){
		if (running) {
			GameObject obj = aircraftJet;
			if (obj.transform.position.y < teleportPos || obj.transform.position.y > upperTeleportPos) {
				obj.transform.position = new Vector3 (obj.transform.position.x,teleportRecoveryPos,obj.transform.position.z);
			}
		}
	}

	float angleFiltered=45f;
	void updatePowerGauge(float t){
		if (running) {

			t = t.Remap (0f, 0.5f, 45f, 315f);
			angleFiltered = t * 0.1f + 0.9f * angleFiltered;

			Transform go = GameObject.Find ("powergauge").transform;
			go.localEulerAngles = new Vector3 (0f, 0f, -angleFiltered);
		}
	}
	

	// Update is called once per frame
	void Update () {
		//handle key input - key input overrides touchomatic for this game
		if (Input.GetKey ("up")) {
			keyOverride=true;
			keyPitch=1;
			keyPitch=Mathf.Clamp(keyPitch,-1,1);
		}
		
		if (Input.GetKey ("down")) {
			keyPitch-=Time.deltaTime;
			keyPitch=Mathf.Clamp(keyPitch,-1,1);
		}


		GameObject tm = GameObject.Find ("Touchomatic");
		if (!keyOverride) {
			if (tm != null) {
				TouchReader tr = tm.GetComponent<TouchReader> ();
				pitch = tr.connectionStdev / 256.0f - 1.0f;
			} 
		} else {
			pitch=keyPitch;
			keyPitch=-1;
//			keyPitch-=Time.deltaTime*3f;
			keyPitch=Mathf.Clamp(keyPitch,-1,1);
		}

		ScoreController sc=aircraftJet.GetComponent<ScoreController>();

		workFromTilt ();
		checkFuelLow ();
		checkCeiling (sc);
		checkForTeleport ();

		if (Input.GetKeyDown(KeyCode.M)) {
				aircraftJet.GetComponent<AeroplaneUserControl2Axis>().fullcontrol = !aircraftJet.GetComponent<AeroplaneUserControl2Axis>().fullcontrol;
		}


		if (running) {

			pitch = Mathf.Clamp (pitch, -1f, 1f);

			Rigidbody rb=aircraftJet.GetComponent<Rigidbody>();
			double f = aircraftJet.GetComponent<ScoreController> ().getFuel ();

			if (f > 0){
				throttle = pitch.Remap (-1f, 1f, -0.0f, 0.5f);
			}
			else{
				throttle = 0f;
			}

			updatePowerGauge (throttle);

			float val = pitch.Remap (-1f, 1f, 0f, 100f);
			val = Mathf.RoundToInt (val);

		//check if we're running here


			//print ("Fuel:" + f);
			if (f > 0) {
				if (sc.getLevel () > 0) {
					rb.AddForce (aircraftJet.transform.up * ((float)((val * 8f) - 200f)));
				} else {
					rb.AddForce (aircraftJet.transform.up * ((float)((val) - 50)));
				}
				GameObject.Find ("SimpleFlame(Red)").GetComponent<ParticleSystem> ().startSize = throttle.Remap (-0.1f, 0.5f, 0f, 0.5f);
				//updateFBR();
				double fueltoKill = ((double)val) / fuel_burn_rate * Time.deltaTime;
				if (sc.getLevel () > 0) {
					sc.modFuel (-fueltoKill);
				}
			} else {
				rb.AddForce (aircraftJet.transform.up * (-100));
				GameObject.Find ("SimpleFlame(Red)").GetComponent<ParticleSystem> ().startSize = 0f;
			}
			// terminal velocity in falling
			if(rb.velocity.y<-150)
			{
				rb.velocity=new Vector3(rb.velocity.x,-150,rb.velocity.z);
			}
			// straighten everything up, stop us going backwards, stop bad things happening generally 
			// stop backwards
			if(rb.velocity.z<0)
			{
				rb.AddForce (new Vector3(0,0,0.1f));
//				rb.velocity=new Vector3(rb.velocity.x,rb.velocity.y,0);
			}
			// stop sideways
			if(rb.velocity.x!=0)
			{
				rb.velocity=new Vector3(rb.velocity.x*0.95f,rb.velocity.y,rb.velocity.z);
			}
			// make angle straighten out
			rb.rotation=Quaternion.Slerp (rb.rotation,Quaternion.Euler (0,0,0),0.01f);
			// stop on level 0 (intro) if we crash
			if(sc.getLevel()==0 && sc.heightFromGround()<5)
			{
				rb.velocity=new Vector3(0,rb.velocity.y,0);
				rb.angularVelocity=new Vector3(0,0,0);
				rb.rotation=Quaternion.Euler(0,0,0);
			}

			if(throttle > 0){
				timeouttime = Time.time;
			}
			if (Time.time - timeouttime > timout){
				GameObject.Find ("Controller").GetComponent<SceneFadeInOut>().EndScene("intro");
			}
		} 
	}
}

public static class ExtensionMethods {
	
	public static float Remap (this float value, float from1, float to1, float from2, float to2) {
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}
	
}
