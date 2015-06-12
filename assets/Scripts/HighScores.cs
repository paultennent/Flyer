using UnityEngine;
using System.Collections;
using System.Text;
using System.IO; 

public class HighScores : MonoBehaviour {

	int[] scores;
	string[] names;
	GUIStyle smallFont;
	int counter = 0;
	string KEY_NAME = "Highscores";

	bool visible = false;
	// Use this for initialization
	void Start () {
		scores = new int[10] {1,2,3,4,5,6,7,8,9,10};
		names = new string[10] {"Paul","Paul","Paul","Paul","Paul","Paul","Paul","Paul","Paul","Paul"};
		if (PlayerPrefs.HasKey (KEY_NAME)) {
			loadPrefs();
		} else {
			Load ("highscores.txt");
		}
		sortScores ();
		smallFont = new GUIStyle();
		GUI.color = Color.yellow;
		smallFont.fontSize = 32;
		smallFont.alignment = TextAnchor.MiddleCenter;
	}

	private string getHighScoreText(){
		string toReturn = "<color=yellow>";
		toReturn += "<b>High Scores</b>\n\n";
		for (int i=0; i<scores.Length; i++) {
			toReturn += names[i];
			toReturn += ": ";
			toReturn += scores[i];
			toReturn += "\n";
		}
		toReturn += "</color>";
		return toReturn;
	}

	public void showHighScores(){
		visible = true;
	}
	// Update is called once per frame
	void Update () {
		
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

	void OnGUI(){
		if (visible) {
			GUI.Label (new Rect (0, 0, Screen.width, Screen.height), getHighScoreText (),smallFont);
		}
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
}