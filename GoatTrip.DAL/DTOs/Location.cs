using System;
using System.Data;
using System.Globalization;
using GoatTrip.Common.Formatters;

namespace GoatTrip.DAL.DTOs
{
    public class Location
    {
        public long Id { get; set; }

        public string PostCode { get; private set; }
        public string PostalTown { get; private set; }
        public string OrganisationName { get; private set; }
        public string PostcodeLocator { get; private set; }
        public string BuildingName { get; private set; }
        public string HouseNumber { get; private set; }
        public string TownName { get; private set; }
        public string StreetDescription { get; private set; }
        public string AdministrativeArea { get; private set; }
        public string Localiry { get; private set; }
        public float XCoordinate { get; private set; }
        public float YCoordinate { get; private set; }

        public Location(IDataRecord readerDataObject, IConditionalFormatter<string, string> formatter)
        {
            BuildFromReader(readerDataObject, formatter);
        }

        private void BuildFromReader(IDataRecord readerDataObject, IConditionalFormatter<string, string> formatter)
        {
            if (readerDataObject[LocationFields.AdministrativeArea] != DBNull.Value)
                this.AdministrativeArea = formatter.DetermineConditionsAndFormat(readerDataObject[LocationFields.AdministrativeArea].ToString(),LocationFields.AdministrativeArea);
            if (readerDataObject[LocationFields.BuildingName] != DBNull.Value)
                this.BuildingName = formatter.DetermineConditionsAndFormat(readerDataObject[LocationFields.BuildingName].ToString(),LocationFields.BuildingName);
            if (readerDataObject[LocationFields.BlpuOrganisation] != DBNull.Value)
                this.OrganisationName = formatter.DetermineConditionsAndFormat(readerDataObject[LocationFields.BlpuOrganisation].ToString(),LocationFields.BlpuOrganisation);
            if (readerDataObject[LocationFields.StreetDescription] != DBNull.Value)
                this.StreetDescription = formatter.DetermineConditionsAndFormat(readerDataObject[LocationFields.StreetDescription].ToString(),LocationFields.StreetDescription);
            if (readerDataObject[LocationFields.PaoStartNumber] != DBNull.Value)
                this.HouseNumber = formatter.DetermineConditionsAndFormat(readerDataObject[LocationFields.PaoStartNumber].ToString(),LocationFields.PaoStartNumber);
            if (readerDataObject[LocationFields.Locality] != DBNull.Value)
                this.Localiry = formatter.DetermineConditionsAndFormat(readerDataObject[LocationFields.Locality].ToString(),LocationFields.Locality);
            if (readerDataObject[LocationFields.TownName] != DBNull.Value)
                this.TownName = formatter.DetermineConditionsAndFormat(readerDataObject[LocationFields.TownName].ToString(),LocationFields.TownName);
            if (readerDataObject[LocationFields.PostTown] != DBNull.Value)
                this.PostalTown = formatter.DetermineConditionsAndFormat(readerDataObject[LocationFields.PostTown].ToString(),LocationFields.PostTown);
            if (readerDataObject[LocationFields.Postcode] != DBNull.Value)
                this.PostCode = formatter.DetermineConditionsAndFormat(readerDataObject[LocationFields.Postcode].ToString(),LocationFields.Postcode);
            if (readerDataObject[LocationFields.PostcodeLocator] != DBNull.Value)
                this.PostcodeLocator = formatter.DetermineConditionsAndFormat(readerDataObject[LocationFields.PostcodeLocator].ToString(), LocationFields.PostcodeLocator);
            //TODO: Add formatter for int's below.
            if (readerDataObject[LocationFields.XCoordinate] != DBNull.Value)
                this.XCoordinate = float.Parse(readerDataObject[LocationFields.XCoordinate].ToString(), CultureInfo.InvariantCulture.NumberFormat);
            if (readerDataObject[LocationFields.YCoordinate] != DBNull.Value)
                this.YCoordinate = float.Parse(readerDataObject[LocationFields.YCoordinate].ToString(), CultureInfo.InvariantCulture.NumberFormat);
        }
    }
}
