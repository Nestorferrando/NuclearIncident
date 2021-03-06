﻿
using System;
using UnityEngine;


public class CentralMapDrawer
    {

        private NuclearGameCore gameCore;

        public CentralMapDrawer(NuclearGameCore gameCore)
        {
            this.gameCore = gameCore;
        }

        public void drawStatus(ScreenOutput output)
        {
            Texture2D levelBitmap = Resources.Load("centralmapBG") as Texture2D;
            Color[] pix = levelBitmap.GetPixels();

            ScreenCharacter[][] outputImage = initializeOutputImage(levelBitmap);


            for (int i = levelBitmap.height - 1; i >= 0; i--)
            {
                for (int j = 0; j < levelBitmap.width; j++)
                {

                    outputImage[i][j] = new ScreenCharacter(2, pix[i * levelBitmap.width + j], pix[i * levelBitmap.width + j]);
                }
            } 

            //--------------------------
            CoolingSystem cooling = gameCore.CoolingSystem;
            drawPipes(cooling, outputImage);
            drawPumps(cooling, outputImage);

            //--------------------------

            output.addLine("Reactor #4 status at: "+System.DateTime.Now.ToString("hh:mm:ss"), ScreenOutput.DEFAULT_CONSOLE_COLOR);

            for (int i = levelBitmap.height-1; i >= 0; i--)
            {
                output.addLine(outputImage[i]);
            }          
            output.addLine("", ScreenOutput.DEFAULT_CONSOLE_COLOR);
        }

    private static void drawPumps(CoolingSystem cooling, ScreenCharacter[][] outputImage)
    {
        foreach (Pump pump in cooling.Pumps)
        {
            int x = (int) pump.Location.x;
            int y = (int) pump.Location.y;

            outputImage[y][x] = new ScreenCharacter(0, Color.green, Color.green);

            outputImage[y - 1][x - 1] = new ScreenCharacter(0, Color.white, Color.white);
            outputImage[y][x - 1] = new ScreenCharacter(0, Color.white, Color.white);
            outputImage[y + 1][x - 1] = new ScreenCharacter(0, Color.white, Color.white);
            outputImage[y - 1][x] = new ScreenCharacter(0, Color.white, Color.white);
            outputImage[y + 1][x] = new ScreenCharacter(0, Color.white, Color.white);
            outputImage[y - 1][x + 1] = new ScreenCharacter(0, Color.white, Color.white);
            outputImage[y][x + 1] = new ScreenCharacter(0, Color.white, Color.white);
            outputImage[y + 1][x + 1] = new ScreenCharacter(0, Color.white, Color.white);


            Color bColor = Color.cyan;
            Color fColor = Color.magenta;

            if (pump.Radiation > RadiationPumps.MAX_RADIATION/10)
            {

                bColor = Color.yellow;
            }

            if (pump.Radiation > RadiationPumps.MAX_RADIATION/2)
            {
                fColor = Color.white;
                bColor = Color.red;
            }

            if (pump.Radiation >= RadiationPumps.MAX_RADIATION) 
            {
                bColor = Color.black;
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j=0;j< 12; j++)
                {
                    outputImage[y + 2 + i][x + 2 + j] = new ScreenCharacter(0, bColor, bColor);
                }
            }
            drawString(x + 2, y + 4, "PUMP: " + pump.StationId, fColor, bColor, outputImage);
            drawString(x + 2, y + 3, "RAD: " + pump.Radiation + " R/h", fColor, bColor, outputImage);
            drawString(x + 2, y + 2, pump.Status.ToString(), fColor, bColor, outputImage);
        }
    }


    private static void drawString(int x, int y, String str, Color frontColor, Color backgroundColor,
        ScreenCharacter[][] outputImage)
    {
        byte[] encodings = System.Text.Encoding.Default.GetBytes(str);
        for (int i = 0; i < encodings.Length; i++)
        {
            outputImage[y][x + i] = new ScreenCharacter(encodings[i],frontColor,backgroundColor);
        }
    }

    private static void drawPipes(CoolingSystem cooling, ScreenCharacter[][] outputImage)
    {
        foreach (Pipe pipe in cooling.Pipes)
        {
            foreach (Vector2 layout in pipe.Layout)
            {
                int x = (int)layout.x;
                int y = (int)layout.y;
                if (pipe.Status == Pipe.PipeStatus.FULL)
                {
                    outputImage[y][x] = new ScreenCharacter(0, Color.blue, Color.blue);
                }
               
                else
                {
                    outputImage[y][x] = new ScreenCharacter(0, Color.white, Color.white);
                }

            }
        }

        foreach (Pipe pipe in cooling.SourcePipes)
        {
            foreach (Vector2 layout in pipe.Layout)
            {
                int x = (int)layout.x;
                int y = (int)layout.y;
                outputImage[y][x] = new ScreenCharacter(0, Color.blue, Color.blue);
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

