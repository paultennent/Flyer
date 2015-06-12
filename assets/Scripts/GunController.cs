using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {
	
	public Transform Effect;
	public int damage = 100;
	
	// Update is called once per frame
	void Update () {
		
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (new Vector3 (Screen.width * 0.5f, Screen.height * 0.5f, 10f));
		
		if (Input.GetMouseButton (1)) {
			if(!GameObject.Find ("MyGun").GetComponent<AudioSource> ().isPlaying){
				GameObject.Find ("MyGun").GetComponent<AudioSource> ().Play ();
			}
			if (Physics.Raycast (ray, out hit, 1000)) {
				//print (hit.transform.name);
				Transform particleClone = (Transform)Instantiate (Effect, hit.point, Quaternion.LookRotation (hit.normal));
				Destroy (particleClone.gameObject, 0.5f);
				hit.transform.SendMessage ("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
			}
		} else {
			if(GameObject.Find ("MyGun").GetComponent<AudioSource> ().isPlaying){
				GameObject.Find ("MyGun").GetComponent<AudioSource> ().Stop();
			}
		}

	}
}
