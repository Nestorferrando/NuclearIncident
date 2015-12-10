using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class NuclearInterpreter
{
    private NuclearGameCore core;
    private ScreenOutput output;

    public NuclearInterpreter(NuclearGameCore core, ScreenOutput output)
    {
        this.core = core;
        this.output = output;
        output.addLine("Balakovo power plant control terminal", ScreenOutput.DEFAULT_CONSOLE_COLOR);
        output.addLine("Version 1.25d", ScreenOutput.DEFAULT_CONSOLE_COLOR);
        output.addLine("----------", ScreenOutput.DEFAULT_CONSOLE_COLOR);
        output.addLine("Assigned reactor: #4", ScreenOutput.DEFAULT_CONSOLE_COLOR);
        output.addLine("----------", ScreenOutput.DEFAULT_CONSOLE_COLOR);
        output.addLine("type HELP to see possible actions", ScreenOutput.DEFAULT_CONSOLE_COLOR);
        output.addLine("", ScreenOutput.DEFAULT_CONSOLE_COLOR);

       // new CentralMapDrawer(core).drawStatus(output);
    }

    public void processUserCommand(char[] command)

    {
        if (new String(command).Equals("HELP"))

        {
            output.addLine("Control terminal command list", ScreenOutput.DEFAULT_CONSOLE_COLOR);
            output.addLine("================================", ScreenOutput.DEFAULT_CONSOLE_COLOR);
            output.addLine("", ScreenOutput.DEFAULT_CONSOLE_COLOR);
            output.addLine("STATUS : Shows power plan general status", ScreenOutput.DEFAULT_CONSOLE_COLOR);
            output.addLine("STATUS [nodeID] : Shows status of specific node of the plant", ScreenOutput.DEFAULT_CONSOLE_COLOR);
            output.addLine("----------", ScreenOutput.DEFAULT_CONSOLE_COLOR);
            output.addLine("OPEN VALVE [ID] : opens specific water circuit valve", ScreenOutput.DEFAULT_CONSOLE_COLOR);
            output.addLine("CLOSE VALVE [ID] : closes specific water circuit valve", ScreenOutput.DEFAULT_CONSOLE_COLOR);
            output.addLine("----------", ScreenOutput.DEFAULT_CONSOLE_COLOR);
            output.addLine("OPEN RELAY [ID] : opens specific electric relay", ScreenOutput.DEFAULT_CONSOLE_COLOR);
            output.addLine("CLOSE RELAY [ID] : closes specific electric relay", ScreenOutput.DEFAULT_CONSOLE_COLOR);
            output.addLine("----------", ScreenOutput.DEFAULT_CONSOLE_COLOR);
            output.addLine("START ENGINE [ID] : start specific electric engine", ScreenOutput.DEFAULT_CONSOLE_COLOR);
            output.addLine("STOP ENGINE [ID] : stops specific electric engine", ScreenOutput.DEFAULT_CONSOLE_COLOR);
            output.addLine("----oOo------", ScreenOutput.DEFAULT_CONSOLE_COLOR);

            output.addLine("", ScreenOutput.DEFAULT_CONSOLE_COLOR);
            output.addLine("", ScreenOutput.DEFAULT_CONSOLE_COLOR);

            return;
        }

        if (new String(command).Equals("STATUS"))

        {
            new CentralMapDrawer(core).drawStatus(output);
            return;
        }

        if (new String(command).StartsWith("STATUS"))
        {
            String componentId = new String(command).Substring(7);

            List<Pump> pumps = core.CoolingSystem.Pumps;

            Pump selectedPump = null;

            foreach (Pump pump in pumps)
            {
                if (pump.StationId.Equals(componentId)) selectedPump = pump;
            }

            if (selectedPump != null)
            {
                PumpDrawer.draw(selectedPump, output);
            }
            else
            {
                output.addLine("Pump station not found", ScreenOutput.DEFAULT_CONSOLE_COLOR);
                output.addLine("", ScreenOutput.DEFAULT_CONSOLE_COLOR);
            }
            return;
        }

        if (new String(command).StartsWith("OPEN RELAY"))
        {
            String componentId = new String(command).Substring(11);

            List<Pump> pumps = core.CoolingSystem.Pumps;

            foreach (Pump pump in pumps)
            {
                Relee relee = pump.Circuit.getRelee(componentId);
                if (relee != null)
                {
                    relee.Closed = false;
                    output.addLine("Relay " + componentId + " opened", ScreenOutput.DEFAULT_CONSOLE_COLOR);
                    output.addLine("", ScreenOutput.DEFAULT_CONSOLE_COLOR);
                    return;
                }
            }

            output.addLine("Relay not found", ScreenOutput.DEFAULT_CONSOLE_COLOR);
            output.addLine("", ScreenOutput.DEFAULT_CONSOLE_COLOR);
            return;
        }

        if (new String(command).StartsWith("CLOSE RELAY"))
        {
            String componentId = new String(command).Substring(12);

            List < Pump > pumps = core.CoolingSystem.Pumps;

            foreach (Pump pump in pumps)
            {
                Relee relee = pump.Circuit.getRelee(componentId);
                if (relee != null)
                {
                    CircuitStatus oldStatus = CircuitUtils.calculateStatus(pump.Circuit);
                    relee.Closed = true;
                    output.addLine("Relay " + componentId + " closed", ScreenOutput.DEFAULT_CONSOLE_COLOR);
                    output.addLine("", ScreenOutput.DEFAULT_CONSOLE_COLOR);

                    CircuitStatus status = CircuitUtils.calculateStatus(pump.Circuit);

                    if (status.CircuitStatus1.Equals(CircuitStatus.Status.SHORTCIRCUIT))
                    {
                        output.addLine("ERROR", ScreenOutput.DEFAULT_CONSOLE_COLOR);
                        output.addLine("Power surge detected on Pump " + pump.StationId + ", opening all relays", ScreenOutput.DEFAULT_CONSOLE_COLOR);
                        pump.Circuit.reset();
                    }


                    if (status.CircuitStatus1.Equals(CircuitStatus.Status.BAD_PHASE))
                    {
                        output.addLine("Warning, phases connected incorrectly. Engine can't start", ScreenOutput.DEFAULT_CONSOLE_COLOR);
                    }

                    if (oldStatus.CircuitStatus1.Equals(CircuitStatus.Status.OFF) && status.CircuitStatus1.Equals(CircuitStatus.Status.ON))
                    {
                        output.addLine("Engine on Pump "+ pump.StationId +" powered, ready to start", ScreenOutput.DEFAULT_CONSOLE_COLOR);
                    }

                    if (oldStatus.CircuitStatus1.Equals(CircuitStatus.Status.ON) && status.CircuitStatus1.Equals(CircuitStatus.Status.OFF))
                    {
                        output.addLine("Engine on Pump " + pump.StationId + " unpowered", ScreenOutput.DEFAULT_CONSOLE_COLOR);
                    }

                    return;
                }
            }

            output.addLine("Relay not found", ScreenOutput.DEFAULT_CONSOLE_COLOR);
            output.addLine("", ScreenOutput.DEFAULT_CONSOLE_COLOR);
            return;
        }


        output.addLine("Syntax error, command not found",ScreenOutput.DEFAULT_CONSOLE_COLOR);
        output.addLine("", ScreenOutput.DEFAULT_CONSOLE_COLOR);

    }


}
