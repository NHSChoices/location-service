namespace GoatTrip.Common.Collections.Generic {
    using System.Collections.Generic;
    using System.Linq;

    public static class IEnumerableOfLocationGroupExtensions {
        public static bool HasSingleGroup<T>(this IEnumerable<T> operand) {
            return operand.Count() == 1;
        }
    }
}