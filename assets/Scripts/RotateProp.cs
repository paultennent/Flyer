using UnityEngine;
using System.Collections;

public class RotateProp : MonoBehaviour {

	private float rotation;
	private float lastThrot = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float throttle = GameObject.Find ("Controller").GetComponent<InputController> ().getThrottle();
			lastThrot = 0.8f * lastThrot + 0.1f * throttle;
			lastThrot = Mathf.Clamp (lastThrot, 0.05f, 1f);
			rotation += 100 * lastThrot;
			
//			lastThrot - throttle;
//		} else {
//
//		}
		transform.localEulerAngles = new Vector3 (0, 0, rotation);
	}
}
