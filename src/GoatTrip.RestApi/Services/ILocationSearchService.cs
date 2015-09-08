
namespace GoatTrip.RestApi.Services {
    using System.Collections.Generic;
    using DAL;
    using Models;

    public interface ILocationSearchService {
        IEnumerable<LocationGroupModel> Search(string query, ILocationGroupingStrategy groupingStrategy);

    }
}