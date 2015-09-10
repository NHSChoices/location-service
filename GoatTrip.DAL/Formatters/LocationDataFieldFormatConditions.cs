using GoatTrip.Common.Formatters;

namespace GoatTrip.DAL.Formatters
{
    public class LocationDataFieldFormatConditions : IFormatConditions<LocationDataField>
    {
        bool IFormatConditions<LocationDataField>.ShouldFormat(LocationDataField inputType)
        {
            return inputType != LocationDataField.PostCode;
        }
    }
}