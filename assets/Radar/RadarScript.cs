using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RadarScript : MonoBehaviour {

	//public GameObject[] trackedObjects;
	List<GameObject> radarObjects;
	public GameObject blipPrefab;
	public GameObject playerBlipPrefab;
	List<GameObject> borderObjects;
	public float switchDistance;
	public Transform helpTransform;
	public GameObject radarPrefab;

	// Use this for initialization
	void Start () {
		createRadarObjects ();
	}
	
	// Update is called once per frame
	void Update () {
		int counter = radarObjects.Count;
		for (int i=0; i< counter; i++) {

			if (radarObjects[i] == null){
				//object has been destroyed
				radarObjects.RemoveAt(i);
				borderObjects.RemoveAt(i);
				counter--;
			}
			else{
				Vector3 planeVec = new Vector3(radarObjects[i].transform.position.x, transform.position.y, radarObjects[i].transform.position.z);
				if(Vector3.Distance(planeVec, transform.position) > switchDistance){
					helpTransform.LookAt(planeVec);
					borderObjects[i].transform.position = transform.position + switchDistance*helpTransform.forward;
					borderObjects[i].layer = LayerMask.NameToLayer("Radar");
					radarObjects[i].layer = LayerMask.NameToLayer("Invisible");
					//print ("Blip" + Vector3.Distance(radarObjects[i].transform.position, transform.position));
				}
				else{
					borderObjects[i].layer = LayerMask.NameToLayer("Invisible");
					radarObjects[i].layer = LayerMask.NameToLayer("Radar");
					//print ("Real" + Vector3.Distance(radarObjects[i].transform.position, transform.position));
				}
			}
		}
	}

	public void addToRadar(GameObject o){
		if(radarObjects == null){
			radarObjects = new List<GameObject> ();
		}
		if (borderObjects == null) {
			borderObjects = new List<GameObject> ();
		}

		GameObject k = Instantiate(blipPrefab,o.transform.position, Quaternion.identity) as GameObject;
		k.transform.parent = o.transform;
		radarObjects.Add(k);
		GameObject j = Instantiate(blipPrefab,o.transform.position, Quaternion.identity) as GameObject;
		j.transform.parent = o.transform;
		borderObjects.Add (j);
	}

	void createRadarObjects(){

		if(radarObjects == null){
			radarObjects = new List<GameObject> ();
		}
		if (borderObjects == null) {
			borderObjects = new List<GameObject> ();
		}

		//create mine
		GameObject m = Instantiate(playerBlipPrefab,transform.position, Quaternion.identity) as GameObject;
		m.transform.parent = transform;

		GameObject r = Instantiate(radarPrefab,transform.position, Quaternion.identity) as GameObject;
		r.transform.parent = transform;
	}
}
