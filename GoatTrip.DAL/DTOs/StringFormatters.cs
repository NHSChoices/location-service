using System.Globalization;

namespace GoatTrip.DAL.DTOs
{
    public static class StringFormatters
    {
        public static string ToTitleCase(this string str)
        {
            var textInfo = CultureInfo.InvariantCulture.TextInfo;
            return textInfo.ToTitleCase(textInfo.ToLower(str));
        }
    }
}