using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

public class NuclearGameCore : MonoBehaviour {

	// Use this for initialization

    private CoolingSystem coolingSystem;

	void Start ()
	{

	    coolingSystem = CoolingUtils.readFromResource("centralmapPipes");

        Debug.Log(coolingSystem);

	}

    public CoolingSystem CoolingSystem
    {
        get { return coolingSystem; }
    }

    // Update is called once per frame
	void Update () {
	
	}
}
