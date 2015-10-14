using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;

public class StartScript : MonoBehaviour {

	private bool input1Active = false;
	private bool input2Active = false;

	private bool endingScene = false;
	private bool warningScreenActive = false;

	private bool startedHighFive = false;
	
	// Use this for initialization
	void Start () {
		// FORCE RESOLUTION
		//Screen.SetResolution (960, 540, true);
		Cursor.visible = false;
		TouchReader tr = TouchReader.GetReader ();
		if (tr != null) {
			tr.clearClapSensing ();
		}
	}

	public void endScene(){
		endingScene = true;
	}

	public void restartScene(){
		GameObject.Find ("Controller").GetComponent<SceneFadeInOut>().EndScene("intro");
	}
	
	// Update is called once per frame
	void Update () {
		if (!endingScene) {
			if (!warningScreenActive) {
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
					if (tr.clapSensed) {
						//endingScene = true;
						warningScreenActive = true;
						GameObject.Find ("PhotoPlane").GetComponent<WarningCamera> ().showPhotoBooth ();
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
				}

				//allow keypress effect
				if (Input.GetKeyDown ("down")) {
					warningScreenActive = true;
					GameObject.Find ("PhotoPlane").GetComponent<WarningCamera> ().showPhotoBooth ();
				}

				if (input1Active) {
					if (!GameObject.Find ("PaulCharacter 1").GetComponent<AudioSource> ().isPlaying) {
						GameObject.Find ("PaulCharacter 1").GetComponent<AudioSource> ().Play ();
					}
				} else {
					if (GameObject.Find ("PaulCharacter 1").GetComponent<AudioSource> ().isPlaying) {
						GameObject.Find ("PaulCharacter 1").GetComponent<AudioSource> ().Stop ();
					}
				}
				if (input2Active) {
					if (!GameObject.Find ("AircraftJet").GetComponent<AudioSource> ().isPlaying) {
						GameObject.Find ("AircraftJet").GetComponent<AudioSource> ().Play ();
					}
				} else {
					if (GameObject.Find ("AircraftJet").GetComponent<AudioSource> ().isPlaying) {
						GameObject.Find ("AircraftJet").GetComponent<AudioSource> ().Stop ();
					}
				}

				//allow clearing high scores (might be necessary);
				if (Input.GetKeyDown (KeyCode.O)) {
					PlayerPrefs.DeleteAll();
				}
			}
		}

		else {
			if (Input.GetKeyDown ("down")) {
				warningScreenActive = true;
				GameObject.Find ("PhotoPlane").GetComponent<WarningCamera> ().showPhotoBooth ();
			}
			input1Active = true;
			input2Active = true;
			GameObject.Find ("Controller").GetComponent<SceneFadeInOut>().EndScene("airship");
		}

	}
}
