using System.Collections.Generic;
using GoatTrip.DAL;
using GoatTrip.RestApi.Models;

namespace GoatTrip.RestApi.Services {
    public interface ILocationSearchService {
        IEnumerable<LocationGroupModel> Search(string query, ILocationGroupingStrategy groupingStrategy);

    }
    public interface ILocationSearchPostcodeService {
        IEnumerable<LocationGroupModel> SearchByPostcode(string postcodeQuery);
    }

    public interface ILocationRetrievalService {
        LocationModel Get(string id);
    }
}