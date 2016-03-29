using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class NuclearInterpreter
{
    private NuclearGameCore core;
    private ScreenOutput output;

    private bool firstTimeStatus = true;

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
            show_help();
            return;
        }

        if (new String(command).Equals("STATUS"))

        {
            new CentralMapDrawer(core).drawStatus(output);
            if (firstTimeStatus)
            {
                core.startOverHeat();
            }
            firstTimeStatus = false;
            return;
        }

        if (new String(command).StartsWith("STATUS"))
        {
            show_pump_status(command);
            return;
        }

        if (new String(command).StartsWith("OPEN RELAY"))
        {
            perform_open_relay(command);
            return;
        }

        if (new String(command).StartsWith("CLOSE RELAY"))
        {
            perform_close_relay(command);
            return;
        }

        if (new String(command).StartsWith("START PUMP"))
        {
            perform_start_pump(command);
            return;
        }


        output.addLine("Syntax error, command not found",ScreenOutput.DEFAULT_CONSOLE_COLOR);
        output.addLine("", ScreenOutput.DEFAULT_CONSOLE_COLOR);

    }

    private void perform_start_pump(char[] command)
    {
        String componentId = new String(command).Substring(11);

        List<Pump> pumps = core.CoolingSystem.Pumps;

        Pump selectedPump = null;

        foreach (Pump pump in pumps)
        {
            if (pump.StationId.Equals(componentId)) selectedPump = pump;
        }

        if (selectedPump != null)
        {

            if (selectedPump.Status == Pump.PumpStatus.ERROR)
            {
                output.addLine("Error, pump " + selectedPump.StationId + " is inaccessible", ScreenOutput.DEFAULT_CONSOLE_COLOR);
                return;
            }

            if (selectedPump.Status.Equals(Pump.PumpStatus.WORKING))
            {
                output.addLine("Pump " + selectedPump.StationId + " is already online",
                    ScreenOutput.DEFAULT_CONSOLE_COLOR);
                output.addLine("", ScreenOutput.DEFAULT_CONSOLE_COLOR);
                return;
            }

            if (!selectedPump.Status.Equals(Pump.PumpStatus.READY))
            {
                output.addLine("Pump " + selectedPump.StationId + " cannot be started yet",
                    ScreenOutput.DEFAULT_CONSOLE_COLOR);
                output.addLine("", ScreenOutput.DEFAULT_CONSOLE_COLOR);
                return;
            }

            output.addLine("Starting engine... ", ScreenOutput.DEFAULT_CONSOLE_COLOR);
            core.startPump(selectedPump);
        }
        else
        {
            output.addLine("Pump station not found", ScreenOutput.DEFAULT_CONSOLE_COLOR);
            output.addLine("", ScreenOutput.DEFAULT_CONSOLE_COLOR);
        }
        return;
    }

    private void show_help()
    {
        output.addLine("", ScreenOutput.DEFAULT_CONSOLE_COLOR);
        output.addLine("Control terminal command list", ScreenOutput.DEFAULT_CONSOLE_COLOR);
        output.addLine("================================", ScreenOutput.DEFAULT_CONSOLE_COLOR);
        output.addLine("", ScreenOutput.DEFAULT_CONSOLE_COLOR);
        output.addLine("STATUS : Show power plant general status", ScreenOutput.DEFAULT_CONSOLE_COLOR);
        output.addLine("STATUS [nodeID] : Show status of specific node of the plant", ScreenOutput.DEFAULT_CONSOLE_COLOR);
        output.addLine("----------", ScreenOutput.DEFAULT_CONSOLE_COLOR);
        //output.addLine("OPEN VALVE [ID] : open specific water circuit valve", ScreenOutput.DEFAULT_CONSOLE_COLOR);
        //output.addLine("CLOSE VALVE [ID] : closes specific water circuit valve", ScreenOutput.DEFAULT_CONSOLE_COLOR);
        //output.addLine("----------", ScreenOutput.DEFAULT_CONSOLE_COLOR);
        output.addLine("OPEN RELAY [ID] : Open electric relay", ScreenOutput.DEFAULT_CONSOLE_COLOR);
        output.addLine("CLOSE RELAY [ID] : Close electric relay", ScreenOutput.DEFAULT_CONSOLE_COLOR);
        output.addLine("----------", ScreenOutput.DEFAULT_CONSOLE_COLOR);
        output.addLine("START PUMP [ID] : Activate pump engine", ScreenOutput.DEFAULT_CONSOLE_COLOR);
        output.addLine("----oOo------", ScreenOutput.DEFAULT_CONSOLE_COLOR);

        output.addLine("", ScreenOutput.DEFAULT_CONSOLE_COLOR);
        output.addLine("", ScreenOutput.DEFAULT_CONSOLE_COLOR);

        return;
    }

    private void show_pump_status(char[] command)
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
            if (selectedPump.Status == Pump.PumpStatus.ERROR)
            {
                output.addLine("Error, pump " + selectedPump.StationId + " is inaccessible", ScreenOutput.DEFAULT_CONSOLE_COLOR);
                return;
            }

            PumpDrawer.draw(selectedPump, output);
        }
        else
        {
            output.addLine("Pump station not found", ScreenOutput.DEFAULT_CONSOLE_COLOR);
            output.addLine("", ScreenOutput.DEFAULT_CONSOLE_COLOR);
        }
        return;
    }

    private void perform_open_relay(char[] command)
    {
        String componentId = new String(command).Substring(11);

        List<Pump> pumps = core.CoolingSystem.Pumps;

        foreach (Pump pump in pumps)
        {
            Relee relee = pump.Circuit.getRelee(componentId);
            if (relee != null)
            {
                if (pump.Status.Equals(Pump.PumpStatus.WORKING))
                {
                    output.addLine("Pump " + pump.StationId + " is online, cannot manually access to its circuitry",
                        ScreenOutput.DEFAULT_CONSOLE_COLOR);
                    output.addLine("", ScreenOutput.DEFAULT_CONSOLE_COLOR);
                    return;
                }

                if (pump.Status == Pump.PumpStatus.ERROR)
                {
                    output.addLine("Error, pump " + pump.StationId + " is inaccessible", ScreenOutput.DEFAULT_CONSOLE_COLOR);
                    return;
                }

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

    private void perform_close_relay(char[] command)
    {
        String componentId = new String(command).Substring(12);

        List<Pump> pumps = core.CoolingSystem.Pumps;

        foreach (Pump pump in pumps)
        {
            Relee relee = pump.Circuit.getRelee(componentId);
            if (relee != null)
            {
                if (pump.Status.Equals(Pump.PumpStatus.WORKING))
                {
                    output.addLine("Pump " + pump.StationId + " is online, cannot manually access to its circuitry",
                        ScreenOutput.DEFAULT_CONSOLE_COLOR);
                    output.addLine("", ScreenOutput.DEFAULT_CONSOLE_COLOR);
                    return;
                }

                if (pump.Status == Pump.PumpStatus.ERROR)
                {
                    output.addLine("Error, pump " + pump.StationId + " is inaccessible", ScreenOutput.DEFAULT_CONSOLE_COLOR);
                    return;
                }

                CircuitStatus oldStatus = CircuitUtils.calculateStatus(pump.Circuit);
                relee.Closed = true;
                output.addLine("Relay " + componentId + " closed", ScreenOutput.DEFAULT_CONSOLE_COLOR);
                output.addLine("", ScreenOutput.DEFAULT_CONSOLE_COLOR);

                CircuitStatus status = CircuitUtils.calculateStatus(pump.Circuit);

                if (status.CircuitStatus1.Equals(CircuitStatus.Status.SHORTCIRCUIT))
                {
                    output.addLine("ERROR", ScreenOutput.DEFAULT_CONSOLE_COLOR);
                    output.addLine("Power surge detected on Pump " + pump.StationId + ", opening all relays",
                        ScreenOutput.DEFAULT_CONSOLE_COLOR);
                    pump.Circuit.reset();
                }


                if (status.CircuitStatus1.Equals(CircuitStatus.Status.BAD_PHASE))
                {
                    output.addLine("Warning, phases connected incorrectly. Engine can't start",
                        ScreenOutput.DEFAULT_CONSOLE_COLOR);
                }

                if (oldStatus.CircuitStatus1.Equals(CircuitStatus.Status.OFF) &&
                    status.CircuitStatus1.Equals(CircuitStatus.Status.ON))
                {
                    output.addLine("Engine on Pump " + pump.StationId + " powered, ready to start",
                        ScreenOutput.DEFAULT_CONSOLE_COLOR);
                }

                if (oldStatus.CircuitStatus1.Equals(CircuitStatus.Status.ON) &&
                    status.CircuitStatus1.Equals(CircuitStatus.Status.OFF))
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
}
