using UnityEngine;
using System.Collections;

public class DamageReceiver : MonoBehaviour {

	int health = 100;
	public GameObject boom;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void ApplyDamage(int damage){
		health -= damage;
		checkForDeath ();
	}

	void checkForDeath(){
		if (health <= 0){
			GameObject myBoom; 
			myBoom = (GameObject) Instantiate (boom, transform.position, Quaternion.identity);
			myBoom.name = "PlayerDeathEffect";
			myBoom.GetComponent<AudioSource> ().PlayOneShot (myBoom.GetComponent<AudioSource> ().clip);
			Destroy(myBoom,1);
			Destroy(gameObject);
		}
	}
}
