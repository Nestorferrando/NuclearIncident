using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


   public class CircuitStatus
    {

       public enum Status { OFF, ON, BAD_PHASE, SHORTCIRCUIT };


       private List<ConnectedRelee> releesWithPower;

       private Status circuitStatus;

       public CircuitStatus(List<ConnectedRelee> releesWithPower, Status status)
       {
           this.releesWithPower = releesWithPower;
           this.circuitStatus = status;
       }

       public List<ConnectedRelee> ReleesWithPower
       {
           get { return releesWithPower; }
       }

       public Status CircuitStatus1
       {
           get { return circuitStatus; }
       }
    }

