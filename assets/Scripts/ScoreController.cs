using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class ScoreController : MonoBehaviour {

	int level = 0;
	double lastDingScore = 0;
	double dingPoint = 3;
	double score = 0;
	int health = 100;
	double fuel = 100;
	int balloonsHit=0;
	float scoring_leeway = 30;
	public Transform Effect;


	//terrain controller stuff
	// Value ranges.
	public float maxDetail = 0.04f;
	public float minDetail = 0.04f;
	public float minHeight = 0f;
	public float maxHeight = 100f;
	
	private float detailScale = 0.004f;
	public float heightScale = 0;

	private string DEFAULT_NAME = "Paul";


	private float lastScoreTime = 0;

	private double lastLevelChangeTime = 0;
	private double levelChangeInterval = 25.0;

	private double levelChangeWarningInterval = 3;

	private TerrainGenerator generator;

	private bool gameRunning = true;

	public GameObject boom;
	private bool hideTouch = false;

	private string m_LogBaseName;
	private GameObject myBoom; 

	// Use this for initialization
	void Start () {
		Cursor.visible = false;
		generator = GameObject.Find ("Terrain Generator").GetComponent<TerrainGenerator>();
		startGame ();
		System.Diagnostics.Process.Start ("C:\\Dropbox\\Airship\\startobs.ahk");
	}

	public bool showScoreTimeout()
	{
		if (gameRunning && Time.time - lastScoreTime > 5.0) {
			return true;
		}
		return false;
	}

	public void balloonHit ()
	{
		balloonsHit += 1;
	}

	public void modFuel(double f){
		fuel += f;
		if (fuel < 0.0) {
			fuel = 0.0;
		}
	}

	public double getFuel(){
		return fuel;
	}

	public int getLevel(){
		return level;
	}

	public bool isGameRunning(){
		return gameRunning;
	}

	private StreamWriter m_Logfile;
	float m_LogTime;

    float m_DistanceToGround=0;
    
    public float heightFromGround()
    {
        return m_DistanceToGround;
    }
    
	public void startGame(){
		balloonsHit = 0;
		detailScale = 0.004f;
		heightScale = 0f;
		lastLevelChangeTime = Time.time-2;
		lastScoreTime = Time.time;
		GameObject.Find ("GameOver").GetComponent<Text> ().color = Color.clear;
		GameObject.Find ("TryAgain").GetComponent<Text> ().color = Color.clear;
		GameObject.Find ("TooHigh").GetComponent<Text> ().color = Color.clear;

		GameObject.Find ("Main Camera").transform.parent = GameObject.Find ("AircraftJet").transform;
		generator.Generate (detailScale, heightScale);

		m_LogBaseName="C:\\Dropbox\\Airship\\airshiplogs\\"+System.DateTime.Now.ToString ("yyyyMMdd-HHmmss"); 
		TouchReader.GetReader().StartLogFile (m_LogBaseName+"-touch.csv");
		m_Logfile = new StreamWriter (m_LogBaseName+"-game.csv");
		m_Logfile.WriteLine ("time,level,health,fuel,score,balloons");
		m_LogTime = Time.time;
	}

	// Update is called once per frame
	void Update () {

		if (gameRunning) {
			if (Input.GetKeyDown ("l")) {
				lastLevelChangeTime = Time.time - levelChangeInterval;
			}


			RaycastHit hit;
		
			int layerMask=1<<10;
			if(Physics.Raycast(GameObject.Find ("AircraftJet").transform.position,Vector3.down,out hit,Mathf.Infinity,layerMask))
			{
				m_DistanceToGround = hit.distance;
			}else
			{
				m_DistanceToGround = GameObject.Find ("AircraftJet").transform.position.y + 50f;
			}


			double val = 0;
			if (m_DistanceToGround > 0) {
				val = level * 10 - (m_DistanceToGround - scoring_leeway);
				if (val < 0) {
					val = 0;
				}else{
					lastScoreTime=Time.time;
				}
			}


			if (level > 0) {
				if (m_Logfile != null) {
					m_Logfile.WriteLine ((Time.time - m_LogTime) + "," + level + "," + health + "," + fuel + "," + score + "," + balloonsHit);
				}
				GameObject.Find ("ScoreAura").GetComponent <ParticleSystem> ().emissionRate = (float)val * 2f;//.enableEmission=(val>0);
				score = score + (Time.deltaTime * val);
				if (score > lastDingScore + dingPoint) {
					GameObject.Find ("PaulCharacter 1").GetComponent<AudioSource> ().Play ();
					lastDingScore = score;			
				}
			} else {
				GameObject.Find ("ScoreAura").GetComponent <ParticleSystem> ().emissionRate = 0;
			}

			if (level == 0) {
				lastScoreTime = Time.time;
				if (!hideTouch) {

					int tutorialPosition = (int)((Time.time - lastLevelChangeTime) / 3f);
					string showString = "";
					string [] tutorialTexts = {
						"Hold the handles and touch Each Other Gently To Fly",
						"The Harder you touch the higher you fly",
						"Flying higher burns more fuel",
						"You get more fuel each level",
						"Fly very low to collect coins for points",
						"As the levels go up the terrain gets harder",
						"Bonus balloons give you extra fuel",
						"Good Luck"
					};
					if (tutorialPosition < tutorialTexts.Length) {
						showString = tutorialTexts [tutorialPosition];
					}
					if (m_DistanceToGround < 1) {
						lastLevelChangeTime += Time.deltaTime;
						showString = "Touch each other to fly";
						//					lastLevelChangeTime=Time.time;
					}
					GameObject.Find ("ReadyLevel").GetComponent<Text> ().color = new Color (239f / 255f, 224f / 255f, 185 / 255f);
					GameObject.Find ("ReadyLevel").GetComponent<Text> ().text = showString;
				}
			}
			if (Time.time > lastLevelChangeTime + levelChangeInterval) {
				level += 1;
				GameObject.Find ("Controller").GetComponent<CollectableGenerator> ().setEnabled ();
				if (level == 1) {
					levelChangeInterval -= 5;
				}
				fuel += 20;
				GameObject.Find ("Level").GetComponent<Text> ().text = "Level: " + level;
				lastLevelChangeTime = Time.time;
				heightScale += 10;
				generator.Generate (detailScale, heightScale);
			}

			if (Time.time > lastLevelChangeTime + levelChangeInterval - levelChangeWarningInterval) {
				hideTouch = true;
				GameObject.Find ("ReadyLevel").GetComponent<Text> ().text = "Ready for Level " + (level + 1);
				GameObject.Find ("ReadyLevel").GetComponent<Text> ().color = new Color (239f / 255f, 224f / 255f, 185 / 255f);
			} else {
				if (hideTouch) {
					GameObject.Find ("ReadyLevel").GetComponent<Text> ().color = Color.clear;
				}
			}

			//score=0;
			//destroyPlane();
		} else {
			GameObject.Find ("GameOver").GetComponent<Text> ().color = new Color (239f / 255f, 224f / 255f, 185 / 255f);
			GameObject.Find ("TryAgain").GetComponent<Text> ().color = new Color (239f / 255f, 224f / 255f, 185 / 255f);
		}

		int intscore = (int)score;
		GameObject.Find ("Score").GetComponent<Text> ().text = "Score: " + intscore;

		int intfuel = (int)fuel;
		GameObject.Find ("Fuel").GetComponent<Text> ().text = "Fuel: " + intfuel;



	}

	public void addPoints(int points){
		score = score + (double) points;
	}

	void destroyPlane(){
		TouchReader.GetReader ().StopLogFile ();

		if (m_Logfile != null) {
			m_Logfile.Close ();
			m_Logfile = null;
		}




		GameObject.Find ("Controller").GetComponent<InputController> ().setRunning (false);
		GameObject.Find ("GameOver").GetComponent<Text> ().color = new Color(239f/255f,224f/255f,185/255f);
		GameObject.Find ("Main Camera").transform.parent = GameObject.Find ("Owner").transform;

		myBoom = (GameObject) Instantiate (boom, transform.position, Quaternion.identity);
		myBoom.name = "PlayerDeathEffect";
		myBoom.GetComponent<AudioSource> ().PlayOneShot (myBoom.GetComponent<AudioSource> ().clip);
		GameObject.Find ("ReadyLevel").GetComponent<Text> ().color = Color.clear;

		GameObject.Find ("Score").GetComponent<Text> ().color = Color.clear;
		GameObject.Find ("Health").GetComponent<Text> ().color = Color.clear;
		GameObject.Find ("Level").GetComponent<Text> ().color = Color.clear;
		GameObject.Find ("Fuel").GetComponent<Text> ().color = Color.clear;
		GameObject.Find ("Altitude").GetComponent<Text> ().color = Color.clear;


		GameObject.Find ("Controller").GetComponent<CollectableGenerator> ().setDisabled ();
		GameObject.Find ("AircraftJet").SetActive (false);
		GameObject.Find ("dial").SetActive (false);
		enterHighScore ();

	}

	void enterHighScore()
	{
		GameObject.Find ("PhotoPlane").GetComponent<Photobooth> ().showPhotoBooth ((int)score);
	}

	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.name == "Terrain Plane(Clone)")
		{
			if(gameRunning){
				if (level > 0){
					health = health - 20;
					GameObject.Find("CrashSoundEmitter").GetComponent<AudioSource>().Play();
				}
			GameObject.Find("Health").GetComponent<Text>().text = "Health: "+health;
			if (health <= 0){
				gameRunning=false;
				destroyPlane();
			}
			}
		}
	}

}
