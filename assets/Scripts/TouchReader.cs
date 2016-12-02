using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.IO;
using System.Threading;
using System;
using System.Text;
using System.Net.Sockets;
using System.Net;

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
	
	//public string port_name = "COM2";
	private string port_name;
	private string opened_port = "";
	private SerialPort comport = null;
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
	public bool receivedReading=false;

	[HideInInspector]
	public int
		connectionVariance;
	private string lastLineRead;
	[HideInInspector]
	public Boolean
		clapSensed = false;

	enum ClapStage
	{
		SENSED_NONE,
		SENSED_NO_TOUCH,
		SENSED_TOUCH,
		SENSED_RELEASE
	};
	float noTouchTimer=0;

	private Boolean allowSinglePlayer=false;

	private ClapStage m_Stage=ClapStage.SENSED_NONE;


	Boolean sentFirst = false;

	public void clearClapSensing ()
	{
		m_Stage = ClapStage.SENSED_NONE;
		clapSensed = false;
		noTouchTimer = 0;
	}

	private StreamWriter m_Logfile;
	float m_LogTime;

	public void StartLogFile(string name)
	{
		m_LogTime = Time.time;
		m_Logfile = new StreamWriter (name);
		m_Logfile.WriteLine ("time,leftCapacitive,rightCapacitive,connectionStdev");
	}

	public void StopLogFile()
	{
		if (m_Logfile != null) {
			m_Logfile.Close ();
			m_Logfile = null;
		}
	}


	void Start ()
	{
		string[] arguments = Environment.GetCommandLineArgs ();
		if (arguments.Length > 1) {
			port_name = arguments [1];
		}
	}

	private bool USE_UDP=false;

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown ("1")) {
			allowSinglePlayer=true;
			print("One player mode enabled");
		}

		if (USE_UDP) {
			UpdateUdp ();
		} else {
			UpdateComPortDirect ();
		}
	}

	const int LISTEN_PORT = 11123;

    public Socket udpReceiver;    
    
	IAsyncResult receiveResult=null;
	EndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, LISTEN_PORT);
    byte[] receiveBytes=new byte[128];

	private void UpdateUdp()
	{
		if (udpReceiver == null) {
			udpReceiver = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			udpReceiver.Bind (new IPEndPoint (IPAddress.Any, LISTEN_PORT));
		}
        while(udpReceiver.Available>2)
        {
            int len=udpReceiver.ReceiveFrom(receiveBytes,ref remoteIpEndPoint);
            string receiveString = Encoding.ASCII.GetString(receiveBytes);
            if(receiveString!=null)
            {
                HandleLine(receiveString);
            }
        }
/*		if (receiveResult == null) {
			receiveResult = udpReceiver.BeginReceive (null, null);
		} 
		if(receiveResult!=null && receiveResult.IsCompleted)
		{
			Byte[] receiveBytes=udpReceiver.EndReceive(receiveResult,ref endpoint);
			receiveResult=null;
			string receiveString = Encoding.ASCII.GetString(receiveBytes);
			if(receiveString!=null)
			{
				HandleLine(receiveString);
			}
		}*/

	}

	private void UpdateComPortDirect()
	{
		if (opened_port != port_name || comport == null) {
			try {
				port_name = Load ("comport-data.config");
				comport = new SerialPort ("\\\\.\\" + port_name, 9600, Parity.None, 8, StopBits.One);
				comport.ReadTimeout = 10;
				comport.Open ();
				opened_port = port_name;
				sentFirst = false;
			} catch (IOException e) {
				comport.Close ();
				comport = null;
				//Debug.Log (e);
			}
		} else {
			if (sentFirst) {
				try {
					string line = comport.ReadLine ();
					if (line != null) {
						try {
							HandleLine(line);
								
							} catch {
								//print("Error getting data:" + values);
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

	private void HandleLine(String line)
	{
		receivedReading = true;
		string[] values = line.Split (' ');
		if (values.Length == 5) {
            try
            {
                leftCapacitive = int.Parse (values [0]);
                rightCapacitive = int.Parse (values [1]);
                connectionMean = int.Parse (values [2]);
                connectionVariance = int.Parse (values [3]);
                connectionStdev = int.Parse (values [4]);
            }catch(FormatException e)
            {
                print(line);
            }
			if (m_Logfile != null) {
				m_Logfile.WriteLine ((Time.time - m_LogTime) + "," + leftCapacitive + "," + rightCapacitive + "," + connectionStdev);
			}
			switch (m_Stage) {
			case ClapStage.SENSED_NONE:
				// this code forces two players, not one person holding both handles
				if (((leftCapacitive > 100 && rightCapacitive > 100) || allowSinglePlayer) && connectionStdev < 50) {
					noTouchTimer += Time.deltaTime;
					if (noTouchTimer > 0.1f) {
						m_Stage = ClapStage.SENSED_NO_TOUCH;
						print ("NT");
						
					}
				} else {
					noTouchTimer = 0;
				}
				break;
			case ClapStage.SENSED_NO_TOUCH:
				if (connectionStdev > 200) {
					print ("SNT");
					m_Stage = ClapStage.SENSED_TOUCH;
				}
				break;
			case ClapStage.SENSED_TOUCH:
				if (connectionStdev < 50) {
					print ("SR");
					m_Stage = ClapStage.SENSED_RELEASE;
					clapSensed = true;
				}
				break;
			}
		}
	}

	private string Load(string fileName)
	{
		try
		{
			string line;
			StreamReader theReader = new StreamReader(fileName, Encoding.Default);
			using (theReader)
			{
				line = theReader.ReadLine();
				if(line.StartsWith("UDP"))
				{
					print("UDP com reader");
					USE_UDP=true;
				}
				theReader.Close();
			}
			return line;
		}
		catch
		{
			print ("Something went wrong reading com port from the file");
			return "COM3";
		}
	}
}
