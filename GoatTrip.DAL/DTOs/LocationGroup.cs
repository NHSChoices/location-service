

namespace GoatTrip.DAL.DTOs {

    using System;
    using System.Data;
    using System.Linq;
    using System.Collections.Generic;

    public class LocationGroup
    {
        public int UPRN { get; set; }
        public int LocationsCount { get; set; }
        public string GroupDescription { get;  set; }
        public Dictionary<LocationDataField, string> GroupFields { get;  set; }
    }

    public static class IDataRecordExtensions {

        public static bool HasStringColumn(this IDataRecord dr, string columnName) {
            return dr.HasColumn(columnName) && !string.IsNullOrEmpty(dr[columnName].ToString())
                && dr[columnName] != DBNull.Value;
        }

        public static bool HasColumn(this IDataRecord dr, string columnName) {
            for (int i = 0; i < dr.FieldCount; i++) {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
