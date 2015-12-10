using System;
using UnityEngine;

public class PumpDrawer
{
    public static void draw(Pump pump, ScreenOutput output)
    {
        var levelBitmap = Resources.Load("pumpBG") as Texture2D;
        Color[] pix = levelBitmap.GetPixels();

        ScreenCharacter[][] outputImage = initializeOutputImage(levelBitmap);


        for (int i = levelBitmap.height - 1; i >= 0; i--)
        {
            for (int j = 0; j < levelBitmap.width; j++)
            {
                outputImage[i][j] = new ScreenCharacter(2, pix[i*levelBitmap.width + j], pix[i*levelBitmap.width + j]);
            }
        }

        printRelee(pump.Circuit.Inputs[0].InputRelee, 21, 10 + 8, outputImage);
        printRelee(pump.Circuit.Inputs[1].InputRelee, 21, 18 + 8, outputImage);
        printRelee(pump.Circuit.Inputs[2].InputRelee, 21, 26 + 8, outputImage);


        printRelee(pump.Circuit.EngineInputs[0].Relee, 21+39, 10 + 8, outputImage);
        printRelee(pump.Circuit.EngineInputs[1].Relee, 21 + 39, 18 + 8, outputImage);
        printRelee(pump.Circuit.EngineInputs[2].Relee, 21 + 39, 26 + 8, outputImage);

        
        for (int i = 0; i < pump.Circuit.ConnectionRelees.Count/2; i++)
        {
            printRelee(pump.Circuit.ConnectionRelees[i], 21+26  , 14 + 28 - 8*i, outputImage);
        }

        for (int i = pump.Circuit.ConnectionRelees.Count / 2; i < pump.Circuit.ConnectionRelees.Count ; i++)
        {
            printRelee(pump.Circuit.ConnectionRelees[i], 21 + 13, 14 + 28 - 8 * (i - pump.Circuit.ConnectionRelees.Count / 2), outputImage);
        }



        drawString(2, 20, "POWER SOURCE", Color.magenta, Color.cyan, outputImage);
        drawString(2, 19, "10 kV", Color.magenta, Color.cyan, outputImage);


        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                outputImage[6 + i][75 + j] = new ScreenCharacter(0, Color.cyan, Color.cyan);
            }
        }

        drawString(75, 7, "PUMP ENGINE", Color.magenta, Color.cyan, outputImage);
        //drawString(75, 7, "PUMP ENGINE", Color.magenta, Color.cyan, outputImage);





        output.addLine("Pump " + pump.StationId + " status at: " + DateTime.Now.ToString("hh:mm:ss"),
            ScreenOutput.DEFAULT_CONSOLE_COLOR);

        for (int i = levelBitmap.height - 1; i >= 0; i--)
        {
            output.addLine(outputImage[i]);
        }
        output.addLine("", ScreenOutput.DEFAULT_CONSOLE_COLOR);
    }

    private static void printRelee(Relee relee, int baseX, int baseY, ScreenCharacter[][] outputImage)
    {
        if (relee.InColor != Color.clear)
        {
            outputImage[baseY][baseX] = new ScreenCharacter(0, relee.InColor, relee.InColor);
            outputImage[baseY][baseX + 1] = new ScreenCharacter(0, relee.InColor, relee.InColor);
        }
        outputImage[baseY][baseX + 2] = new ScreenCharacter(0, Color.white, Color.white);
        outputImage[baseY][baseX + 6] = new ScreenCharacter(0, Color.white, Color.white);
        if (relee.OutColor != Color.clear)
        {
            outputImage[baseY][baseX + 7] = new ScreenCharacter(0, relee.OutColor, relee.OutColor);
            outputImage[baseY][baseX + 8] = new ScreenCharacter(0, relee.OutColor, relee.OutColor);
        }

        if (relee.Closed)
        {
            outputImage[baseY + 1][baseX + 2] = new ScreenCharacter(0, Color.gray, Color.gray);
            outputImage[baseY + 1][baseX + 3] = new ScreenCharacter(0, Color.gray, Color.gray);
            outputImage[baseY + 1][baseX + 4] = new ScreenCharacter(0, Color.gray, Color.gray);
            outputImage[baseY + 1][baseX + 5] = new ScreenCharacter(0, Color.gray, Color.gray);
            outputImage[baseY + 1][baseX + 6] = new ScreenCharacter(0, Color.gray, Color.gray);
        }
        else
        {
            outputImage[baseY + 1][baseX + 2] = new ScreenCharacter(0, Color.gray, Color.gray);
            outputImage[baseY + 2][baseX + 3] = new ScreenCharacter(0, Color.gray, Color.gray);
            outputImage[baseY + 3][baseX + 4] = new ScreenCharacter(0, Color.gray, Color.gray);
        }

        /*
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                outputImage[baseY - 3 + i][baseX - 2 + j] = new ScreenCharacter(0, Color.cyan, Color.cyan);
            }
        }*/


        drawString(baseX - 2, baseY - 2, "RELAY: " + relee.Id, Color.magenta, Color.cyan, outputImage);
        if (relee.Closed)
            drawString(baseX - 2, baseY - 3, "CLOSED", Color.magenta, Color.cyan, outputImage);
        else
        {
            drawString(baseX - 2, baseY - 3, "OPEN", Color.magenta, Color.cyan, outputImage);
        }

    }

    private static void drawString(int x, int y, String str, Color frontColor, Color backgroundColor,
    ScreenCharacter[][] outputImage)
    {
        byte[] encodings = System.Text.Encoding.Default.GetBytes(str);
        for (int i = 0; i < encodings.Length; i++)
        {
            outputImage[y][x + i] = new ScreenCharacter(encodings[i], frontColor, backgroundColor);
        }
    }

    private static ScreenCharacter[][] initializeOutputImage(Texture2D levelBitmap)
    {
        var outputImage = new ScreenCharacter[levelBitmap.height+10][];

        for (int i = 0; i < levelBitmap.height+10; i++)
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