using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoatTrip.DAL
{
    public interface ILocationQueryFields
    {
         LocationQueryField HouseNumber { get; }
         LocationQueryField HouseSuffix { get; }
         LocationQueryField Town { get; }
         LocationQueryField Street { get; }
         LocationQueryField AdministrativeArea { get; }
         LocationQueryField PostCode { get; }
    }
}
