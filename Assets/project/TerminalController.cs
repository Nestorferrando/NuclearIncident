using System;
using System.Collections.Generic;
using UnityEngine;

public class TerminalController : MonoBehaviour
{

    public static readonly float TERMINAL_RESPONSE_TIME =0.65f;

    private KeyboardInput keyboardInput;
    private ScreenOutput screenOutput;
    private NuclearInterpreter interpreter;

    private Queue<List<char>> commandQueue;


    // Use this for initialization
    private void Start()
    {
        screenOutput = new ScreenOutput();
        keyboardInput = new KeyboardInput();
        interpreter = new NuclearInterpreter(GetComponent<NuclearGameCore>(),screenOutput);
        screenOutput.Start(GetComponent<Console>());
        commandQueue = new Queue<List<char>>();
        keyboardInput.Start();
    }


    // Update is called once per frame
    private void Update()
    {
        keyboardInput.Update();

        if (keyboardInput.ReturnPressed)
        {
            screenOutput.addLine(new string(keyboardInput.CurrentLine.ToArray()),ScreenOutput.DEFAULT_INPUT_COLOR);
            
            commandQueue.Enqueue(keyboardInput.CurrentLine);
            Invoke("processCommand", TERMINAL_RESPONSE_TIME);
            keyboardInput.lineProcessed();
        }

        addInputToScreen();
        screenOutput.Update();
    }

    private void processCommand()
    {
        interpreter.processUserCommand(commandQueue.Dequeue().ToArray());
    }

    private void addInputToScreen()
    {
        List<Char> input = keyboardInput.CurrentLine;
        int secondSinceLastKeyPressed = (int) (Time.realtimeSinceStartup - keyboardInput.LastTimePressedKeyboard);
        if (secondSinceLastKeyPressed%2 == 0)
        {
            screenOutput.setInputBuffer(new string(input.ToArray()));
        }
        else
        {
            screenOutput.setInputBuffer(new string(input.ToArray()) + "_");
        }
    }
}