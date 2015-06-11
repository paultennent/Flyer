using UnityEngine;
using System.Collections;

public class BulletMove : MonoBehaviour {

	public int bulletRange;
	public float bulletSpeed;
	public GameObject shooter;

	private Vector3 startPos;
	private Quaternion rotation;
	
	// Use this for initialization
	void Start () {
		shooter = GameObject.FindGameObjectWithTag("Player");
		startPos = shooter.transform.position;
		rotation = shooter.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		float distance = Vector3.Distance(startPos,transform.position);
		Debug.Log(distance);
		float move = bulletSpeed * Time.deltaTime;
		transform.Translate(transform.forward * move);
		if(distance >=bulletRange)
		{
			Destroy(gameObject);
		}
	}
}