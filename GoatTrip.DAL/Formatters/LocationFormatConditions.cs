using GoatTrip.Common.Formatters;
using GoatTrip.DAL.DTOs;

namespace GoatTrip.DAL.Formatters
{
    public class LocationFormatConditions : IFormatConditions<string>
    {
        public bool ShouldFormat(string inputType)
        {
            return inputType == LocationFields.AdministrativeArea ||
                   inputType == LocationFields.BuildingName ||
                   inputType == LocationFields.BlpuOrganisation ||
                   inputType == LocationFields.StreetDescription ||
                   inputType == LocationFields.Locality ||
                   inputType == LocationFields.TownName ||
                   inputType == LocationFields.PostTown;
        }
    }
}
