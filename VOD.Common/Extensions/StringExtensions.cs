namespace VOD.Common.Extensions;

public static class StringExtensions
{

    public static string Truncate(this string str, int maxLength)
    {
        if(string.IsNullOrEmpty(str)) return string.Empty;

        if (str.Length <= maxLength)
            return str;
        else
            return str.Substring(0, maxLength) + "...";

    }


}
