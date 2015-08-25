using System.Collections.Generic;
using UnityEngine;


public class Pipe  {

    public enum PipeStatus { FULL,EMPTY};

    private PipeStatus status;
    private List<Vector2> layout;
    private List<Pump> connectedPumps;



    public Pipe()
    {
        this.status = PipeStatus.EMPTY;
        this.layout = new List<Vector2>();
        this.connectedPumps = new List<Pump>();
    }

    public List<Pump> ConnectedPumps
    {
        get { return connectedPumps; }
    }

    public List<Vector2> Layout
    {
        get { return layout; }
    }


    public PipeStatus Status
    {
        get { return status; }
        set { status = value; }
    }
}
