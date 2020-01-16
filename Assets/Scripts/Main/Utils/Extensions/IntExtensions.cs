public static class IntExtensions
{
    public static string ToStringExtension(this int intValue)
    {
        var chars = new[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        var str = string.Empty;
        if (intValue == 0)
        {
            str = chars[0];
        }
        else if (intValue == int.MinValue)
        {
            str = "-2147483648";
        }
        else
        {
            bool isNegative = (intValue < 0);
            if (isNegative)
            {
                intValue = -intValue;
            }

            while (intValue > 0)
            {
                str = chars[intValue % 10] + str;
                intValue /= 10;
            }

            if (isNegative)
            {
                str = "-" + str;
            }
        }

        return str;
    }
}