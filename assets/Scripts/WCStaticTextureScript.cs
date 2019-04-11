using UnityEngine;
using System.Collections;

public class WCStaticTextureScript : MonoBehaviour {

	WebCamTexture m_Cam;
	private static bool created = false;
	private static WCStaticTextureScript singleton=null;
	
	public static WCStaticTextureScript GetWC()
	{
		return singleton;
	}
	
	void Awake ()
	{
		if (!created) {
			// this is the first instance - make it persist
			DontDestroyOnLoad (this.gameObject);
			created = true;
			singleton=this;
		} else {
			// this must be a duplicate from a scene reload - DESTROY!
			Destroy (this.gameObject);
		} 
	}

	// Use this for initialization
	void Start () {
		WebCamDevice[] devices = WebCamTexture.devices;
		int driverToUse = 0;
/*		for (var i = 0; i < devices.Length; i++) {
			if (devices[i].name != "SplitCam Video Driver"){
				print ("Found non splitcam driver");
				driverToUse = i;
			}
		}*/
        print ("Camera:"+devices [driverToUse].name);
		m_Cam = new WebCamTexture ();
		m_Cam.deviceName = devices [driverToUse].name;
	}

	public WebCamTexture getTexture(){
		return m_Cam;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
