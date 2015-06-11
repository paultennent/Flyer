using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {

	public GameObject projectile;
	public float speed = 2000;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey("space"))
		{
			Transform t = GameObject.Find("AircraftJet").transform;
			Vector3 playerPosition = new Vector3(t.position.x,t.position.y,t.position.z);
			Instantiate(projectile,playerPosition,t.rotation);
		}
	}
}
