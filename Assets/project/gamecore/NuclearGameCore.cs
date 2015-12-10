using System;
using UnityEngine;

public class NuclearGameCore : MonoBehaviour {

	// Use this for initialization

    private CoolingSystem coolingSystem;
    private ScreenOutput output;

    private Pump pumpToStart;


	void Start ()
	{
	    coolingSystem = CoolingUtils.readFromResource("centralmapPipes");
	}

    public ScreenOutput WarningOutput
    {
        set { output = value; }
    }

    public CoolingSystem CoolingSystem
    {
        get { return coolingSystem; }
    }

    // Update is called once per frame
	void Update () {


	}

    public void startPump(Pump pump)
    {
        this.pumpToStart = pump;
        Invoke("_startPump", 3.0f);
    }

    private void _startPump()
    {
        output.addLine("Pump " + pumpToStart.StationId + " online", ScreenOutput.DEFAULT_CONSOLE_COLOR);
        output.addLine("", ScreenOutput.DEFAULT_CONSOLE_COLOR);
        pumpToStart.start();
    }

    private void warningMessage(String message)
    {
        output.addLine(System.DateTime.Now.ToString("hh:mm:ss") + ">> " + message + " <<", Color.yellow);
        output.addLine("", Color.black);
    }
    private void errorMessage(String message)
    {
        output.addLine(System.DateTime.Now.ToString("hh:mm:ss") + ">>> " + message + " <<<", Color.red);
        output.addLine("", Color.black);
    }


}
