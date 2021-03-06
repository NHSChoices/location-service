﻿using System.Collections.Generic;
using GoatTrip.DAL.DTOs;

namespace GoatTrip.DAL
{
    public interface ILocationRepository
    {
        IEnumerable<Location> FindLocations(string postCode);
        IEnumerable<Location> FindLocationsbyAddress(string addressLookup);
        IEnumerable<LocationGroup> FindLocations(string addressLookup, ILocationGroupingStrategy groupingStrategy);
        Location Get(string id);
    }
}
