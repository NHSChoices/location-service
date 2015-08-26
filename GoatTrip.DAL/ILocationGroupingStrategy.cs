

namespace GoatTrip.DAL {

    using System.Collections.Generic;

    public interface ILocationGroupingStrategy {

        IEnumerable<SqLiteQueryField> Fields { get; }
    }
}