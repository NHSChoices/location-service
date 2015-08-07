using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoatTrip.DAL.DTOs
{
    public class LocationGroup
    {
        public int LocationsCount { get; private set; }
        public string GroupDescription { get; private set; }
        public Dictionary<LocationDataField, string> GroupFields { get; private set; }

        public LocationGroup(IDataRecord readerDataObject, LocationGroupByStringBuilder locationGroupByStringBuilder)
        {
            BuildFromReader(readerDataObject, locationGroupByStringBuilder);
        }

        private void BuildFromReader(IDataRecord readerDataObject, LocationGroupByStringBuilder locationGroupByStringBuilder)
        {
            this.GroupFields = GetGroupedFields(readerDataObject, locationGroupByStringBuilder);
            if (readerDataObject["Number"] != DBNull.Value)
                this.LocationsCount = Convert.ToInt32(readerDataObject["Number"].ToString());

            this.GroupDescription = CreateGroupDescription(readerDataObject, locationGroupByStringBuilder);
        }

        private string CreateGroupDescription(IDataRecord readerDataObject,
            LocationGroupByStringBuilder locationGroupByStringBuilder)
        {
            return locationGroupByStringBuilder.GroupByFields
                .Where(field => readerDataObject[field.Value] != DBNull.Value)
                .Select(f => readerDataObject[f.Value].ToString())
                .Aggregate((i, j) => i + ", " + j);
        }

        private Dictionary<LocationDataField, string> GetGroupedFields(IDataRecord readerDataObject,
           LocationGroupByStringBuilder locationGroupByStringBuilder)
        {

            return locationGroupByStringBuilder.GroupByFields
                .Where(field => readerDataObject[field.Value] != DBNull.Value)
                .ToDictionary(f => f.Key, f => readerDataObject[f.Value].ToString()); 
        }
    }
}
