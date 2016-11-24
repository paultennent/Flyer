using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectableGenerator : MonoBehaviour {

	private double nextTime;
	private List<GameObject> al;
	bool disabled = true;

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

	public void setEnabled(){
		disabled = false;
	}

	// optimise the position of the balloon - returns true if it is not in a valley
	bool optimizeBalloonPos(GameObject nc)
	{
		Vector3 pos = nc.transform.position;
		// go up 1000, then drop to ground + 50

		pos.y+=1000;
		nc.transform.position = pos;

		int layerMask=1<<10;
		RaycastHit hit;
		Vector3 hitFrom=nc.transform.position;
		hitFrom.y-=100;

		float distanceToGround = 0;
		if(Physics.Raycast(hitFrom,Vector3.down,out hit,Mathf.Infinity/*,layerMask*/))
		{
			Vector3 newPos=hit.point;
			newPos.y+=50f;
//			print (newPos.y);
			nc.transform.position=newPos;
		}
		// now check forwards if we are in a valley within the nearest 400 units and bring up until it is above that high point
		for (int c=0; c<5; c++) {
			hitFrom = nc.transform.position;
			if (Physics.Raycast (hitFrom, Vector3.forward, out hit, 400/*,layerMask*/)) {
				Vector3 newPos = nc.transform.position;
				newPos.y = hit.point.y + 10;
				nc.transform.position = newPos;
//				print ("in valley");
			}else
			{
				return true;
			}
		}
		return true;
	}
		
		// Update is called once per frame
	void Update () {
		if (! disabled) {
			if (Time.time > nextTime || Input.GetKeyDown("b")) {
				//add the collectable
				nextTime = Time.time + (double)Random.Range (5f, 15f);
				Vector3 pos = GameObject.Find ("AircraftJet").transform.position;
				GameObject nc = Instantiate (collectable, pos, Quaternion.identity) as GameObject;
				nc.transform.parent = GameObject.Find ("AircraftJet").transform;
				pos = nc.transform.localPosition;
//				print(nc.transform.position+":air");

				pos.z = pos.z + Random.Range (300f, 500f);
				nc.transform.localPosition = pos;
				nc.transform.parent=null;
				// make sure balloon is low down but not stuck in a deep valley
				optimizeBalloonPos(nc);

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
