using System;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

public class NuclearGameCore : MonoBehaviour {

	// Use this for initialization

    private CoolingSystem coolingSystem;
    private ScreenOutput warningOutput;


	void Start ()
	{

	    coolingSystem = CoolingUtils.readFromResource("centralmapPipes");

        
	}

    public ScreenOutput WarningOutput
    {
        set { warningOutput = value; }
    }

    public CoolingSystem CoolingSystem
    {
        get { return coolingSystem; }
    }

    // Update is called once per frame
	void Update () {


	}


    private void warningMessage(String message)
    {
        warningOutput.addLine(System.DateTime.Now.ToString("hh:mm:ss") + ">> " + message + " <<", Color.yellow);
        warningOutput.addLine("", Color.black);
    }
    private void errorMessage(String message)
    {
        warningOutput.addLine(System.DateTime.Now.ToString("hh:mm:ss") + ">>> " + message + " <<<", Color.red);
        warningOutput.addLine("", Color.black);
    }


}
