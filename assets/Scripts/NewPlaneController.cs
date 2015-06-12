using UnityEngine;
using System.Collections;

public class NewPlaneController : MonoBehaviour {

	public GameObject enemy;
	public Transform spawnPoint;

	private int enemyCount = 0;
	private int lastEnemyCount = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (enemyCount < 1) {
			addEnemies(lastEnemyCount + 1);
		}
	}

	public void enemyDestroyed(){
		enemyCount -= 1;
		GameObject.Find ("AircraftJet").GetComponent<ScoreController>().addPoints (1000);
	}

	public void addEnemies(int count){
		for (int i=0; i<count; i++) {
			GameObject baddie;

			Vector3 bposition = spawnPoint.position;
			bposition.x = bposition.x - (float) (100*i);
			bposition.y = bposition.y + (float) (100*i);

			baddie = Instantiate (enemy, bposition, Quaternion.identity) as GameObject;
			baddie.GetComponent<AeroplaneAiControl>().SetTarget(GameObject.Find ("AircraftJet").transform);
			//need to set me as the target or they'll just fall to earth
			enemyCount += 1;
		}
		lastEnemyCount = enemyCount;
	}
}
