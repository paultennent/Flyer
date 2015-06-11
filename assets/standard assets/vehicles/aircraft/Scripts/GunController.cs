using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {

	Transform Effect;
	int damage = 100;
	
	// Update is called once per frame
	void Update () {

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (new Vector3 (Screen.width * 0.5f, Screen.height * 0.5f, 0f));

		if (Input.GetMouseButtonDown(1))
		{
			if(Physics.Raycast(ray, out hit, 1000)){
				print (hit.transform.name);
				hit.transform.SendMessage("ApplyDamage",damage, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
