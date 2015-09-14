using System.Globalization;

namespace GoatTrip.Common.Formatters
{
    public class TitleCaseFormatter : IFormatter<string>
    {
        string IFormatter<string>.Format(string input)
        {
            var textInfo = CultureInfo.InvariantCulture.TextInfo;
            return textInfo.ToTitleCase(textInfo.ToLower(input));
        }
    }
}