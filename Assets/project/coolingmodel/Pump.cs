
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


            if (!status.Equals(CircuitStatus.Status.ON)) return PumpStatus.NO_POWER;



            return PumpStatus.READY;
        }
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
