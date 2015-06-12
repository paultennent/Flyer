using UnityEngine;
using System.Collections;

public class RotateProp : MonoBehaviour {

	private float rotation;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float throttle = GameObject.Find ("AircraftJet").GetComponent<AeroplaneController> ().Throttle;
		rotation += 1000*throttle;
		transform.localEulerAngles = new Vector3 (0, 0, rotation);
	}
}
