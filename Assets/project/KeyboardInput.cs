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
    private AudioSource audioSource;

    private List<AudioClip> keys;
    private AudioClip returnKey;
    private AudioClip spaceKey;


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

    public void Start(AudioSource audioSource)
    {
        currentLine = new List<Char>();
        returnPressed = false;
        this.audioSource = audioSource;
        this.keys = new List<AudioClip>();
        for (int i = 0; i < 10; i++)
        {
            this.keys.Add(Resources.Load("key"+i) as AudioClip);
        }
        this.returnKey = Resources.Load("return") as AudioClip;
        this.spaceKey = Resources.Load("space") as AudioClip;
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
                int sound = vKey.GetHashCode() % 10;

                if (vKey >= KeyCode.A && vKey <= KeyCode.Z)
                {
                    addCharacter(vKey);
                    audioSource.PlayOneShot(keys[sound]);
                }


                if (vKey >= KeyCode.Alpha0 && vKey <= KeyCode.Alpha9)
                {
                    currentLine.Add(vKey.ToString()[vKey.ToString().Length - 1]);
                    audioSource.PlayOneShot(keys[sound]);
                }

                if (vKey == KeyCode.Backspace)
                {
                    audioSource.PlayOneShot(keys[sound]); 
                }

                if (vKey == KeyCode.Backspace && currentLine.Count > 0 && Time.realtimeSinceStartup - lastTimeCharRemoved > 1.0f / CHARACTER_REMOVAL_PER_SECOND)
                {
                    currentLine.RemoveAt(currentLine.Count - 1);
                    lastTimeCharRemoved = Time.realtimeSinceStartup;
                }

                if (vKey == KeyCode.Space)
                {
                    currentLine.Add(' ');
                    audioSource.PlayOneShot(spaceKey);
                }

                if (vKey == KeyCode.Return)
                {
                    returnPressed = true;
                    audioSource.PlayOneShot(returnKey);
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
   
            currentLine.Add(vKey.ToString()[0]);
    }
}