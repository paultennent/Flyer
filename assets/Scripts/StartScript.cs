using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;

public class StartScript : MonoBehaviour {

	private bool input1Active = false;
	private bool input2Active = false;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {


		GameObject.Find ("PaulCharacter 1").GetComponent<ThirdPersonCharacter> ().Move(new Vector3(0,0,0),!(input1Active && input2Active),false);
		if (input1Active && input2Active) {
			GameObject.Find ("Inst").GetComponent<Text> ().text = "High Five To Launch The Airship";
		} else {
			GameObject.Find ("Inst").GetComponent<Text> ().text = "Grab a handle each to get going";
		}


		GameObject.Find ("emitter").GetComponent<Renderer> ().enabled = (input1Active);
		GameObject.Find ("SimpleFlame(Red)").GetComponent<Renderer> ().enabled = (input1Active);
		GameObject.Find ("VapourTrailSystem").GetComponent<Renderer> ().enabled = (input1Active);

		GameObject.Find ("AfterburnerLeft").GetComponent<Renderer> ().enabled = (input2Active);
		GameObject.Find ("AfterburnerRight").GetComponent<Renderer> ().enabled = (input2Active);

		GameObject.Find ("Prop_01").GetComponent<RotatePropIntro> ().engage (input2Active);
		GameObject.Find ("Prop_02").GetComponent<RotatePropIntro> ().engage (input2Active);


		GameObject tm = GameObject.Find ("Touchomatic");
		if (tm != null) {
			TouchReader tr = tm.GetComponent<TouchReader> ();
			input1Active = tr.leftCapacitive > 100;
			input2Active = tr.rightCapacitive > 100;
			if(tr.connectionStdev>256)
			{
				Application.LoadLevel ("airshipflyer-newGUI");
			}
		} else {
			if (Input.GetKey (KeyCode.T)) {
				input1Active = true;
			} else {
				input1Active = false;
			}

			if (Input.GetKey (KeyCode.G)) {
				input2Active = true;
			} else {
				input2Active = false;
			}
			if (Input.GetKeyDown ("space")) {
				Application.LoadLevel ("airshipflyer-newGUI");
			}
		}

	}
}
