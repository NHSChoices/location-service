using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoatTrip.DAL
{
    public class SqlIteLocationQueryFields : ILocationQueryFields
    {
        public SqLiteQueryField HouseNumber { get { return new SqLiteQueryField("PAO_START_NUMBER", LocationDataField.HouseNumber); } }
        public SqLiteQueryField HouseSuffix { get { return new SqLiteQueryField("PAO_START_SUFFIX", LocationDataField.HouseSuffix); } }
        public SqLiteQueryField Town { get { return new SqLiteQueryField("TOWN_NAME", LocationDataField.Town); } }
        public SqLiteQueryField Street { get { return new SqLiteQueryField("STREET_DESCRIPTION", LocationDataField.Street); } }
        public SqLiteQueryField AdministrativeArea { get { return new SqLiteQueryField("ADMINISTRATIVE_AREA", LocationDataField.AdministrativeArea); } }
        public SqLiteQueryField PostCode { get { return new SqLiteQueryField("POSTCODE", LocationDataField.PostCode); } }
    }
}
