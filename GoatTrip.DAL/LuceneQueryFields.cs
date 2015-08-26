using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoatTrip.DAL
{
    public class LuceneQueryFields : ILocationQueryFields
    {
        public SqLiteQueryField HouseNumber { get { return new SqLiteQueryField("StartNumber", LocationDataField.HouseNumber); } }
        public SqLiteQueryField HouseSuffix { get { return new SqLiteQueryField("StartSuffix", LocationDataField.HouseSuffix); } }
        public SqLiteQueryField Town { get { return new SqLiteQueryField("Town", LocationDataField.Town); } }
        public SqLiteQueryField Street { get { return new SqLiteQueryField("Street", LocationDataField.Street); } }
        public SqLiteQueryField AdministrativeArea { get { return new SqLiteQueryField("AdminArea", LocationDataField.AdministrativeArea); } }
        public SqLiteQueryField PostCode { get { return new SqLiteQueryField("Postcode", LocationDataField.PostCode); } }
    }
}



