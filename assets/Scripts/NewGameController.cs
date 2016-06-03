using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewGameController : MonoBehaviour {


	Vector2 startPosition;
	float startTime;
	double countdownStartTime = 0;
	double delay = 3.0;
	private bool itsenabled = false;
	private bool endingScene = false;
	TouchReader tr;
	// Use this for initialization
	void Start () {
		tr = TouchReader.GetReader ();
	}

	public void setEnabled(bool e){
		itsenabled = e;
		countdownStartTime = Time.time;
	}

	// Update is called once per frame
	void Update () {
		if (itsenabled) {
			if(Time.time > countdownStartTime + delay){
				GameObject.Find ("TryAgain").GetComponent<Text> ().color = new Color(239f/255f,224f/255f,185/255f);
				TouchReader tr = TouchReader.GetReader ();
				if (tr != null) {
					if(tr.clapSensed)
					{
						endingScene=true;
					}
				}
				if(Input.GetKeyDown("up"))
				{
					endingScene=true;
				}
			}
			else{
				if (tr != null) {
					tr.clearClapSensing ();
				}
				endingScene = false;
			}
			GameObject.Find ("Main Camera").transform.RotateAround (GameObject.Find ("PlayerDeathEffect").transform.position, Vector3.up, 10 * Time.deltaTime);
		
			if (endingScene) {
				GameObject.Find ("Controller").GetComponent<SceneFadeInOut>().EndScene("airship");
			}
		}
	}
}
