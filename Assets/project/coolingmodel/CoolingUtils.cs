using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Random = System.Random;

public class CoolingUtils {

    private enum CoolingElem
    {
        EMPTY,PUMP,PIPE,BEGINNING,ENDING
    }

    static Random _random = new Random();


    public static CoolingSystem readFromResource(String resourceName)
    {
        Texture2D levelBitmap = Resources.Load("centralmapPipes") as Texture2D;
        Color[] pixels = levelBitmap.GetPixels();

        CoolingElem[] elem = fromColorsToElems(pixels);

        bool[] visited = new bool[pixels.Length];
        List<Pipe> sourcePipes = new List<Pipe>();
        List<Pipe> pipes = new List<Pipe>();
        List<Pump> pumps = new List<Pump>();
        List<CoreEntrance> coreEntrances = new List<CoreEntrance>();
        CoolingSystem cooling = new CoolingSystem(sourcePipes, pipes, pumps, coreEntrances);
        //find seeds and create pipes from them
        for (int i = 0; i < levelBitmap.height; i++)
        {
            for (int j = 0; j < levelBitmap.width; j++)
            {
                if (elem[i*levelBitmap.width + j].Equals(CoolingElem.BEGINNING))
                {
                    visited[i*levelBitmap.width + j] = true;
                    Pipe pipe = new Pipe();
                    pipe.Status=Pipe.PipeStatus.FULL;
                    pipe.Layout.Add(new Vector2(j,i));
                    sourcePipes.Add(pipe);
                }
            }
        }


        //iterate until no more are added
        Boolean newPointsAdded = true;
        while (newPointsAdded)

        {
            newPointsAdded = false;

            foreach (Pipe pipe in cooling.SourcePipes)
            {
                List<Vector2> initialLayout = new List<Vector2>(pipe.Layout);
                foreach (Vector2 point in initialLayout)
                {
                    int pointX = (int)point.x;
                    int pointY = (int)point.y;
                    newPointsAdded = newPointsAdded || evaluateFromPipe(visited, pointX - 1, pointY, levelBitmap, elem, pipe, cooling);
                    newPointsAdded = newPointsAdded || evaluateFromPipe(visited, pointX, pointY + 1, levelBitmap, elem, pipe, cooling);
                    newPointsAdded = newPointsAdded || evaluateFromPipe(visited, pointX + 1, pointY, levelBitmap, elem, pipe, cooling);
                    newPointsAdded = newPointsAdded || evaluateFromPipe(visited, pointX, pointY - 1, levelBitmap, elem, pipe, cooling);
                }
            }

            foreach (Pipe pipe in cooling.Pipes)
            {
                List<Vector2> initialLayout = new List<Vector2>(pipe.Layout);
                foreach (Vector2 point in initialLayout)
                {
                    int pointX = (int)point.x;
                    int pointY = (int)point.y;
                    newPointsAdded = newPointsAdded || evaluateFromPipe(visited, pointX - 1, pointY, levelBitmap, elem, pipe, cooling);
                    newPointsAdded = newPointsAdded || evaluateFromPipe(visited, pointX, pointY + 1, levelBitmap, elem, pipe, cooling);
                    newPointsAdded = newPointsAdded || evaluateFromPipe(visited, pointX + 1, pointY, levelBitmap, elem, pipe, cooling);
                    newPointsAdded = newPointsAdded || evaluateFromPipe(visited, pointX, pointY - 1, levelBitmap, elem, pipe, cooling);
                } 
            }



            foreach (Pump pump in cooling.Pumps)
            {
                int pointX = (int)pump.Location.x;
                int pointY = (int)pump.Location.y;
                newPointsAdded = newPointsAdded || evaluateFromPump(visited, pointX, pointY - 1, levelBitmap, elem, pump, cooling);
                newPointsAdded = newPointsAdded || evaluateFromPump(visited, pointX, pointY + 1, levelBitmap, elem, pump, cooling);
                newPointsAdded = newPointsAdded || evaluateFromPump(visited, pointX - 1, pointY, levelBitmap, elem, pump, cooling);
                newPointsAdded = newPointsAdded || evaluateFromPump(visited, pointX + 1, pointY, levelBitmap, elem, pump, cooling);
            }
        }

        return cooling;
    }

    private static bool evaluateFromPipe(bool[] visited, int pointX, int pointY, Texture2D levelBitmap, CoolingElem[] elem,
        Pipe pipe, CoolingSystem cooling)
    {

        bool newAdditions = false;

        if (pointX < 0 || pointY < 0 || pointX >= levelBitmap.width || pointY >= levelBitmap.height)
            return newAdditions;

        if (!visited[pointY*levelBitmap.width + pointX])
        {
            visited[pointY*levelBitmap.width + pointX] = true;
            if (elem[pointY*levelBitmap.width + pointX].Equals(CoolingElem.PIPE))
            {
                pipe.Layout.Add(new Vector2(pointX, pointY));
                newAdditions = true;
            }

            if (elem[pointY * levelBitmap.width + pointX].Equals(CoolingElem.ENDING))
            {
                cooling.CorenEntrances.Add(new CoreEntrance(new Vector2(pointX, pointY)));
                newAdditions = true;
            }

            if (elem[pointY*levelBitmap.width + pointX].Equals(CoolingElem.PUMP))
            {
                Pump pump = new Pump(IDGenerator.generateID(), new Vector2(pointX, pointY), CircuitUtils.generateCircuit());
                pump.ConnectedPipes.Add(pipe);
                pipe.ConnectedPumps.Add(pump);
                cooling.Pumps.Add(pump);
                newAdditions = true;
            }
        }
        return newAdditions;
    }


    private static bool evaluateFromPump(bool[] visited, int pointX, int pointY, Texture2D levelBitmap, CoolingElem[] elem,
        Pump pump, CoolingSystem cooling)
    {
        bool newAdditions = false;
        if (pointX < 0 || pointY < 0 || pointX >= levelBitmap.width || pointY >= levelBitmap.height)
            return newAdditions;

        if (!visited[pointY * levelBitmap.width + pointX])
        {
            visited[pointY * levelBitmap.width + pointX] = true;
            if (elem[pointY * levelBitmap.width + pointX].Equals(CoolingElem.PIPE))
            {
                Pipe pipe = new Pipe();
                pipe.Layout.Add(new Vector2(pointX, pointY));
                cooling.Pipes.Add(pipe);
                pump.ConnectedPipes.Add(pipe);
                pipe.ConnectedPumps.Add(pump);
                newAdditions = true;
            }
        }
        return newAdditions;
    }



    private static CoolingElem[] fromColorsToElems(Color[] pixels)
    {
        CoolingElem[] elem = new CoolingElem[pixels.Length];

        for (int i = 0; i < pixels.Length; i++)
        {
            Color color = pixels[i];

            if (color.r < 0.2 && color.g < 0.2 && color.b < 0.2)
            {
                elem[i] = CoolingElem.EMPTY;
            }

            if (color.r < 0.2 && color.g < 0.2 && color.b > 0.9)
            {
                elem[i] = CoolingElem.BEGINNING;
            }

            if (color.r < 0.2 && color.g > 0.9 && color.b < 0.2)
            {
                elem[i] = CoolingElem.PUMP;
            }

            if (color.r >0.9 && color.g <0.2 && color.b <0.2)
            {
                elem[i] = CoolingElem.ENDING;
            }

            if (color.r > 0.9 && color.g > 0.9 && color.b > 0.9)
            {
                elem[i] = CoolingElem.PIPE;
            }
        }
        return elem;
    }
}
