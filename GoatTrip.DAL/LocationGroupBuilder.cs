using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoatTrip.DAL.DTOs;
using Lucene.Net.Documents;
using Lucene.Net.Index;

namespace GoatTrip.DAL
{
    public  class LocationGroupBuilder : ILocationGroupBuilder
    {
        public LocationGroup Build(System.Data.IDataRecord readerDataObject, IEnumerable<SqLiteQueryField> groupByFields)
        {
            return BuildFromReader(readerDataObject, groupByFields);
        }

        public LocationGroup Build(Document document, IEnumerable<SqLiteQueryField> groupByFields)
        {
            return BuildFromReader(document, groupByFields);
        }

        private LocationGroup BuildFromReader(IDataRecord readerDataObject,
           IEnumerable<SqLiteQueryField> groupByFields)
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
         IEnumerable<SqLiteQueryField> groupByFields)
        {
            return new LocationGroup()
            {
                GroupFields = GetGroupedFields(document, groupByFields),
                GroupDescription = GenerateGroupDescription(document, groupByFields),
                LocationsCount = 1
            };
        }

        public string GenerateGroupDescription(IDataRecord readerDataObject,
            IEnumerable<SqLiteQueryField> groupByFields)
        {
            return AddDeliminatorToGroupDescrioption(GenerateHouseDescription(readerDataObject, groupByFields))
                             + GenerateAddressDescriptionWithoutHouseDetail(readerDataObject, groupByFields);
        }

        public string GenerateGroupDescription(Document document,
           IEnumerable<SqLiteQueryField> groupByFields)
        {
            return AddDeliminatorToGroupDescrioption(GenerateHouseDescription(document, groupByFields))
                             + GenerateAddressDescriptionWithoutHouseDetail(document, groupByFields);
        }

        private string AddDeliminatorToGroupDescrioption(string description)
        {
            if (description.Length > 0)
                return description + ", ";
            return description;
        }


        private string GenerateAddressDescriptionWithoutHouseDetail(IDataRecord readerDataObject, IEnumerable<SqLiteQueryField> groupByFields)
        {
            return groupByFields.Where(field => readerDataObject[field.Name] != DBNull.Value
                                                &&
                                                (field.Key != LocationDataField.HouseNumber &&
                                                 field.Key != LocationDataField.HouseSuffix))
                .Select(f => readerDataObject[f.Name].ToString())
                .Aggregate((i, j) => AddDeliminatorToGroupDescrioption(i) + j);
        }

        private string GenerateAddressDescriptionWithoutHouseDetail(Document document,
            IEnumerable<SqLiteQueryField> groupByFields)
        {
            return groupByFields.Where(field => document.Get(field.Name) != null
                                               &&
                                               (field.Key != LocationDataField.HouseNumber &&
                                                field.Key != LocationDataField.HouseSuffix))
               .Select(f => document.Get(f.Name).ToString())
               .Aggregate((i, j) => AddDeliminatorToGroupDescrioption(i) + j);
        }

        private string GenerateHouseDescription(IDataRecord readerDataObject, IEnumerable<SqLiteQueryField> groupByFields)
        {
            return String.Join("", groupByFields.Where(field => readerDataObject[field.Name] != DBNull.Value
                                                                &&
                                                                (field.Key == LocationDataField.HouseNumber ||
                                                                 field.Key == LocationDataField.HouseSuffix))
                .Select(f => readerDataObject[f.Name].ToString()));
        }

        private string GenerateHouseDescription(Document document, IEnumerable<SqLiteQueryField> groupByFields)
        {
            return String.Join("", groupByFields.Where(field => document.Get(field.Name) != null
                                                                &&
                                                                (field.Key == LocationDataField.HouseNumber ||
                                                                 field.Key == LocationDataField.HouseSuffix))
                .Select(f => document.Get(f.Name)));
        }

        private Dictionary<LocationDataField, string> GetGroupedFields(IDataRecord readerDataObject,
            IEnumerable<SqLiteQueryField> groupByFields)
        {

            return groupByFields.Where(field => readerDataObject[field.Name] != DBNull.Value)
                .ToDictionary(f => f.Key, f => readerDataObject[f.Name].ToString());
        }

        private Dictionary<LocationDataField, string> GetGroupedFields(Document document,
            IEnumerable<SqLiteQueryField> groupByFields)
        {

            return groupByFields.Where(field => document.Get(field.Name) != null)
                .ToDictionary(f => f.Key, f => document.Get(f.Name));
        }


       
    }
}
