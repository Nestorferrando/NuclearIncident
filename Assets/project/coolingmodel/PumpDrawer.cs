using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class PumpDrawer
{

    public static void draw(Pump pump, ScreenOutput output)
    {
        output.addLine("Pump " + pump.PumpId + " status at: " + System.DateTime.Now.ToString("hh:mm:ss"),
            ScreenOutput.DEFAULT_CONSOLE_COLOR);

        Texture2D levelBitmap = Resources.Load("pumpBG") as Texture2D;
        Color[] pix = levelBitmap.GetPixels();

        ScreenCharacter[][] outputImage = initializeOutputImage(levelBitmap);


        for (int i = levelBitmap.height - 1; i >= 0; i--)
        {
            for (int j = 0; j < levelBitmap.width; j++)
            {

                outputImage[i][j] = new ScreenCharacter(2, pix[i*levelBitmap.width + j], pix[i*levelBitmap.width + j]);
            }
        }

    }

    private static ScreenCharacter[][] initializeOutputImage(Texture2D levelBitmap)
    {
        ScreenCharacter[][] outputImage = new ScreenCharacter[levelBitmap.height][];

        for (int i = 0; i < levelBitmap.height; i++)
        {
            outputImage[i] = new ScreenCharacter[levelBitmap.width];
            for (int j = 0; j < levelBitmap.width; j++)
            {
                outputImage[i][j] = new ScreenCharacter(0, Color.black, Color.black);
            }
        }

        return outputImage;

    }
}

