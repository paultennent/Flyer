using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectableGenerator : MonoBehaviour {

	private double nextTime;
	private List<GameObject> al;
	bool disabled = false;

	public GameObject collectable;
	// Use this for initialization
	void Start () {
		nextTime = Time.time + 5;
		al = new List<GameObject> ();
	}
	
	public void removeCollectable(GameObject go){
		al.Remove (go);
	}

	public void setDisabled(){
		disabled = true;
	}


	// Update is called once per frame
	void Update () {
		if (! disabled) {
			if (Time.time > nextTime) {
				//add the collectable
				nextTime = Time.time + (double)Random.Range (5f, 15f);
				Vector3 pos = GameObject.Find ("AircraftJet").transform.position;
				GameObject nc = Instantiate (collectable, pos, Quaternion.identity) as GameObject;
				nc.transform.parent = GameObject.Find ("AircraftJet").transform;
				pos = nc.transform.localPosition;

				pos.z = pos.z + Random.Range (300f, 500f);
				pos.y = pos.y + Random.Range (-50f, 0f);
				//pos.x = pos.x + Random.Range (-10f, 10f);

				nc.transform.localPosition = pos;
				nc.name = "Collectable";
				nc.transform.parent = GameObject.Find ("Collectables").transform;
				al.Add (nc);
				//print ("making new collectable");
			}
			//cull
			for (int i = 0; i< al.Count; i++) {
				GameObject go = al [i];
				if (Vector3.Distance (go.transform.position, GameObject.Find ("AircraftJet").transform.position) > 1000) {
					al.RemoveAt (i);
					Destroy (go);
					//print ("culling");
					i--;
				}
			}
		}

	}
}
