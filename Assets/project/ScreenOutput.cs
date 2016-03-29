using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ScreenOutput
{
    public static readonly float LINES_PER_SECOND = 25;

    public static readonly Color DEFAULT_INPUT_COLOR = Color.green;
    public static readonly Color DEFAULT_CONSOLE_COLOR = Color.white;

    private readonly Queue<ScreenCharacter[]> inernalBuffer = new Queue<ScreenCharacter[]>();
    private readonly List<ScreenCharacter[]> renderBuffer = new List<ScreenCharacter[]>();
    private AudioSource audioSource;
    private Console console;
    private int frameBufferHeight;
    private String inputBuffer = "";

    private int internalLineIndex;

    private float lastLineAdd;
    private AudioClip longRenderClip;
    private int renderLineIndex;

    public void Start(Console _console, AudioSource audioSource)
    {
        // Getting a reference on the Console component :
        console = _console;
        console.GraphicsOnly = true;

        // ... and for example :
        console.ClearGlyph = 0;
        console.ClearGlyphColor = Color.black;
        console.ClearBackgroundColor = Color.black;
        console.GraphicsOnly = true;

        frameBufferHeight = console.height - 1;
        lastLineAdd = Time.realtimeSinceStartup;

        this.audioSource = audioSource;
        this.audioSource.loop = true;
        longRenderClip = Resources.Load("longRender") as AudioClip;
        this.audioSource.clip = longRenderClip;
    }


    public void addLine(ScreenCharacter[] line)
    {
        inernalBuffer.Enqueue(line);

        if (inernalBuffer.Count > 5 && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void addLine(String inLine, Color color)
    {
        var line = new ScreenCharacter[inLine.Length];
        byte[] encodings = Encoding.Default.GetBytes(inLine);
        for (int i = 0; i < inLine.Length; i++)
        {
            line[i] = new ScreenCharacter(encodings[i], color, Color.black);
        }

        inernalBuffer.Enqueue(line);

        if (inernalBuffer.Count > 5 && !audioSource.isPlaying)
        {
            audioSource.Play();
        }

    }


    public void setInputBuffer(String line)
    {
        inputBuffer = line;
    }


    public void Update()
    {
        updateBuffer();
        // Drawing in the console begins here :
        console.PrepareUpdate();
        console.Clear(); // clearing the console
        renderizeBuffer();
        renderizeInput();
        console.EndUpdate();
    }

    private void renderizeInput()
    {
        console.GlyphColor = DEFAULT_INPUT_COLOR;
        console.BackgroundColor = Color.black;
        console.DrawString(0, 0, inputBuffer);
    }

    private void renderizeBuffer()
    {
        for (int i = 0; i < Math.Min(frameBufferHeight, renderBuffer.Count); i++)
        {
            for (int j = 0; j < renderBuffer[i].Length; j++)
            {
                if (renderBuffer[i][j] != null)
                {
                    console.Glyph = renderBuffer[i][j].GlyphIndex;
                    console.GlyphColor = renderBuffer[i][j].GlyphColor;
                    console.BackgroundColor = renderBuffer[i][j].BackgroundColor;
                    console.SetTile(j, i + 1);
                }
            }
        }
    }

    private void updateBuffer()
    {
        if (inernalBuffer.Count > 0 && Time.realtimeSinceStartup - lastLineAdd > (1/LINES_PER_SECOND))
        {
            renderBuffer.Insert(0, inernalBuffer.Dequeue());
            lastLineAdd = Time.realtimeSinceStartup;
            if (renderBuffer.Count > frameBufferHeight)
            {
                renderBuffer.RemoveAt(renderBuffer.Count - 1);
            }

            if (inernalBuffer.Count == 0 && audioSource.isPlaying)
            {
                audioSource.Stop();
            }

        }
    }
}