using UnityEngine;
using System.Collections;
using System;

public class Photobooth : MonoBehaviour {

	int score;
	WebCamTexture m_Cam;

	private bool m_Showing=false;

	// Use this for initialization
	void Start () {
		Renderer renderer = GetComponent<Renderer>();
		renderer.enabled = false;
		//showPhotoBooth ();
	}

	public void showPhotoBooth(int theScore)
	{
		score = theScore;
		if (GameObject.Find ("Controller").GetComponent<HighScores> ().lowScore () <= theScore) {


			m_Showing = true;

			m_Cam = new WebCamTexture ();
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
		}
	}

	string writeSnapshot()
	{
		Texture2D snap = new Texture2D(m_Cam.width, m_Cam.height);
		snap.SetPixels(m_Cam.GetPixels());
		snap.Apply();
		string filename = "c:\\airshipimages\\" + DateTime.Now.ToString ("yyyyMMddHHmmss") + ".png";
		System.IO.File.WriteAllBytes(filename, snap.EncodeToPNG());
		return filename;
	}



}
