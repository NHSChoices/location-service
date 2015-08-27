

namespace GoatTrip.DAL.DTOs {

    using System;
    using System.Data;
    using System.Linq;
    using System.Collections.Generic;

    public class LocationGroup
    {
        public int LocationId { get; private set; }
        public int LocationsCount { get; private set; }
        public string GroupDescription { get; private set; }
        public Dictionary<LocationDataField, string> GroupFields { get; private set; }

        public LocationGroup(IDataRecord readerDataObject, IEnumerable<LocationQueryField> groupByFields) {
            BuildFromReader(readerDataObject, groupByFields);
        }

        private void BuildFromReader(IDataRecord readerDataObject,
            IEnumerable<LocationQueryField> groupByFields) {

            if (readerDataObject.HasStringColumn("LocationId"))
                LocationId = Convert.ToInt32(readerDataObject["LocationId"].ToString());
            GroupFields = GetGroupedFields(readerDataObject, groupByFields);
            if (readerDataObject.HasStringColumn("Number"))
                LocationsCount = Convert.ToInt32(readerDataObject["Number"].ToString());

            GroupDescription = CreateGroupDescription(readerDataObject, groupByFields);
        }

        private string CreateGroupDescription(IDataRecord readerDataObject,
            IEnumerable<LocationQueryField> groupByFields)
        {
            return AddDeliminatorToGroupDescrioption(GenerateHouseDescription(readerDataObject, groupByFields))
                             + GenerateAddressDescriptionWithoutHouseDetail(readerDataObject, groupByFields);
        }

        private string AddDeliminatorToGroupDescrioption(string description)
        {
            if (description.Length > 0)
                return description + ", ";
            return description;
        }


        private string GenerateAddressDescriptionWithoutHouseDetail(IDataRecord readerDataObject, IEnumerable<LocationQueryField> groupByFields)
        {
            return groupByFields.Where(field => readerDataObject[field.Name] != DBNull.Value
                                                &&
                                                (field.Key != LocationDataField.HouseNumber &&
                                                 field.Key != LocationDataField.HouseSuffix))
                .Select(f => readerDataObject[f.Name].ToString())
                .Aggregate((i, j) => AddDeliminatorToGroupDescrioption(i) + j);
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

    public static class IDataRecordExtensions {

        public static bool HasStringColumn(this IDataRecord dr, string columnName) {
            return dr.HasColumn(columnName) && !string.IsNullOrEmpty(dr[columnName].ToString())
                && dr[columnName] != DBNull.Value;
        }

        public static bool HasColumn(this IDataRecord dr, string columnName) {
            for (int i = 0; i < dr.FieldCount; i++) {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
