using UnityEngine;
using System.Collections;

public class ScoreController : MonoBehaviour {

	int level = 0;
	double score = 0;
	int health = 100;


	//terrain controller stuff
	// Value ranges.
	public float maxDetail = 0.04f;
	public float minDetail = 0.04f;
	public float minHeight = 0f;
	public float maxHeight = 100f;
	
	private float detailScale = 0.004f;
	private float heightScale = 0f;

	private string DEFAULT_NAME = "Paul";

	private double lastLevelChangeTime = 0;
	private double levelChangeInterval = 15.0;

	private double levelChangeWarningInterval = 3;

	private TerrainGenerator generator;

	private bool gameRunning = true;

	public GameObject boom;
	
	// Use this for initialization
	void Start () {
		generator = GameObject.Find ("Terrain Generator").GetComponent<TerrainGenerator>();
		startGame ();
	}

	public void startGame(){
		detailScale = 0.004f;
		heightScale = 0f;
		lastLevelChangeTime = Time.time;
		GameObject.Find ("GameOver").GetComponent<TextMesh> ().color = Color.clear;

		GameObject.Find ("Main Camera").transform.parent = GameObject.Find ("AircraftJet").transform;
		generator.Generate (detailScale, heightScale);
	}

	// Update is called once per frame
	void Update () {

		if (gameRunning) {

			RaycastHit hit;
			float distanceToGround = 0;
		
			if (Physics.Raycast (transform.position, -Vector3.up, out hit, 100.0F))
				distanceToGround = hit.distance;

			double val = 0;
			if (distanceToGround > 0) {
				val = 100 - distanceToGround;
			}
			//print (val);

			if (level > 0){
				score = score + (Time.deltaTime * val);
				int intscore = (int)score;
				GameObject.Find ("Score").GetComponent<TextMesh> ().text = "Score: " + intscore;
			}

			if (Time.time > lastLevelChangeTime + levelChangeInterval) {
				level += 1;
				GameObject.Find ("Level").GetComponent<TextMesh> ().text = "Level: " + level;
				lastLevelChangeTime = Time.time;
				heightScale += 10;
				generator.Generate(detailScale, heightScale);
				GameObject.Find ("Controller").GetComponent<NewPlaneController> ().addEnemies(level);
			}

			if (Time.time > lastLevelChangeTime + levelChangeInterval - levelChangeWarningInterval) {
				GameObject.Find ("ReadyLevel").GetComponent<TextMesh> ().text = "Ready for Level: " + (level + 1);
				GameObject.Find ("ReadyLevel").GetComponent<TextMesh> ().color = Color.yellow;
			} else {
				GameObject.Find ("ReadyLevel").GetComponent<TextMesh> ().color = Color.clear;
			}
		} else {
			GameObject.Find ("GameOver").GetComponent<TextMesh> ().color = Color.yellow;
		}

	}

	void destroyPlane(){
		GameObject.Find ("GameOver").GetComponent<TextMesh> ().color = Color.yellow;
		GameObject.Find ("Main Camera").transform.parent = GameObject.Find ("Owner").transform;
		GameObject myBoom; 
		myBoom = (GameObject) Instantiate (boom, transform.position, Quaternion.identity);
		myBoom.name = "PlayerDeathEffect";
		myBoom.GetComponent<AudioSource> ().PlayOneShot (myBoom.GetComponent<AudioSource> ().clip);
		GameObject.Find ("Controller").GetComponent<NewGameController> ().setEnabled (true);
		GameObject.Find ("Controller").GetComponent<HighScores> ().showHighScores ();
		GameObject.Find ("Controller").GetComponent<HighScores> ().addScore ((int)score, DEFAULT_NAME);

		GameObject.Find ("AircraftJet").SetActive (false);

	}

	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.name == "Terrain Plane(Clone)")
		{
			if(gameRunning){
				if (level > 0){
					health = health - 10;
				}
			GameObject.Find("Health").GetComponent<TextMesh>().text = "Heatlh: "+health;
			if (health <= 0){
				gameRunning=false;
				destroyPlane();
			}
			}
		}
	}

}
