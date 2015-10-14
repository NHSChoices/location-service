namespace GoatTrip.RestApi.Services {
    using System.Collections.Generic;
    using Models;

    public interface ILocationSearchPostcodeService {
        IEnumerable<LocationModel> SearchByPostcode(string postcodeQuery);
    }
}