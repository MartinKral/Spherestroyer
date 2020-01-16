using System;

public static class StringExtensions
{
    public static int ToInt(this string stringValue)
    {
        if (9 < stringValue.Length) throw new Exception($"String {stringValue} is too long to be an int");

        string lookupTable = "0123456789";
        int value = 0;
        int multiplier = 1;
        for (int i = stringValue.Length - 1; 0 <= i; i--)
        {
            int currentValue = lookupTable.IndexOf(stringValue[i]);
            if (currentValue == -1) throw new Exception($"String {stringValue} can't be converted");

            value += currentValue * multiplier;
            multiplier *= 10;
        }

        return value;
    }
}