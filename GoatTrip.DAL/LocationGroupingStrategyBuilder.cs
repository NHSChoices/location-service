
namespace GoatTrip.DAL {
    using System.Collections.Generic;

    public class LocationGroupingStrategyBuilder {

        public LocationGroupingStrategyBuilder(ILocationGroupingStrategy groupingStrategy) {
            _fields.AddRange(groupingStrategy.Fields);
        }

        public LocationGroupingStrategyBuilder(SqLiteQueryField field) {
            _fields.Add(field);
        }

        public LocationGroupingStrategyBuilder ThenBy(SqLiteQueryField field) {
            _fields.Add(field);
            return this;
        }

        public LocationGroupingStrategyBuilder ThenBy(ILocationGroupingStrategy groupingStrategy)
        {
            _fields.AddRange(groupingStrategy.Fields);
            return this;
        }

        public ILocationGroupingStrategy Build() {
            return new AdhocLocationGroupingStrategy(_fields);
        }

        private readonly List<SqLiteQueryField> _fields = new List<SqLiteQueryField>();
    }
}
