using UnityEngine;
using System.Collections;
using System.Text;
using System.IO; 
using UnityEngine.UI;

public class HighScores : MonoBehaviour {

	int[] scores;
	string[] names;
	Texture2D[] pics;
	int counter = 0;
	string KEY_NAME = "Highscores";
	bool counting = false;
	double countTimerStart = 0;
	double backToIntroDelay = 20;
	bool endingScene = false;
	bool showscores = false;
	public Font myFont;

	
	// Use this for initialization
	void Start () {
		scores = new int[10] {0,0,0,0,0,0,0,0,0,0};
		names = new string[10] {"","","","","","","","","",""};
		pics = new Texture2D[10];
		if (PlayerPrefs.HasKey (KEY_NAME)) {
			loadPrefs();
		} else {
			//Load ("highscores.txt");
		}
		sortScores ();
		GameObject tm = GameObject.Find ("Touchomatic");
		if (tm != null) {
			TouchReader tr = tm.GetComponent<TouchReader> ();
			tr.clearClapSensing ();
		}
	}

	public void startCountdown(){
		countTimerStart = Time.time;
		counting = true;
	}

	private string getHighScoreText(){
		//string toReturn = "High Scores\n\n";
		string toReturn = "High Scores\n\n";
		for (int i=0; i<scores.Length; i++) {
			toReturn += names[i];
			toReturn += ": ";
			toReturn += scores[i];
			toReturn += "\n";
		}
		return toReturn;
	}

	public void showHighScores(){
		System.Diagnostics.Process.Start ("C:\\stopobs.ahk");
		//GameObject.Find ("HighScores").GetComponent<Text> ().text = getHighScoreText();
		loadTextures ();
		showscores = true;
	}
	// Update is called once per frame
	void Update () {
		if (counting) {

			if(Time.time > countTimerStart + backToIntroDelay){
				endingScene = true;
			}
		}
		if (endingScene) {
			GameObject.Find ("Controller").GetComponent<SceneFadeInOut>().EndScene("intro");
		}
	}

	void OnGUI()
	{
		if (showscores) {
			//GameObject.Find ("GameOver").GetComponent<Text> ().color = Color.clear;
	
			float scale=Screen.height / 1080f;

			GUIStyle gs = new GUIStyle();
			gs.font = myFont;
			gs.fontSize = (int)(64f*scale);
			gs.alignment = TextAnchor.MiddleCenter;
			gs.normal.textColor = Color.white;
			GUI.Label (new Rect (0*scale, 80*scale, 1920*scale, 72*scale), "High scores",gs);
			gs.alignment = TextAnchor.MiddleLeft;

			for (int i=0; i<names.Length; i++) {
				if (pics[i] != null) {
					GUI.DrawTexture (new Rect (837*scale, (150 + (i * 80))*scale, 128*scale, 72*scale), pics[i], ScaleMode.ScaleToFit, true);
					GUI.Label (new Rect (987*scale, (150 + (i * 80))*scale, 300*scale, 72*scale), scores [i].ToString (),gs);
				}
			}
		}
	}

	public int lowScore()
	{
		return scores [scores.Length - 1];
	}

	public void addScore(int score, string name){
		if (score < scores [scores.Length-1]) {
			return;
		} else {
			scores[scores.Length-1] = score;
			names[names.Length-1] = name;
		}
		sortScores ();
		savePrefs ();
	}

	private string getScoresForSave(){
		string s = "";
		for (int i=0; i<scores.Length; i++) {
			s += names[i];
			s += ",";
			s += scores[i];
			s += "\n";
		}
		return s;
	}

	private void handle(string s){
		//print ("Handling:" + s);
		string[] split = s.Split(",".ToCharArray());
		scores [counter] = int.Parse (split [1]);
		names [counter] = split [0];
		counter++;
	}

	private void sortScores(){
		bool foundone = true; 
		while(foundone) { 
			foundone = false; 
			for(int i = 0; i < scores.Length - 1; i++) { 
				if(scores[i] < scores[i + 1]) { 
					string tempobj = names[i + 1]; 
					int tempfloat = scores[i + 1]; 
					names[i + 1] = names[i]; 
					scores[i + 1] = scores[i]; 
					names[i] = tempobj; 
					scores[i] = tempfloat; 
					foundone = true; } 
			} 
		}
	}

	private void loadPrefs(){
		//print ("loading prefs:" + KEY_NAME + getScoresForSave ());
		string s = PlayerPrefs.GetString (KEY_NAME);
		string[] lines = s.Split ("\n".ToCharArray ());
		counter = 0;
		for (int i=0; i<lines.Length-1; i++) {
			handle (lines[i]);
		}
	}

	private void savePrefs(){
		//print ("saving prefs:" + KEY_NAME + getScoresForSave ());
		PlayerPrefs.SetString(KEY_NAME, getScoresForSave());
	}

	private bool Load(string fileName)
	{
		try
		{
			string line;
			StreamReader theReader = new StreamReader(fileName, Encoding.Default);
			using (theReader)
			{
				do
				{
					line = theReader.ReadLine();
					if (line != null)				{
						handle(line);
					}
				}
				while (line != null);
				theReader.Close();
				return true;
			}
		}
		catch
		{
			print ("Something went wrong reading high scores");
			return false;
		}
	}

	void loadTextures(){
		for (int i=0; i<names.Length; i++) {
			pics [i] = LoadTex (names [i], 128, 72);
		}
	}

	public static Texture2D LoadTex(string filePath, int w, int h) {
		
		Texture2D tex = null;
		byte[] fileData;
		
		if (File.Exists(filePath))     {
			fileData = File.ReadAllBytes(filePath);
			tex = new Texture2D(w, h);
			tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
		}
		return tex;
	}
}