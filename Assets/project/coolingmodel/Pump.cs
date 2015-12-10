
using System;
using System.Collections.Generic;
using Assets.project.coolingmodel.coolingcircuit;
using UnityEngine;


public class Pump
{

    public enum PumpStatus { NO_WATER, NO_POWER, READY, WORKING, ERROR};

    private string stationID;
   
    private List<Pipe> connectedPipes;
    private Vector2 location;
    private float radiation;
    private CoolingCircuit circuit;
    private bool started = false;


    public Pump(string stationId, Vector2 location, CoolingCircuit circuit)
    {
        stationID = stationId;
        this.location = location;
       
        this.circuit = circuit;
        this.connectedPipes = new List<Pipe>();
    }

    public PumpStatus Status
    {
        get
        {
            bool hasWater = false;
            foreach (Pipe pipe in connectedPipes)
            {
                if (pipe.Status.Equals(Pipe.PipeStatus.FULL)) hasWater = true;
            }

            if (!hasWater) return PumpStatus.NO_WATER;

            CircuitStatus status = CircuitUtils.calculateStatus(circuit);


            if (!status.CircuitStatus1.Equals(CircuitStatus.Status.ON)) return PumpStatus.NO_POWER;

            if (started) return PumpStatus.WORKING;

            return PumpStatus.READY;
        }
    }

    public bool start()
    {
        if (!this.Status.Equals(PumpStatus.READY))   return false;

        foreach (Pipe pipe in connectedPipes)
        {
           pipe.Status = Pipe.PipeStatus.FULL;
        }

        started = true;
        return true;
    }



    public CoolingCircuit Circuit
    {
        get { return circuit; }
    }


    public float Radiation
    {
        get { return radiation; }
        set { radiation = value; }
    }

    public string StationId
    {
        get { return stationID; }
    }

    public List<Pipe> ConnectedPipes
    {
        get { return connectedPipes; }
    }

    public Vector2 Location
    {
        get { return location; }
    }
}
