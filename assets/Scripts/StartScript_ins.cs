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
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > countdownStartTime + delay){
			GameObject.Find ("Inst").GetComponent<Text> ().color = Color.white;
			GameObject tm = GameObject.Find ("Touchomatic");
			if (tm != null) {
				TouchReader tr = tm.GetComponent<TouchReader> ();
				if( tr.leftCapacitive > 100 && tr.rightCapacitive > 100){
					if(tr.connectionStdev>256)
					{

						endingScene = true;
						//Application.LoadLevel ("airship-flyer-fuel");
					}
				}
			}
		}

		if (endingScene) {
			GameObject.Find ("Controller").GetComponent<SceneFadeInOut>().EndScene("airship");
		}
	}
}
