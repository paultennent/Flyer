using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewGameController : MonoBehaviour {


	Vector2 startPosition;
	float startTime;
	double countdownStartTime = 0;
	double delay = 3.0;
	private bool itsenabled = false;
	// Use this for initialization
	void Start () {
	
	}

	public void setEnabled(bool e){
		itsenabled = e;
		countdownStartTime = Time.time;
	}

	// Update is called once per frame
	void Update () {
		if (itsenabled) {
			if(Time.time > countdownStartTime + delay){
				GameObject.Find ("TryAgain").GetComponent<Text> ().color = Color.white;
				GameObject tm = GameObject.Find ("Touchomatic");
				if (tm != null) {
					TouchReader tr = tm.GetComponent<TouchReader> ();
					if( tr.leftCapacitive > 100 && tr.rightCapacitive > 100){
						if(tr.connectionStdev>256)
						{
							//GameObject.Find ("Controller").GetComponent<SceneFadeInOut>().EndScene(
							GameObject.Find ("Controller").GetComponent<SceneFadeInOut>().EndScene("airship-flyer-fuel");
							//Application.LoadLevel ("airship-flyer-fuel");
						}
					}
				}
			}
			else if (Input.GetKeyDown ("space")) {
				Application.LoadLevel ("intro");
			}
			GameObject.Find ("Main Camera").transform.RotateAround (GameObject.Find ("PlayerDeathEffect").transform.position, Vector3.up, 10 * Time.deltaTime);
		
			try{
				if (Input.touchCount > 0 & Input.GetTouch (0).phase == TouchPhase.Ended) {
					Vector2 endPosition = Input.GetTouch (0).position;
					Vector2 delta = endPosition - startPosition;
					
					float dist = Mathf.Sqrt (Mathf.Pow (delta.x, 2) + Mathf.Pow (delta.y, 2));
					float angle = Mathf.Atan (delta.y / delta.x) * (180.0f / Mathf.PI);
					float duration = Time.time - startTime;
					float speed = dist / duration;
					
					// Left to right swipe
					if (startPosition.y < endPosition.y) {
						if (angle < 0)
							angle = angle * 1.0f;
						print ("Distance: " + dist + " Angle: " + angle + " Speed: " + speed);
						
						if (dist > 300 & angle < 10 & speed > 1000) {
							//Application.LoadLevel ("intro-ios");
							GameObject.Find ("Controller").GetComponent<SceneFadeInOut>().EndScene("intro-ios");
						}
					}
				}
				
				if (Input.touchCount > 0 & Input.GetTouch (0).phase == TouchPhase.Began) {
					startPosition = Input.GetTouch (0).position;
					startTime = Time.time;
				}
			}
			catch{
				//probably not using a touch enabled device
			}
		
		}
	}
}
