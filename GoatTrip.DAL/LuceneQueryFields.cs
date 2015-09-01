using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoatTrip.DAL
{
    public class LuceneQueryFields : ILocationQueryFields
    {
        public LocationDescriptonQueryField HouseNumber { get { return new LocationDescriptonQueryField("StartNumber", LocationDataField.HouseNumber); } }
        public LocationDescriptonQueryField HouseSuffix { get { return new LocationDescriptonQueryField("StartSuffix", LocationDataField.HouseSuffix); } }
        public LocationDescriptonQueryField Town { get { return new LocationDescriptonQueryField("Town", LocationDataField.Town); } }
        public LocationDescriptonQueryField Street { get { return new LocationDescriptonQueryField("Street", LocationDataField.Street); } }
        public LocationDescriptonQueryField AdministrativeArea { get { return new LocationDescriptonQueryField("AdminArea", LocationDataField.AdministrativeArea); } }
        public LocationDescriptonQueryField PostCode { get { return new LocationDescriptonQueryField("Postcode", LocationDataField.PostCode); } }
        public LocationQueryField PostCodeLocator { get { return new LocationQueryField("PostcodeLocator", LocationDataField.PostCodeLocator); } }


        public LocationDescriptonQueryField PrimaryText { get { return new LocationDescriptonQueryField("PrimaryText", LocationDataField.PrimaryText); } }

        public LocationDescriptonQueryField SecondaryText { get { return new LocationDescriptonQueryField("SecondaryText", LocationDataField.SecondaryText); } }
    }
}



