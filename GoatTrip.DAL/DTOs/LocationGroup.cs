

namespace GoatTrip.DAL.DTOs {

    using System;
    using System.Data;
    using System.Linq;
    using System.Collections.Generic;

    public class LocationGroup
    {
        public int LocationsCount { get; private set; }
        public string GroupDescription { get; private set; }
        public Dictionary<LocationDataField, string> GroupFields { get; private set; }

        public LocationGroup(IDataRecord readerDataObject, IEnumerable<LocationQueryField> groupByFields) {
            BuildFromReader(readerDataObject, groupByFields);
        }

        private void BuildFromReader(IDataRecord readerDataObject,
            IEnumerable<LocationQueryField> groupByFields) {

            GroupFields = GetGroupedFields(readerDataObject, groupByFields);
            if (readerDataObject["Number"] != DBNull.Value)
                LocationsCount = Convert.ToInt32(readerDataObject["Number"].ToString());

            GroupDescription = CreateGroupDescription(readerDataObject, groupByFields);
        }

        private string CreateGroupDescription(IDataRecord readerDataObject,
            IEnumerable<LocationQueryField> groupByFields)
        {

            return GenerateHouseDescription(readerDataObject, groupByFields)
                            + ", " + GenerateAddressDescriptionWithoutHouseDetail(readerDataObject, groupByFields);
        }

        private string GenerateAddressDescriptionWithoutHouseDetail(IDataRecord readerDataObject, IEnumerable<LocationQueryField> groupByFields)
        {
            return groupByFields.Where(field => readerDataObject[field.Name] != DBNull.Value
                                                &&
                                                (field.Key != LocationDataField.HouseNumber &&
                                                 field.Key != LocationDataField.HouseSuffix))
                .Select(f => readerDataObject[f.Name].ToString())
                .Aggregate((i, j) => i + ", " + j);
        }

        private string GenerateHouseDescription(IDataRecord readerDataObject, IEnumerable<LocationQueryField> groupByFields)
        {
            return String.Join("", groupByFields.Where(field => readerDataObject[field.Name] != DBNull.Value
                                                                &&
                                                                (field.Key == LocationDataField.HouseNumber ||
                                                                 field.Key == LocationDataField.HouseSuffix))
                .Select(f => readerDataObject[f.Name].ToString()));
        }

        private Dictionary<LocationDataField, string> GetGroupedFields(IDataRecord readerDataObject,
            IEnumerable<LocationQueryField> groupByFields) {

            return groupByFields.Where(field => readerDataObject[field.Name] != DBNull.Value)
                .ToDictionary(f => f.Key, f => readerDataObject[f.Name].ToString()); 
        }
    }
}
