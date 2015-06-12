using UnityEngine;
using System.Collections;

public class AttachSelfToRadar : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject.Find ("AircraftJet").GetComponent<RadarScript> ().addToRadar (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
