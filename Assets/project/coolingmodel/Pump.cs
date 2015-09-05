
using System.Collections.Generic;
using Assets.project.coolingmodel.coolingcircuit;
using UnityEngine;


public class Pump
{

    public enum PipeStatus { NO_WATER, OFF, READY, ACTIVATED, ERROR};

    private string PumpID;
    private PipeStatus status;
    private List<Pipe> connectedPipes;
    private Vector2 location;
    private float radiation;
    private CoolingCircuit circuit;


    public Pump(string pumpId, Vector2 location, CoolingCircuit circuit)
    {
        PumpID = pumpId;
        this.location = location;
        this.status = PipeStatus.OFF;
        this.circuit = circuit;
        this.connectedPipes = new List<Pipe>();
    }

    public PipeStatus Status
    {
        get { return status; }
        set { status = value; }
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

    public string PumpId
    {
        get { return PumpID; }
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
