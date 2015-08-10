using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoatTrip.DAL
{
    public class LocationGroupByStringBuilder
    {
        public LocationGroupByStringBuilder(LocationDataField field)
        {
            _fields.Add(field);
        }

        public Dictionary<LocationDataField, string> GroupByFields
        {
            get
            {
                return _fields.ToDictionary(f => f, f=> _groupFieldMappings[f]);
            }
        }

        private List<LocationDataField> _fields = new List<LocationDataField>();

        public LocationGroupByStringBuilder ThenBy(LocationDataField field)
        {
            _fields.Add(field);
            return this;
        }

        public override string ToString()
        {
            return _fields.Select(f => MapString(f)).Aggregate((i, j) => i + ',' + j);
        }

        private static readonly Dictionary<LocationDataField, string> _groupFieldMappings = new Dictionary
            <LocationDataField, string>
        {
            {LocationDataField.HouseNumber, "PAO_START_NUMBER"},
            {LocationDataField.Town, "TOWN_NAME"},
            {LocationDataField.Street, "STREET_DESCRIPTION"},
            {LocationDataField.AdministrativeArea, "ADMINISTRATIVE_AREA"},
            {LocationDataField.PostCode, "POSTCODE"}
        };

        private string MapString(LocationDataField field)
        {
            string result = "";
            _groupFieldMappings.TryGetValue(field, out result);

            if(string.IsNullOrEmpty(result))
                 throw new ArgumentException("Unkown Data Field");
            return result;
        }
    }
}
