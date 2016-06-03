using UnityEngine;
using System.Collections;
using System;

public class Photobooth : MonoBehaviour {

	int score;
	WebCamTexture m_Cam;
	public Font myFont;

	private bool m_Showing=false;

	// Use this for initialization
	void Start () {
		Renderer renderer = GetComponent<Renderer>();
		renderer.enabled = false;
	}

	public void showPhotoBooth(int theScore)
	{
		score = theScore;
		if (GameObject.Find ("Controller").GetComponent<HighScores> ().lowScore () <= theScore) {
			GameObject.Find ("PlayerDeathEffect").GetComponent<Renderer> ().enabled = false;
			m_Showing = true;
			WCStaticTextureScript wc = WCStaticTextureScript.GetWC ();
			m_Cam = wc.getTexture ();
			Renderer renderer = GetComponent<Renderer> ();
			renderer.enabled = true;
			renderer.material.mainTexture = m_Cam;
			m_Cam.Play ();
			TouchReader tr = TouchReader.GetReader ();
			if (tr != null) {
				tr.clearClapSensing ();
			}
		} else {
			GameObject.Find ("Controller").GetComponent<NewGameController> ().setEnabled (true);
			GameObject.Find ("Controller").GetComponent<HighScores> ().showHighScores ();
			GameObject.Find ("Controller").GetComponent<HighScores> ().startCountdown ();
		}
	}

	public void hidePhotoBooth()
	{
		m_Showing = false;
		m_Cam.Stop ();
		Renderer renderer = GetComponent<Renderer>();
		renderer.enabled = false;
		GameObject.Find ("PlayerDeathEffect").GetComponent<Renderer> ().enabled = true;
		GameObject.Find ("Controller").GetComponent<NewGameController> ().setEnabled (true);
		GameObject.Find ("Controller").GetComponent<HighScores> ().showHighScores ();
		GameObject.Find ("Controller").GetComponent<HighScores> ().startCountdown ();

	}
	
	// Update is called once per frame
	void Update () {
		if (m_Showing) {
			TouchReader tr = TouchReader.GetReader ();
			if (tr != null) {
				if (tr.clapSensed) {
					// take the picture
					string name=writeSnapshot ();
					GameObject.Find ("Controller").GetComponent<HighScores> ().addScore ((int)score, name);
					tr.clearClapSensing ();
					hidePhotoBooth ();
				}
			}
			if (Input.GetKeyDown ("up")) {
				string name=writeSnapshot ();
				GameObject.Find ("Controller").GetComponent<HighScores> ().addScore ((int)score, name);
				hidePhotoBooth ();
			}
			if(Input.GetKeyDown ("x"))
			{
				hidePhotoBooth ();
			}
		}
	}

	void OnGUI()
	{
		if (m_Showing) {
			float scale=Screen.height / 1080f;
			GUIStyle gs = new GUIStyle();
			gs.font = myFont;
			gs.fontSize = (int)(64f*scale);
			gs.alignment = TextAnchor.MiddleCenter;
			gs.normal.textColor = new Color(239f/255f,224f/255f,185/255f);
			GUI.Label (new Rect (810*scale, 150*scale, 300*scale, 72*scale), "High Five to take your high score photo!",gs);
		}
	}

	string writeSnapshot()
	{
		Texture2D snap = new Texture2D(m_Cam.width, m_Cam.height);
		snap.SetPixels(m_Cam.GetPixels());
		snap.Apply();
		string filename = "C:\\Dropbox\\Airship\\airshipimages\\" + DateTime.Now.ToString ("yyyyMMddHHmmss") + ".png";
		System.IO.File.WriteAllBytes(filename, snap.EncodeToPNG());
		return filename;
	}



}
