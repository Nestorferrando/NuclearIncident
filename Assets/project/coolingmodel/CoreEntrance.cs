
using System.Collections.Generic;
using UnityEngine;


public class CoreEntrance
{

    private Vector2 location;
    private List<Pipe> connectingPipes; 

    public CoreEntrance(Vector2 location)
    {
        connectingPipes = new List<Pipe>();
    }

    public Vector2 Location
    {
        get { return location; }
    }

    public List<Pipe> ConnectingPipes
    {
        get { return connectingPipes; }
    }
}
