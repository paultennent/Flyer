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
				if(Vector3.Distance(radarObjects[i].transform.position, transform.position) > switchDistance){
					helpTransform.LookAt(radarObjects[i].transform);
					borderObjects[i].transform.position = transform.position + switchDistance*helpTransform.forward;
					borderObjects[i].layer = LayerMask.NameToLayer("Radar");
					radarObjects[i].layer = LayerMask.NameToLayer("Invisible");
				}
				else{
					borderObjects[i].layer = LayerMask.NameToLayer("Invisible");
					radarObjects[i].layer = LayerMask.NameToLayer("Radar");
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
	}
}
