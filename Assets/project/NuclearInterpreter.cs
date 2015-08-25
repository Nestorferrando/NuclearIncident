using System;
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


        output.addLine("Syntax error, command not found",ScreenOutput.DEFAULT_CONSOLE_COLOR);
        output.addLine("", ScreenOutput.DEFAULT_CONSOLE_COLOR);

    }


}
