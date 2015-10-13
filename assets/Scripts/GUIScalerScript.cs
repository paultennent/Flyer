using UnityEngine;
using System.Collections;

public class GUIScalerScript : MonoBehaviour {

	float originalWidth = 1920.0f;  // define here the original resolution
	float originalHeight = 1080.0f; // you used to create the GUI contents 
	private Vector3 scale;
	
	void OnGUI(){
		scale.y = Screen.height/originalHeight; // calculate vert scale	
		scale.x = scale.y; // this will keep your ratio base on Vertical scale	
		scale.z = 1;	
		float scaleX = Screen.width/originalWidth; // store this for translat	
		Matrix4x4 svMat = GUI.matrix; // save current matrix // substitute matrix - only scale is altered from standard	
		GUI.matrix = Matrix4x4.TRS(new Vector3( (scaleX - scale.y) / 2 * originalWidth, 0, 0), Quaternion.identity, scale);
	}
}
