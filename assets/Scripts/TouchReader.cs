﻿using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.IO;
using System.Threading;
using System;

public class TouchReader : MonoBehaviour
{
	
	private static bool created = false;
	private static TouchReader singleton=null;

	public static TouchReader GetReader()
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
	[HideInInspector]
	public Boolean
		clapSensed = false;

	private Boolean sensedTouch = false;
	Boolean sentFirst = false;

	public void clearClapSensing ()
	{
		clapSensed = false;
		sensedTouch = false;
	}

	void Start ()
	{
		string[] arguments = Environment.GetCommandLineArgs ();
		if (arguments.Length > 1) {
			port_name = arguments [1];
		}
	}
	// Update is called once per frame
	void Update ()
	{
		if (opened_port != port_name || comport == null) {
			try {
				comport = new SerialPort ("\\\\.\\" + port_name, 9600, Parity.None, 8, StopBits.One);
				comport.ReadTimeout = 1;
				comport.Open ();
				opened_port = port_name;
				sentFirst = false;
			} catch (IOException e) {
				comport.Close ();
				comport = null;
				Debug.Log (e);
			}
		} 

		if (sentFirst) {
			try {
				string line = comport.ReadLine ();
				if (line != null) {
					string[] values = line.Split (' ');
					//print (line);
					if (values.Length == 5) {
						try {
							leftCapacitive = int.Parse (values [0]);
							rightCapacitive = int.Parse (values [1]);
							connectionMean = int.Parse (values [2]);
							connectionVariance = int.Parse (values [3]);
							connectionStdev = int.Parse (values [4]);
							if (connectionStdev > 200) {
								sensedTouch = true;
							} else if (connectionStdev < 50 && sensedTouch) {
								clapSensed = true;
							}
						
						} catch {
							//print("Error getting data:" + values);
						}
					}
				}
			} catch (TimeoutException e) {

			}
		}
		// poll arduino
		comport.Write ("p");
		sentFirst = true;
	}
}
