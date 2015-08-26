using System.Collections.Generic;
using System.Linq;

namespace GoatTrip.DAL
{
    public class SqLiteQueryField {

        public string Name { get; private set; }

        public LocationDataField Key { get; private set; }

        internal SqLiteQueryField(string name, LocationDataField key) {
            Name = name;
            Key = key;
        }

        public static string Concatenate(IEnumerable<SqLiteQueryField> fields) {
            return fields.Select(f => f.Name).Aggregate((i, j) => i + ',' + j);
        }

        public static string Concatenate(IEnumerable<SqLiteQueryField> fields, string aliasPrefix)
        {
            return fields.Select(f => aliasPrefix +"." +f.Name).Aggregate((i, j) => i + ',' + j);
        }

    }
}