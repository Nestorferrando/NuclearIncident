

using System.Collections.Generic;

public class CoolingSystem
{

    private List<Pipe> sourcePipes;
    private List<Pipe> pipes;
    private List<Pump> pumps;
    private List<CoreEntrance> corenEntrances;

    public CoolingSystem(List<Pipe> sourcePipes, List<Pipe> pipes, List<Pump> pumps, List<CoreEntrance> corenEntrances)
    {
        this.sourcePipes = sourcePipes;
        this.pipes = pipes;
        this.pumps = pumps;
        this.corenEntrances = corenEntrances;

    }

    public List<Pipe> Pipes
    {
        get { return pipes; }
    }

    public List<Pump> Pumps
    {
        get { return pumps; }
    }

    public List<Pipe> SourcePipes
    {
        get { return sourcePipes; }
    }

    public List<CoreEntrance> CorenEntrances
    {
        get { return corenEntrances; }
    }
}
