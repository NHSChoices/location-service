namespace GoatTrip.RestApi.Services {
    using System.Collections.Generic;
    using Models;

    public interface ILocationSearchPostcodeService {
        IEnumerable<LocationGroupModel> SearchByPostcode(string postcodeQuery);
    }
}