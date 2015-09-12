using System;
using System.Collections;
using System.Collections.Generic;
using Assets.project.coolingmodel.coolingcircuit;
using UnityEngine;
using Random = System.Random;


public  class CircuitUtils
{

    private static Random rnd = new Random();

    public static int PHASE1_LENGTH = 2;
    public static int PHASE2_LENGTH = 2;
    public static int PHASE3_LENGTH = 2;

    public static int INTERCONNECTIONS = 2;

      public static List<Color> circuitColors = new List<Color>
      {
            new Color(0.5f,0.5f,0.5f),
            new Color(0.75f,0.75f,0.75f),
            new Color(0.5f,0f,0f),
            new Color(1f,0f,0f),
            new Color(0.5f,0.5f,0f),
            new Color(1f,1f,0f),
            new Color(0f,0.5f,0f),
            new Color(0f,1f,0f),
            new Color(0f,0.5f,0.5f),
            new Color(0f,1f,1f),
            new Color(0f,0f,0.5f),
            new Color(0f,0f,1f),
            new Color(0.5f,0f,0.5f),
            new Color(1f,0f,1f),
      };


      public static CoolingCircuit generateCircuit()
    {
        CoolingCircuit circuit = new CoolingCircuit();


        //generate random sequence of Colors;
        List<Color> shuffledColors = new List<Color>(circuitColors);
        shuffle(shuffledColors);
        Queue<Color> queuedColors = new Queue<Color>(shuffledColors); 

          //connect the three phases
          //---------------------------------------------------
        List<Color> colors1 = connectPhase(circuit, queuedColors, 0, PHASE1_LENGTH);
        List<Color> colors2 = connectPhase(circuit, queuedColors, 1, PHASE2_LENGTH);
        List<Color> colors3 = connectPhase(circuit, queuedColors, 2, PHASE3_LENGTH);

          //add confussing connections ;)
          //---------------------------------------------------

          List<Color> usedColors = new List<Color>();
          usedColors.AddRange(colors1);
          usedColors.AddRange(colors2);
          usedColors.AddRange(colors3);

        for (int i = 0; i < INTERCONNECTIONS; i++)
        {
            Color interConnection1 = usedColors[rnd.Next(usedColors.Count)];
            usedColors.Remove(interConnection1);
            Color interConnection2 = usedColors[rnd.Next(usedColors.Count)];
            usedColors.Remove(interConnection2);
            circuit.ConnectionRelees.Add(new Relee(IDGenerator.generateID(), interConnection1, interConnection2));
        }


          return circuit;
    }

      private static List<Color> connectPhase(CoolingCircuit circuit, Queue<Color> shuffledColors, int phase, int phase_length)
    {

          List<Color> usedColors = new List<Color>();

        PhaseInput input = circuit.Inputs[phase];

          Color Color1 = shuffledColors.Dequeue();
          Color Color2 = Color1;
          Relee relee = new Relee(IDGenerator.generateID(), Color.clear, Color1);
            input.InputRelee = relee;

        for (int i = 0; i < phase_length; i++)
        {
            Color2 = shuffledColors.Dequeue();
            usedColors.Add(Color2);
            Relee newRelee = new Relee(IDGenerator.generateID(), Color1, Color2);
            circuit.ConnectionRelees.Add(newRelee);
            Color1 = Color2;
        }

        EngineInput engineInput = circuit.EngineInputs[phase];
        engineInput.Relee = new Relee(IDGenerator.generateID(), Color2, Color.clear);
          return usedColors;
    }


    private static  void shuffle<T>(List<T> list) {
        int n = list.Count;
        Random rnd = new Random();
        while (n > 1) {
            int k = (rnd.Next(0, n) % n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static CircuitStatus calculateStatus(CoolingCircuit circuit)
    {

        List<Relee> allRelees=new List<Relee>(circuit.ConnectionRelees);
        allRelees.Add(circuit.EngineInputs[0].Relee);
        allRelees.Add(circuit.EngineInputs[1].Relee);
        allRelees.Add(circuit.EngineInputs[2].Relee);



        List<Color> colorsWithPower = new List<Color>();
        List<ConnectedRelee> releesWithPower = new List<ConnectedRelee>();
        bool shortCircuit = false;

        colorsWithPower.Add(circuit.Inputs[0].InputRelee.OutColor);
        releesWithPower.Add(new ConnectedRelee(circuit.Inputs[0].InputRelee,0));
        colorsWithPower.Add(circuit.Inputs[1].InputRelee.OutColor);
        releesWithPower.Add(new ConnectedRelee(circuit.Inputs[1].InputRelee,1));
        colorsWithPower.Add(circuit.Inputs[2].InputRelee.OutColor);
        releesWithPower.Add(new ConnectedRelee(circuit.Inputs[2].InputRelee,2));

        List<ConnectedRelee> releesOfLastIteration = new List<ConnectedRelee>(releesWithPower);


        while (releesOfLastIteration.Count>0)
        {
            List<ConnectedRelee> releesOfnewIteration = new List<ConnectedRelee>();

            foreach (ConnectedRelee relee in releesOfLastIteration)
            {
                Color destinationColor = relee.Relee.OutColor;

                foreach (Relee destinationRelee in allRelees)
                {
                    if (destinationRelee.InColor.Equals(destinationColor))
                    {
                        if (!releesWithPower.Contains(new ConnectedRelee(destinationRelee,relee.Phase)))
                        {

                            releesWithPower.Add(new ConnectedRelee(destinationRelee,relee.Phase));
                            releesOfnewIteration.Add(new ConnectedRelee(destinationRelee,relee.Phase));
                        }
                        else
                        {
                            shortCircuit = true;
                        }
                    }
                }
            }

            releesOfLastIteration = releesOfnewIteration;
        }


        CircuitStatus.Status status;

        if (shortCircuit) status = CircuitStatus.Status.SHORTCIRCUIT;
        else
        {
            List<ConnectedRelee> engineRelees = new List<ConnectedRelee>();

            foreach (ConnectedRelee relee in releesWithPower)
            {
                if (relee.Relee.Equals(circuit.EngineInputs[0].Relee)) engineRelees.Add(relee);
                if (relee.Relee.Equals(circuit.EngineInputs[1].Relee)) engineRelees.Add(relee);
                if (relee.Relee.Equals(circuit.EngineInputs[2].Relee)) engineRelees.Add(relee);
            }

            if (engineRelees.Count != 3) {status = CircuitStatus.Status.OFF;}
            {
                if ((1 >> engineRelees[0].Phase) + (1 >> engineRelees[1].Phase) +
                    (1 >> engineRelees[2].Phase) != 7)
                {
                    status = CircuitStatus.Status.BAD_PHASE;
                }
                else
                {
                    status = CircuitStatus.Status.ON;
                }
            }
        }

        return new CircuitStatus(releesWithPower, status);
    }

    }

