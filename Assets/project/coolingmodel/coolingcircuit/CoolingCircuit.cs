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

        public Relee getRelee(String id)
        {
            if (inputs[0].InputRelee.Id.Equals(id)) return inputs[0].InputRelee;
            if (inputs[1].InputRelee.Id.Equals(id)) return inputs[1].InputRelee;
            if (inputs[2].InputRelee.Id.Equals(id)) return inputs[2].InputRelee;


            if (engineInputs[0].Relee.Id.Equals(id)) return engineInputs[0].Relee;
            if (engineInputs[1].Relee.Id.Equals(id)) return engineInputs[1].Relee;
            if (engineInputs[2].Relee.Id.Equals(id)) return engineInputs[2].Relee;

            foreach (Relee relee in connectionRelees)
            {
                if (relee.Id.Equals(id))   return relee;
            }

            return null;
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

            engineInputs[0].Relee.Closed = false;
            engineInputs[1].Relee.Closed = false;
            engineInputs[2].Relee.Closed = false;

        }

    }
}
