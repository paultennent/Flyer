using UnityEngine;
using System.Collections;

public class NewPlaneController : MonoBehaviour {

	public GameObject enemy;
	public Transform spawnPoint;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void addEnemies(int count){
		for (int i=0; i<count; i++) {
			GameObject baddie;
			baddie = Instantiate (enemy, spawnPoint.position, Quaternion.identity) as GameObject;
			//need to set me as the target or they'll just fall to earth
		}
	}
}
