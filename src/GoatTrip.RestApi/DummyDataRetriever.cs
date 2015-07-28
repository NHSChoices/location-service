namespace GoatTrip.RestApi {
    using System.Collections.Generic;
    using Models;
    using Services;

    public class DummyDataRetriever
        : ILocationDataRetriever {
        public IEnumerable<LocationModel> RetrieveAll() {
            return new List<LocationModel> {
                new LocationModel {
                    Postcode = "SO111XX"
                },
                new LocationModel {
                    Postcode = "SO222XX"
                },
                new LocationModel {
                    Postcode = "SO222XX"
                },
                new LocationModel {
                    Postcode = "SO222XX",
                    Coordinate = new CoordinateModel(123, 123)
                },
                new LocationModel {
                    Postcode = "SO333XX",
                    Locality = "Southampton"
                },
                new LocationModel {
                    Postcode = "SO333XX",
                    Locality = "Southampton"
                }

            };
        }
    }
}