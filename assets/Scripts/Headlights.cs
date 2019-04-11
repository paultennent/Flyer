using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.IO;


public class Headlights : MonoBehaviour
{
	
	private static bool created = false;
	private static Headlights singleton=null;

	public static Headlights GetHeadlights()
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

    private bool inGame=false;
    
    private float timeSinceLast=0f;
    private bool cycleLights=false;
    private float lastLevel=0;
    private float thisLevel=0;
	private string port_name="COM4";
	private SerialPort comport = null;
    private bool portOpen=false;
    private int test_level=0;
    private float test_counter=0f;
    void Update ()
    {
        timeSinceLast+=Time.deltaTime;
        if(!portOpen)
        {
            comport = new SerialPort ("\\\\.\\" + port_name, 9600, Parity.None, 8, StopBits.One);
            comport.ReadTimeout = 10;
            comport.Open();
            SetLights(-1);
            portOpen=true;
        }
        if(cycleLights)
        {
            test_counter+=Time.deltaTime*100f;
            if(test_counter>=256)test_counter=0f;
            if((int)test_counter!=test_level)
            {
                test_level=(int)test_counter;
                thisLevel=test_level;
                lastLevel=test_level;
            }            
        }else
        {
            if(thisLevel<lastLevel)
            {
                lastLevel= Mathf.Max(lastLevel-Time.deltaTime*255f,thisLevel);                
            }else if(thisLevel>lastLevel)
            {
                lastLevel= Mathf.Min(lastLevel+Time.deltaTime*255f,thisLevel);                
            }
        }

        if(timeSinceLast>0.1f)
        {
            timeSinceLast=0f;
            if(inGame)
            {
                byte[] vals={(byte)'p',(byte)lastLevel,(byte)'\n'};
                comport.Write(vals,0,3);
                //print(lastLevel+":"+thisLevel);
            }
        }        
    }

    public void SetHighscore()
    {
        byte[] vals={(byte)'h',(byte)0,(byte)'\n'};
        comport.Write(vals,0,3);
        inGame=false;
        print("SCORE");
    }

    public void SetDead()
    {
        byte[] vals={(byte)'d',(byte)0,(byte)'\n'};
        comport.Write(vals,0,3);
        inGame=false;
        print("DEAD");
    }

    public void SetIntro(int val)
    {
        byte[] vals={(byte)'t',(byte)val,(byte)'\n'};
        comport.Write(vals,0,3);
        inGame=false;
        print("INTRO:"+val);
    }

    public void SetLights(int level)
    {
        if(level<0 || level>255)
        {
            inGame=false;
//            cycleLights=true;
            SetIntro(0);
        }else
        {
            inGame=true;
            cycleLights=false;
            thisLevel=level;
        }
    }
    
}
