﻿
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
                outputImage[y][x] = new ScreenCharacter(0, Color.white, Color.white);
            }
        }

        foreach (Pipe pipe in cooling.SourcePipes)
        {
            foreach (Vector2 layout in pipe.Layout)
            {
                int x = (int)layout.x;
                int y = (int)layout.y;
                outputImage[y][x] = new ScreenCharacter(0, Color.white, Color.white);
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
