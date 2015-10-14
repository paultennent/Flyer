using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class WarningCamera : MonoBehaviour {

	WebCamTexture m_Cam;
	public Font myFont;
	private float startTime;
	private float timeout = 10f;
	public Texture2D warningLogo;
	
	private bool m_Showing=false;
	
	// Use this for initialization
	void Start () {
		Renderer renderer = GetComponent<Renderer>();
		renderer.enabled = false;
		//showPhotoBooth ();
	}
	
	public void showPhotoBooth()
	{
		startTime = Time.time;
		m_Showing = true;
		WCStaticTextureScript wc = WCStaticTextureScript.GetWC ();
		m_Cam = wc.getTexture ();

		Renderer renderer = GetComponent<Renderer> ();
		renderer.enabled = true;
		renderer.material.mainTexture = m_Cam;
		//Vector2 scale=renderer.material.GetTextureScale ("_MainTex");
		//print (scale);
		//renderer.material.SetTextureScale("_MainTex", new Vector2(-scale.x,scale.y));
		m_Cam.Play ();
		TouchReader tr = TouchReader.GetReader ();
		if (tr != null) {
			tr.clearClapSensing ();
		}
	}
	
	public void hidePhotoBooth()
	{
		m_Showing = false;
		m_Cam.Stop ();
		Renderer renderer = GetComponent<Renderer>();
		renderer.enabled = false;	
	}
	
	// Update is called once per frame
	void Update () {
		if (m_Showing) {
			TouchReader tr = TouchReader.GetReader ();
			if (tr != null) {
				if (tr.clapSensed) {
					tr.clearClapSensing ();
					hidePhotoBooth ();
					GameObject.Find ("Controller").GetComponent<StartScript>().endScene();
				}
			}
			if (Input.GetKeyDown ("up")) {
				hidePhotoBooth ();
				GameObject.Find ("Controller").GetComponent<StartScript>().endScene();
			}
		}
	}
	
	void OnGUI()
	{
		if (m_Showing) {

			GameObject.Find ("Name").GetComponent<Text> ().color = Color.clear;
			GameObject.Find ("Title").GetComponent<Text> ().color = Color.clear;
			GameObject.Find ("Inst").GetComponent<Text> ().color = Color.clear;
			float scale=Screen.height / 1080f;

			GUIStyle gs = new GUIStyle();
			gs.font = myFont;
			gs.fontSize = (int)(64f*scale);
			gs.alignment = TextAnchor.MiddleCenter;
			gs.normal.textColor = new Color(239f/255f,224f/255f,185/255f);


			GUI.DrawTexture (new Rect (835*scale, 50*scale, 250*scale, 218*scale), warningLogo, ScaleMode.ScaleToFit, true);

			GUI.Label (new Rect (0*scale, 350*scale, 1920*scale, 72*scale), "Warning",gs);
			GUI.Label (new Rect (0*scale, 500*scale, 1920*scale, 72*scale), "This Game Records A Video of You Playing",gs);
			GUI.Label (new Rect (0*scale, 650*scale, 1920*scale, 72*scale), "High Five if you are happy to go ahead",gs);
			GUI.Label (new Rect (0*scale, 800*scale, 1920*scale, 72*scale), "Leave now if you do not want to be recorded",gs);

			if(Time.time > startTime + timeout){
				GameObject.Find ("Controller").GetComponent<StartScript>().restartScene();
			}
		}
	}
	

	
	
	
}
