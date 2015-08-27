using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoatTrip.DAL
{
    public class LuceneQueryFields : ILocationQueryFields
    {
        public LocationQueryField HouseNumber { get { return new LocationQueryField("StartNumber", LocationDataField.HouseNumber); } }
        public LocationQueryField HouseSuffix { get { return new LocationQueryField("StartSuffix", LocationDataField.HouseSuffix); } }
        public LocationQueryField Town { get { return new LocationQueryField("Town", LocationDataField.Town); } }
        public LocationQueryField Street { get { return new LocationQueryField("Street", LocationDataField.Street); } }
        public LocationQueryField AdministrativeArea { get { return new LocationQueryField("AdminArea", LocationDataField.AdministrativeArea); } }
        public LocationQueryField PostCode { get { return new LocationQueryField("Postcode", LocationDataField.PostCode); } }
    }
}



