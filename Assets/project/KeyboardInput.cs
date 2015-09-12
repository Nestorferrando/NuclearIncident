using System;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput
{

    private static readonly int CHARACTER_REMOVAL_PER_SECOND = 10;

    private List<Char> currentLine;
    private Boolean returnPressed;

    private float lastTimePressedKeyboard;
    private float lastTimeCharRemoved;


    public float LastTimePressedKeyboard
    {
        get { return lastTimePressedKeyboard; }
    }

    public List<char> CurrentLine
    {
        get { return currentLine; }
    }

    public bool ReturnPressed
    {
        get { return returnPressed; }
    }

    public void Start()
    {
        currentLine = new List<Char>();
        returnPressed = false;
    }

    public void lineProcessed()
    {
        currentLine = new List<char>();
        returnPressed = false;
    }


    // Update is called once per frame
    public void Update()
    {
        foreach (KeyCode vKey in Enum.GetValues(typeof (KeyCode)))
        {
            if (Input.GetKeyDown(vKey))
            {
                if (vKey >= KeyCode.A && vKey <= KeyCode.Z)
                {
                    addCharacter(vKey);
                }


                if (vKey >= KeyCode.Alpha0 && vKey <= KeyCode.Alpha9)
                {
                    currentLine.Add(vKey.ToString()[vKey.ToString().Length - 1]);
                }

                if (vKey == KeyCode.Backspace && currentLine.Count > 0 && Time.realtimeSinceStartup - lastTimeCharRemoved > 1.0f / CHARACTER_REMOVAL_PER_SECOND)
                {
                    currentLine.RemoveAt(currentLine.Count - 1);
                    lastTimeCharRemoved = Time.realtimeSinceStartup;
                }

                if (vKey == KeyCode.Space)
                {
                    currentLine.Add(' ');
                }

                if (vKey == KeyCode.Return)
                {
                    returnPressed = true;
                }
                lastTimePressedKeyboard = Time.realtimeSinceStartup;
            }
        }

        processBackspacepressed();
    }

    private void processBackspacepressed()
    {
        if (Input.GetKey(KeyCode.Backspace) && currentLine.Count > 0 &&
            Time.realtimeSinceStartup - lastTimeCharRemoved > 1.0f/CHARACTER_REMOVAL_PER_SECOND)
        {
            currentLine.RemoveAt(currentLine.Count - 1);
            lastTimeCharRemoved = Time.realtimeSinceStartup;
            lastTimePressedKeyboard = Time.realtimeSinceStartup;
        }
    }

    private void addCharacter(KeyCode vKey)
    {
       // if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
       // {
            currentLine.Add(vKey.ToString()[0]);
      //  }
      //  else
    //    {
     //       currentLine.Add(Char.ToLower(vKey.ToString()[0]));
     //   }
    }
}