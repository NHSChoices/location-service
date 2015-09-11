namespace GoatTrip.Common.Data {
    using System;
    using System.Data;

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