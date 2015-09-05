using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.project.coolingmodel.coolingcircuit
{
    public class CoolingCircuit
    {
        private PhaseInput[] inputs;
        private List<Relee> connectionRelees;
        private EngineInput[] engineInputs;

        public CoolingCircuit()
        {
            inputs= new PhaseInput[3];
            engineInputs = new EngineInput[3];
            connectionRelees=new List<Relee>();

            inputs[0] = new PhaseInput();
            inputs[1] = new PhaseInput();
            inputs[2] = new PhaseInput();

            engineInputs[0] = new EngineInput();
            engineInputs[1] = new EngineInput();
            engineInputs[2] = new EngineInput();
        }


        public PhaseInput[] Inputs
        {
            get { return inputs; }
        }

        public List<Relee> ConnectionRelees
        {
            get { return connectionRelees; }
        }

        public EngineInput[] EngineInputs
        {
            get { return engineInputs; }
        }

        public void reset()
        {
            inputs[0].InputRelee.Closed = false;
            inputs[1].InputRelee.Closed = false;
            inputs[2].InputRelee.Closed = false;

            foreach (Relee relee in connectionRelees)
            {
                relee.Closed = false;
            }

            engineInputs[0].TRelee.Closed = false;
            engineInputs[1].TRelee.Closed = false;
            engineInputs[2].TRelee.Closed = false;

        }

    }
}
