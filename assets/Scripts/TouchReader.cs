using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.IO;
using System.Threading;
using System;

public class TouchReader : MonoBehaviour
{
	
	private static bool created = false;
	
	void Awake() {
		if (!created) {
			// this is the first instance - make it persist
			DontDestroyOnLoad(this.gameObject);
			created = true;
		} else {
			// this must be a duplicate from a scene reload - DESTROY!
			Destroy(this.gameObject);
		} 
	}
	
	
	public string port_name = "COM2";
	private string opened_port = "";
	private SerialPort comport = null;
	private Thread serialThread = null;
	private readonly object syncLock = new object ();
	private readonly object portLock = new object ();
	[HideInInspector]
	public int
		leftCapacitive;
	[HideInInspector]
	public int
		rightCapacitive;
	[HideInInspector]
	public int
		connectionMean;
	[HideInInspector]
	public int
		connectionStdev;
	[HideInInspector]
	public int
		connectionVariance;
	private string lastLineRead;
	private bool threadEnding;


	Boolean sentFirst=false;
	// Update is called once per frame
	void Update ()
	{
		if (opened_port != port_name || comport == null) {
				try {
					comport = new SerialPort ("\\\\.\\" + port_name, 9600, Parity.None, 8, StopBits.One);
					comport.ReadTimeout = 1;
					comport.Open ();
					opened_port = port_name;
				sentFirst=false;
				} catch (IOException e) {
					comport.Close ();
					comport = null;
					Debug.Log (e);
				}
		} 

		if (sentFirst) {
			try
			{
				string line = comport.ReadLine ();
				if (line != null) {
					string[] values = line.Split (' ');
					//print (line);
					if (values.Length == 5) {
						leftCapacitive = int.Parse (values [0]);
						rightCapacitive = int.Parse (values [1]);
						connectionMean = int.Parse (values [2]);
						connectionVariance = int.Parse (values [3]);
						connectionStdev = int.Parse (values [4]);
					}
				}
			}catch(TimeoutException e)
			{

			}
		}
		// poll arduino
		comport.Write ("p");
		sentFirst = true;
	}
}
