using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartScript_ins : MonoBehaviour {

	double countdownStartTime = 0;
	double delay = 3.0;
	bool endingScene = false;
	// Use this for initialization
	void Start () {
		countdownStartTime = Time.time;
		GameObject.Find ("Inst").GetComponent<Text> ().color = Color.clear;
		GameObject.Find ("Inst2").GetComponent<Text> ().color = Color.clear;
		TouchReader tr = TouchReader.GetReader ();
		if (tr != null) {
			tr.clearClapSensing ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > countdownStartTime + delay){
			GameObject.Find ("Inst").GetComponent<Text> ().color = Color.white;
			GameObject.Find ("Inst2").GetComponent<Text> ().color = Color.white;
			TouchReader tr = TouchReader.GetReader ();
			if (tr != null) {
				if(tr.clapSensed){

						endingScene = true;
						//Application.LoadLevel ("airship-flyer-fuel");

				}
			}
		}

		if (endingScene) {
			GameObject.Find ("Controller").GetComponent<SceneFadeInOut>().EndScene("airship");
		}
	}
}
