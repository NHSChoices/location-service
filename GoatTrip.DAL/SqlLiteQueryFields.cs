using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoatTrip.DAL
{
    public class SqlIteLocationQueryFields : ILocationQueryFields
    {
        public LocationDescriptonQueryField HouseNumber { get { return new LocationDescriptonQueryField("PAO_START_NUMBER", LocationDataField.HouseNumber); } }
        public LocationDescriptonQueryField HouseSuffix { get { return new LocationDescriptonQueryField("PAO_START_SUFFIX", LocationDataField.HouseSuffix); } }
        public LocationDescriptonQueryField Town { get { return new LocationDescriptonQueryField("TOWN_NAME", LocationDataField.Town); } }
        public LocationDescriptonQueryField Street { get { return new LocationDescriptonQueryField("STREET_DESCRIPTION", LocationDataField.Street); } }
        public LocationDescriptonQueryField AdministrativeArea { get { return new LocationDescriptonQueryField("ADMINISTRATIVE_AREA", LocationDataField.AdministrativeArea); } }
        public LocationDescriptonQueryField PostCode { get { return new LocationDescriptonQueryField("POSTCODE", LocationDataField.PostCode); } }
        public LocationQueryField PostCodeLocator { get { return new LocationQueryField("POSTCODE_Locator", LocationDataField.PostCodeLocator); } }
        public LocationDescriptonQueryField PrimaryText { get { return new LocationDescriptonQueryField("PAO_TEXT", LocationDataField.PrimaryText); } }
        public LocationDescriptonQueryField SecondaryText { get { return new LocationDescriptonQueryField("SAO_TEXT", LocationDataField.SecondaryText); } }

    }
}
