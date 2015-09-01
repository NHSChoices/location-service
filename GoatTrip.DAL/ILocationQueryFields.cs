using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoatTrip.DAL
{
    public interface ILocationQueryFields
    {
        LocationDescriptonQueryField HouseNumber { get; }
        LocationDescriptonQueryField HouseSuffix { get; }
        LocationDescriptonQueryField Town { get; }
        LocationDescriptonQueryField Street { get; }
        LocationDescriptonQueryField AdministrativeArea { get; }
        LocationDescriptonQueryField PostCode { get; }
        LocationQueryField PostCodeLocator { get; }
    }
}
