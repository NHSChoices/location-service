using System.Collections.Generic;

namespace GoatTrip.DAL
{
    public class AdhocLocationGroupingStrategy
        : ILocationGroupingStrategy {
        public AdhocLocationGroupingStrategy(IEnumerable<SqLiteQueryField> fields) {
            Fields = fields;
        }

        public IEnumerable<SqLiteQueryField> Fields { get; private set; }
        }
}