using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;

public class StartScript : MonoBehaviour {

	private bool input1Active = false;
	private bool input2Active = false;

	private bool endingScene = false;

	private bool startedHighFive = false;
	
	// Use this for initialization
	void Start () {
		TouchReader tr = TouchReader.GetReader ();
		if (tr != null) {
			tr.clearClapSensing ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!endingScene) {
			GameObject.Find ("PaulCharacter 1").GetComponent<ThirdPersonCharacter> ().Move (new Vector3 (0, 0, 0), !(input1Active && input2Active), false);
			if (input1Active && input2Active) {
				GameObject.Find ("Inst").GetComponent<Text> ().text = "High Five To Launch The Airship";
			} else {
				GameObject.Find ("Inst").GetComponent<Text> ().text = "Grab a handle each to get going";
			}


			GameObject.Find ("emitter").GetComponent<Renderer> ().enabled = (input1Active);
			GameObject.Find ("SimpleFlame(Red)").GetComponent<Renderer> ().enabled = (input1Active);
			GameObject.Find ("VapourTrailSystem").GetComponent<Renderer> ().enabled = (input1Active);

			GameObject.Find ("AfterburnerLeft").GetComponent<Renderer> ().enabled = (input2Active);
			GameObject.Find ("AfterburnerRight").GetComponent<Renderer> ().enabled = (input2Active);

			GameObject.Find ("Prop_01").GetComponent<RotatePropIntro> ().engage (input2Active);
			GameObject.Find ("Prop_02").GetComponent<RotatePropIntro> ().engage (input2Active);


			TouchReader tr = TouchReader.GetReader ();
			if (tr != null) {
				input1Active = tr.leftCapacitive > 100;
				input2Active = tr.rightCapacitive > 100;
				if(tr.clapSensed){
					endingScene = true;
				}
			} else {
				if (Input.GetKey (KeyCode.T)) {
					input1Active = true;
				} else {
					input1Active = false;
				}

				if (Input.GetKey (KeyCode.G)) {
					input2Active = true;
				} else {
					input2Active = false;
				}
				if (Input.GetKeyDown ("space")) {
					//GameObject.Find ("Controller").GetComponent<SceneFadeInOut>().EndScene("instructions");
					//Application.LoadLevel ("airship-flyer-fuel");
					endingScene = true;

				}
			}
			if(input1Active){
				if(!GameObject.Find ("PaulCharacter 1").GetComponent<AudioSource>().isPlaying){
				GameObject.Find ("PaulCharacter 1").GetComponent<AudioSource>().Play();
				}
			}else{
				if(GameObject.Find ("PaulCharacter 1").GetComponent<AudioSource>().isPlaying){
				GameObject.Find ("PaulCharacter 1").GetComponent<AudioSource>().Stop();
				}
			}
			if(input2Active){
				if(!GameObject.Find ("AircraftJet").GetComponent<AudioSource>().isPlaying){
				GameObject.Find ("AircraftJet").GetComponent<AudioSource>().Play();
				}
			}else{
				if(GameObject.Find ("AircraftJet").GetComponent<AudioSource>().isPlaying){
				GameObject.Find ("AircraftJet").GetComponent<AudioSource>().Stop();
				}
			}
		}

		else {
			input1Active = true;
			input2Active = true;
			GameObject.Find ("Controller").GetComponent<SceneFadeInOut>().EndScene("instructions");
		}

	}
}
