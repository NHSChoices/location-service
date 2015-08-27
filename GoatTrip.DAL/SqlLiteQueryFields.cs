using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoatTrip.DAL
{
    public class SqlIteLocationQueryFields : ILocationQueryFields
    {
        public LocationQueryField HouseNumber { get { return new LocationQueryField("PAO_START_NUMBER", LocationDataField.HouseNumber); } }
        public LocationQueryField HouseSuffix { get { return new LocationQueryField("PAO_START_SUFFIX", LocationDataField.HouseSuffix); } }
        public LocationQueryField Town { get { return new LocationQueryField("TOWN_NAME", LocationDataField.Town); } }
        public LocationQueryField Street { get { return new LocationQueryField("STREET_DESCRIPTION", LocationDataField.Street); } }
        public LocationQueryField AdministrativeArea { get { return new LocationQueryField("ADMINISTRATIVE_AREA", LocationDataField.AdministrativeArea); } }
        public LocationQueryField PostCode { get { return new LocationQueryField("POSTCODE", LocationDataField.PostCode); } }
    }
}
