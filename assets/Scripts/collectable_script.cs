using UnityEngine;
using System.Collections;

public class collectable_script : MonoBehaviour {

	// Use this for initialization
	float speed = 1.0f;
	bool given = false;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (GameObject.Find ("AircraftJet") != null) {
			transform.parent = GameObject.Find ("AircraftJet").transform;
			Vector3 pos = transform.localPosition;
			pos.x = pos.x * 0.99f;
			transform.localPosition = pos;
			transform.parent = GameObject.Find ("Collectables").transform;
		}

		// keep balloons always in front of player
	//	Vector3 pos = transform.position;
	//	pos.y = pos.y + speed * Time.deltaTime;
	//	transform.position = pos;
	}
	
	void OnCollisionEnter (Collision col)
	{	
			if (col.gameObject.name == "AircraftJet") {
				//print("collected");
				
				if(!given){
					given = true;
					col.gameObject.GetComponent<AudioSource> ().Play ();
					GameObject.Find ("Controller").GetComponent<CollectableGenerator> ().removeCollectable (gameObject);
					//col.gameObject.GetComponent<ScoreController> ().addPoints (50);
					col.gameObject.GetComponent<ScoreController> ().modFuel (10);
					col.gameObject.GetComponent<ScoreController> ().balloonHit();
					Destroy (gameObject);
				}
				
			}
		}

}
