using System;
using System.Data;
using System.Globalization;

namespace GoatTrip.DAL.DTOs
{
    public class Location
    {
        public int Id { get; set; }

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

        public Location(IDataRecord readerDataObject)
        {
            BuildFromReader(readerDataObject);
        }

        private void BuildFromReader(IDataRecord readerDataObject)
        {
            if (readerDataObject["ADMINISTRATIVE_AREA"] != DBNull.Value) 
                this.AdministrativeArea = readerDataObject["ADMINISTRATIVE_AREA"].ToString();
            if (readerDataObject["BUILDING_NAME"] != DBNull.Value) 
                this.BuildingName = readerDataObject["BUILDING_NAME"].ToString();
            if (readerDataObject["BLPU_ORGANISATION"] != DBNull.Value) 
                this.OrganisationName = readerDataObject["BLPU_ORGANISATION"].ToString();
            if (readerDataObject["STREET_DESCRIPTION"] != DBNull.Value)
                this.StreetDescription = readerDataObject["STREET_DESCRIPTION"].ToString();
            if (readerDataObject["PAO_START_NUMBER"] != DBNull.Value)
                this.HouseNumber = readerDataObject["PAO_START_NUMBER"].ToString();
            if (readerDataObject["LOCALITY"] != DBNull.Value) 
                this.Localiry = readerDataObject["LOCALITY"].ToString();
            if (readerDataObject["TOWN_NAME"] != DBNull.Value) 
                this.TownName = readerDataObject["TOWN_NAME"].ToString();
            if (readerDataObject["POST_TOWN"] != DBNull.Value) 
                this.PostalTown = readerDataObject["POST_TOWN"].ToString();
            if (readerDataObject["POSTCODE"] != DBNull.Value)
                this.PostCode = readerDataObject["POSTCODE"].ToString();
            if (readerDataObject["POSTCODE_LOCATOR"] != DBNull.Value)
                this.PostcodeLocator = readerDataObject["POSTCODE_LOCATOR"].ToString().Replace(" ", "").ToLower();
            if (readerDataObject["X_COORDINATE"] != DBNull.Value) 
                this.XCoordinate = float.Parse(readerDataObject["X_COORDINATE"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
            if (readerDataObject["Y_COORDINATE"] != DBNull.Value) 
                this.YCoordinate = float.Parse(readerDataObject["Y_COORDINATE"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
        }
    }
}
