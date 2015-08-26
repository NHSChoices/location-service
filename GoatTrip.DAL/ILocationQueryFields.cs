using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoatTrip.DAL
{
    public interface ILocationQueryFields
    {
         SqLiteQueryField HouseNumber { get; }
         SqLiteQueryField HouseSuffix { get; }
         SqLiteQueryField Town { get; }
         SqLiteQueryField Street { get; }
         SqLiteQueryField AdministrativeArea { get; }
         SqLiteQueryField PostCode { get; }
    }
}
