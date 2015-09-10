using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GoatTrip.Common.Formatters;
using GoatTrip.DAL.DTOs;
using Lucene.Net.Documents;

namespace GoatTrip.DAL
{
    public class LocationGroupBuilder : ILocationGroupBuilder
    {
        private readonly IConditionalFormatter<string, LocationDataField> _formatter;

        public LocationGroupBuilder(IConditionalFormatter<string, LocationDataField> formatter)
        {
            _formatter = formatter;
        }


        public LocationGroup Build(IDataRecord readerDataObject, IEnumerable<LocationQueryField> groupByFields)
        {
            return BuildFromReader(readerDataObject, groupByFields);
        }

        public LocationGroup Build(Document document, IEnumerable<LocationQueryField> groupByFields)
        {
            return BuildFromReader(document, groupByFields);
        }

        private LocationGroup BuildFromReader(IDataRecord readerDataObject,
            IEnumerable<LocationQueryField> groupByFields)
        {
            var locationsCount = 0;
            if (readerDataObject["Number"] != DBNull.Value)
                locationsCount = Convert.ToInt32(readerDataObject["Number"].ToString());

            return new LocationGroup()
            {
                GroupFields = GetGroupedFields(readerDataObject, groupByFields),
                GroupDescription = GenerateGroupDescription(readerDataObject, groupByFields),
                LocationsCount = locationsCount
            };
        }

        private LocationGroup BuildFromReader(Document document,
            IEnumerable<LocationQueryField> groupByFields)
        {
            var locationId = document.Get("id");
            return new LocationGroup()
            {
                GroupFields = GetGroupedFields(document, groupByFields),
                GroupDescription = GenerateGroupDescription(document, groupByFields),
                LocationId = Convert.ToInt32(locationId),
                LocationsCount = 1
            };
        }

        public string GenerateGroupDescription(IDataRecord readerDataObject,
            IEnumerable<LocationQueryField> groupByFields)
        {
            return AddDeliminatorToGroupDescription(GenerateHouseDescription(readerDataObject, groupByFields))
                   + GenerateAddressDescriptionWithoutHouseDetail(readerDataObject, groupByFields);
        }

        public string GenerateGroupDescription(Document document,
            IEnumerable<LocationQueryField> groupByFields)
        {
            return AddDeliminatorToGroupDescription(GenerateHouseDescription(document, groupByFields))
                   + GenerateAddressDescriptionWithoutHouseDetail(document, groupByFields);
        }

        private string AddDeliminatorToGroupDescription(string description)
        {
            if (description.Length > 0 && !description.TrimEnd().EndsWith(","))
                return description + ", ";
            return description;
        }


        private string GenerateAddressDescriptionWithoutHouseDetail(IDataRecord readerDataObject, IEnumerable<LocationQueryField> groupByFields)
        {
            return groupByFields.Where(field => IsDescriptionField(field) &&
                                                readerDataObject[field.Name] != DBNull.Value
                                                &&
                                                (field.Key != LocationDataField.PrimaryText &&
                                                 field.Key != LocationDataField.SecondaryText &&
                                                 field.Key != LocationDataField.HouseNumber &&
                                                 field.Key != LocationDataField.HouseSuffix))
                .Select(f => _formatter.DetermineConditionsAndFormat(readerDataObject[f.Name].ToString(), f.Key))
                .Aggregate((i, j) => AddDeliminatorToGroupDescription(i) + j);
        }

        private string GenerateAddressDescriptionWithoutHouseDetail(Document document, IEnumerable<LocationQueryField> groupByFields)
        {
            return groupByFields.Where(field => IsDescriptionField(field) &&
                                                document.Get(field.Name) != null &&
                                                (field.Key != LocationDataField.PrimaryText &&
                                                 field.Key != LocationDataField.SecondaryText &&
                                                 field.Key != LocationDataField.HouseNumber &&
                                                 field.Key != LocationDataField.HouseSuffix))
                .Select(f => _formatter.DetermineConditionsAndFormat(document.Get(f.Name).ToString(), f.Key))
                .Aggregate((i, j) => AddDeliminatorToGroupDescription(i) + j);
        }

        private string GenerateHouseDescription(IDataRecord readerDataObject, IEnumerable<LocationQueryField> groupByFields)
        {
            var buildingText = AddDeliminatorToGroupDescription(RetriveValue(groupByFields, readerDataObject, LocationDataField.PrimaryText));
            buildingText += AddDeliminatorToGroupDescription(RetriveValue(groupByFields, readerDataObject, LocationDataField.SecondaryText));
            buildingText += AddDeliminatorToGroupDescription(RetrieveHouseNumber(readerDataObject, groupByFields) +
                            RetriveValue(groupByFields, readerDataObject, LocationDataField.HouseSuffix));

            return buildingText;
        }

        private string RetrieveHouseNumber(IDataRecord readerDataObject, IEnumerable<LocationQueryField> groupByFields)
        {
            var value = RetriveValue(groupByFields, readerDataObject, LocationDataField.HouseNumber);
            return value != "0" ? value : "";
        }

        private string RetrieveHouseNumber(Document document, IEnumerable<LocationQueryField> groupByFields)
        {
            var value = RetriveValue(groupByFields, document, LocationDataField.HouseNumber);
            return value != "0" ? value : "";
        }

        private string RetriveValue(IEnumerable<LocationQueryField> groupByFields, IDataRecord readerDataObject, LocationDataField field)
        {

            var value = groupByFields.Where(f => f.Key == field && readerDataObject[f.Name] != DBNull.Value)
                    .Select(f => readerDataObject[f.Name].ToString())
                    .FirstOrDefault();

            return value ?? "";
        }

        private string RetriveValue(IEnumerable<LocationQueryField> groupByFields, Document document, LocationDataField field)
        {

            var value = groupByFields.Where(f => f.Key == field && document.Get(f.Name) != null)
                    .Select(f => document.Get(f.Name))
                    .FirstOrDefault();
            return value ?? "";
        }

        private string GenerateHouseDescription(Document document, IEnumerable<LocationQueryField> groupByFields)
        {
            var buildingText = AddDeliminatorToGroupDescription(RetriveValue(groupByFields, document, LocationDataField.PrimaryText));
            buildingText += AddDeliminatorToGroupDescription(RetriveValue(groupByFields, document, LocationDataField.SecondaryText));
            buildingText += AddDeliminatorToGroupDescription(RetrieveHouseNumber(document, groupByFields) +
                            RetriveValue(groupByFields, document, LocationDataField.HouseSuffix));

            return buildingText;
        }

        private bool IsDescriptionField(LocationQueryField field)
        {
            return field.GetType() == typeof(LocationDescriptonQueryField);
        }

        private Dictionary<LocationDataField, string> GetGroupedFields(IDataRecord readerDataObject,
            IEnumerable<LocationQueryField> groupByFields)
        {

            return groupByFields.Where(field => readerDataObject[field.Name] != DBNull.Value)
                .ToDictionary(f => f.Key, f => readerDataObject[f.Name].ToString());
        }

        private Dictionary<LocationDataField, string> GetGroupedFields(Document document,
            IEnumerable<LocationQueryField> groupByFields)
        {

            return groupByFields.Where(field => document.Get(field.Name) != null)
                .ToDictionary(f => f.Key, f => document.Get(f.Name));
        }



    }
}
