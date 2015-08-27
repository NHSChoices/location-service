

namespace GoatTrip.DAL {

    using System.Collections.Generic;

    public interface ILocationGroupingStrategy {

        IEnumerable<LocationQueryField> Fields { get; }
    }
}