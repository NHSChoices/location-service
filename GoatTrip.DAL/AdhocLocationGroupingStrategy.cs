using System.Collections.Generic;

namespace GoatTrip.DAL
{
    public class AdhocLocationGroupingStrategy
        : ILocationGroupingStrategy {
        public AdhocLocationGroupingStrategy(IEnumerable<LocationQueryField> fields) {
            Fields = fields;
        }

        public IEnumerable<LocationQueryField> Fields { get; private set; }
        }
}