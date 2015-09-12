using System;
using System.Collections.Generic;

public class IDGenerator
{
    private static readonly Random _random = new Random();
    private static readonly HashSet<String> labels = new HashSet<String>();


    public static String generateID()
    {
        String proposedString = "";
        do
        {
            int num = _random.Next(0, 26); // Zero to 25
            var letter = (char) ('A' + num);
            proposedString = (_random.Next(10, 99) + "" + letter + "" + _random.Next(10, 99));
        } while (labels.Contains(proposedString));
        labels.Add(proposedString);

        return proposedString;
    }
}