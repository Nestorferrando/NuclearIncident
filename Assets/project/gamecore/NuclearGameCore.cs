using System;
using System.Collections.Generic;
using UnityEngine;

public class NuclearGameCore : MonoBehaviour {

	// Use this for initialization

    private CoolingSystem coolingSystem;
    private ScreenOutput output;

    private Pump pumpToStart;

    private Reactor reactor;

    private RadiationPumps radiationPumps;

    private float lastMessageTimeStamp = 5;

    private float lastReactorEvaluationTimeStamp;


	void Start ()
	{
	    coolingSystem = CoolingUtils.readFromResource("centralmapPipes");
        reactor = new Reactor();
        radiationPumps = new RadiationPumps(coolingSystem.Pumps);

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
	void Update ()
	{
	    if (Time.realtimeSinceStartup - lastMessageTimeStamp > 10)
	    {
	        lastMessageTimeStamp = Time.realtimeSinceStartup;
	    }

        if (Time.realtimeSinceStartup - lastReactorEvaluationTimeStamp > Reactor.ERROR_INTERVAL_SECONDS)
        {

            if (reactor.getCurrentTemp() > Reactor.DANGER_CORE_TEMP)
            {
                errorMessage("Critical reactor overheat!!! Current temp: " + (int)reactor.getCurrentTemp() + " F");
            }
            else { 

            if (reactor.getCurrentTemp() > Reactor.WARNING_CORE_TEMP)

            {
                warningMessage("Core reactor overheating. Current temp: " + (int)reactor.getCurrentTemp()+" F");
            }
            }

            lastReactorEvaluationTimeStamp = Time.realtimeSinceStartup;
        }


       List<RadiationPumps.RadiationResult> results=radiationPumps.updateRadiations();

	    for (int i = 0; i < results.Count; i++)
	    {
	        switch (results[i])
	        {
	            case RadiationPumps.RadiationResult.OK:
	                break;

                case RadiationPumps.RadiationResult.RADIATION_HIGH:
                    warningMessage("Abnormal radiation on pump " + coolingSystem.Pumps[i].StationId);
                    break;

                case RadiationPumps.RadiationResult.RADIATION_CRITICAL:
                    errorMessage("High radiation on pump " + coolingSystem.Pumps[i].StationId);
                    break;
                case RadiationPumps.RadiationResult.RADIATION_MAX:
                    errorMessage("Radiation levels on pump " + coolingSystem.Pumps[i].StationId + " reached critical values. Aplying contention measures");
                    break;
	        }
	    }
	}

    public void startOverHeat()
    {
        Invoke("performOverheat", 10.0f);
    }

    private void performOverheat()
    {
        reactor.startOverHeating();
        radiationPumps.startRadiation();
        initialOverHeatMessage();
    }

    private void initialOverHeatMessage()
    {
        errorMessage("Water levels of the main cooling system low");
        Invoke("overHeatMessage2", 2.0f);
    }

    private void overHeatMessage2()
    {
        warningMessage("Detected overheating of the Main reactor");
        warningMessage("It is mandatory to activate auxiliary pump systems");
        infoMessage("Critical temp of VVER-1000 type reactor is: 1000F");
    }


    public void startPump(Pump pump)
    {
        this.pumpToStart = pump;
        Invoke("_startPump", 3.0f);
    }

    private void _startPump()
    {
        infoMessage("Pump " + pumpToStart.StationId + " online");
        pumpToStart.start();
    }

    private void warningMessage(String message)
    {
        output.addLine(System.DateTime.Now.ToString("hh:mm:ss") + ">> WARNING: " + message + " <<", Color.yellow);
        output.addLine("", Color.black);
    }
    private void errorMessage(String message)
    {
        output.addLine(System.DateTime.Now.ToString("hh:mm:ss") + ">>> DANGER: " + message + " <<<", Color.red);
        output.addLine("", Color.black);
    }

    private void infoMessage(String message)
    {
        output.addLine(System.DateTime.Now.ToString("hh:mm:ss") + "> INFO: " + message + " <", ScreenOutput.DEFAULT_CONSOLE_COLOR);
        output.addLine("", Color.black);
    }




}
