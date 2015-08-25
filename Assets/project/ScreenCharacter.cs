using UnityEngine;
using System.Collections;

public class ScreenCharacter
{

    private int glyphIndex;
    private Color glyphColor;
    private Color backgroundColor;


    public ScreenCharacter(int glyphIndex, Color glyphColor, Color backgroundColor)
    {
        this.glyphIndex = glyphIndex;
        this.glyphColor = glyphColor;
        this.backgroundColor = backgroundColor;
    }


    public int GlyphIndex
    {
        get { return glyphIndex; }
    }

    public Color GlyphColor
    {
        get { return glyphColor; }
    }

    public Color BackgroundColor
    {
        get { return backgroundColor; }
    }
}
