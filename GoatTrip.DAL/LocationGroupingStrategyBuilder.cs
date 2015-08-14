
namespace GoatTrip.DAL {
    using System.Collections.Generic;

    public class LocationGroupingStrategyBuilder {

        public LocationGroupingStrategyBuilder(ILocationGroupingStrategy groupingStrategy) {
            _fields.AddRange(groupingStrategy.Fields);
        }

        public LocationGroupingStrategyBuilder(LocationQueryField field) {
            _fields.Add(field);
        }

        public LocationGroupingStrategyBuilder ThenBy(LocationQueryField field) {
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

        private readonly List<LocationQueryField> _fields = new List<LocationQueryField>();
    }
}
