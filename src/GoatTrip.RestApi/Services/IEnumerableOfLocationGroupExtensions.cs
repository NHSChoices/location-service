namespace GoatTrip.RestApi.Services {
    using System.Collections.Generic;
    using System.Linq;
    using DAL.DTOs;

    public static class IEnumerableOfLocationGroupExtensions
    {
        public static bool HasSingleGroup(this IEnumerable<LocationGroup> operand)
        {
            return operand.Count() == 1;
        }
    }
}